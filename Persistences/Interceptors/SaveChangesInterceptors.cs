using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Domains;
using Persistences.Histories;

namespace Persistences.Interceptors
{
    internal class SaveChangesInterceptors(MongoDbContext context, IHttpContextAccessor accessor)
        : ISaveChangesInterceptor
    {
        public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            // OAUth 2.0 / 2.1


            var entries = eventData.Context.ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable auditable)
                {
                    var userId = GetUserId();
                    if (entry.State == EntityState.Added)
                    {
                        auditable.CreatedDate = DateTime.UtcNow;
                        auditable.UserId = userId;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        auditable.UpdatedDate = DateTime.UtcNow;
                    }
                }

                if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                {
                    context.History.Add(new History
                    {
                        TableName = entry.Metadata.GetTableName() ?? string.Empty,
                        OldRow = SerializeChangedValues(entry.OriginalValues, entry.CurrentValues, changed: false),
                        NewRow = SerializeChangedValues(entry.OriginalValues, entry.CurrentValues, changed: true),
                        CreatedDate = DateTime.UtcNow,
                        UserId = GetUserId()
                    });
                }

                else if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    context.History.Add(new History
                    {
                        TableName = entry.Metadata.GetTableName() ?? string.Empty,
                        OldRow = null,
                        NewRow = SerializeValues(entry.CurrentValues),
                        CreatedDate = DateTime.UtcNow,
                        UserId = GetUserId()
                    });
                }
                else if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Deleted)
                {
                    context.History.Add(new History
                    {
                        TableName = entry.Metadata.GetTableName() ?? string.Empty,
                        OldRow = SerializeValues(entry.OriginalValues),
                        NewRow = null,
                        CreatedDate = DateTime.UtcNow,
                        UserId = GetUserId()
                    });
                }
            }

            return result;
        }

        private static string SerializeValues(PropertyValues values)
        {
            var dict = values.Properties.ToDictionary(p => p.Name, p => values[p.Name]);
            return System.Text.Json.JsonSerializer.Serialize(dict);
        }

        // Returns old (changed: false) or new (changed: true) values for only modified properties
        private static string SerializeChangedValues(PropertyValues originalValues, PropertyValues currentValues,
            bool changed)
        {
            var dict = originalValues.Properties
                .Where(p => !Equals(originalValues[p.Name], currentValues[p.Name]))
                .ToDictionary(p => p.Name, p => changed ? currentValues[p.Name] : originalValues[p.Name]);
            return System.Text.Json.JsonSerializer.Serialize(dict);
        }

        private int GetUserId()
        {
            return accessor.HttpContext?.User?.Identity?.IsAuthenticated == true
                ? int.Parse(accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0")
                : 0;
        }


        public int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            context.SaveChanges();


            return result;
        }
    }
}
