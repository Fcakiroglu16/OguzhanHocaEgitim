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
        public DbSet<ProductFullModel> ProductFullModels { get; set; }

        public DbSet<VProductFullModel> VProductFullModels { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Database Connection String
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductFullModel>().HasNoKey();
            modelBuilder.Entity<ProductFullModel>().ToTable("ProductFullModels", t => t.ExcludeFromMigrations());


            modelBuilder.Entity<VProductFullModel>().HasNoKey().ToView("v_productList");


            //conventions
            //attribute
            // fluent api ( best practice )
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
