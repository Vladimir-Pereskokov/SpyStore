﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SpyStore.Models;
using SpyStore.Models.Entities;



namespace SpyStore.DAL.EF
{
    public class StoreContext : DbContext
    {
        private Boolean  prepareTests = false;

        public StoreContext()         : this(false) { }


        public StoreContext(bool prepareTests) : base()
        { this.prepareTests = prepareTests;
            if (!prepareTests) base.Database.Migrate();
        }


        public StoreContext(DbContextOptions options) : base(options)
        {
            try
            { base.Database.Migrate(); }
            catch
            { throw; }

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(@"Server=z600\SQL2016ENT;Database=SpyStore;Integrated Security=False;MultipleActiveResultSets=True;User ID=sa;Password=qazsxdrnewqwerT7;",
                     (options) => {
                         options.ExecutionStrategy(c => new MyExecutionStrategy(c, prepareTests));                         
                     });

            }

        }

        private DbSet<Category> _Cats;
        public DbSet<Category> Categories
        {
            get
            {
                return _Cats;
            }

            set
            {
                _Cats = value;
            }
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ShoppingCartRecord> ShoppingCartRecords { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(c => c.EmailAddress).HasName("IDX_EmailAddress").IsUnique();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.OrderDate)
                  .HasDefaultValueSql("getdate()")
                  .HasColumnType("datetime");

                entity.Property(o => o.ShipDate)
                .HasDefaultValueSql("getdate()")
                .HasColumnType("datetime");

                entity.Property(o => o.OrderTotal)
                .HasColumnType("money")
                .HasComputedColumnSql("Store.GetOrderTotal([Id])");



            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(od => od.UnitCost)
                .HasColumnType("money");
                entity.Property(od => od.LineItemTotal)
                .HasColumnType("money")
                .HasComputedColumnSql("[Quantity]*[UnitCost]");
            }
            );

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.UnitCost)
                .HasColumnType("money");
                entity.Property(p => p.UnitPrice)
                .HasColumnType("money");
            }
            );

            modelBuilder.Entity<ShoppingCartRecord>(entity =>
            {
                entity.HasIndex(r => new { r.Id, r.CustomerId, r.ProductId })
                 .HasName("IDX_ShoppingCart")
                 .IsUnique();

                entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Quantity)
                .HasDefaultValue(1);
            }
            );

        }




    }
}

