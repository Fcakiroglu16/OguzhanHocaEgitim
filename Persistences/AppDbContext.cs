using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Domains;
using Microsoft.EntityFrameworkCore;
using Persistences.EntityConfiguration;

namespace Persistences
{
    //Scope
    public class AppDbContext(DbContextOptions<AppDbContext> option) : DbContext(option)
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Database Connection String
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //conventions
            //attribute
            // fluent api ( best practice )
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
