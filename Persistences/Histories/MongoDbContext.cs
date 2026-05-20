using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistences.Histories
{
    //  1 kalem 1 1000  2  abc
    //  1 kalem 2 1000 2 abc
    public class History
    {
        public string TableName { get; set; } = null!;


        public string OldRow { get; set; } = null!;

        public string NewRow { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }
    }


    public class MongoDbContext(DbContextOptions<MongoDbContext> options) : DbContext(options)
    {
        public static MongoDbContext Create(IMongoDatabase database)
        {
            var optionsBuilder =
                new DbContextOptionsBuilder<MongoDbContext>().UseMongoDB(database.Client,
                    database.DatabaseNamespace.DatabaseName);


            return new MongoDbContext(optionsBuilder.Options);
        }

        public DbSet<History> History { get; set; }
    }
}
