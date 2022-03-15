using System;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.ChildrenItems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Orders;
using Core.Entities.Discounts;

namespace Infrastructure.Data
{
    public class HappyKidsContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, 
        IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public HappyKidsContext(DbContextOptions<HappyKidsContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            
            modelBuilder.Entity<ApplicationRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            
            modelBuilder.Entity<ClientOrder>()
                .HasMany(x => x.OrderChildrenItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<ClientOrder>()
                .OwnsOne(s => s.ShippingAddress, a => {a.WithOwner();});

            modelBuilder.Entity<OrderChildrenItem>()
                .OwnsOne(s => s.BasketChildrenItemOrdered, io => {io.WithOwner();});
            
            modelBuilder.Entity<CategoryDiscount>()
                .HasKey(x => new { x.CategoryId, x.DiscountId }); 
            
            modelBuilder.Entity<ChildrenItemCategory>()
                .HasKey(x => new { x.ChildrenItemId, x.CategoryId }); 

            modelBuilder.Entity<ChildrenItemDiscount>()
                .HasKey(x => new { x.ChildrenItemId, x.DiscountId }); 

            modelBuilder.Entity<ChildrenItemManufacturer>()
                .HasKey(x => new { x.ChildrenItemId, x.ManufacturerId }); 

            modelBuilder.Entity<ChildrenItemTag>()
                .HasKey(x => new { x.ChildrenItemId, x.TagId });

            modelBuilder.Entity<ChildrenItemWarehouse>()
                .HasKey(x => new { x.ChildrenItemId, x.WarehouseId }); 
            
            modelBuilder.Entity<ManufacturerDiscount>()
                .HasKey(x => new { x.ManufacturerId, x.DiscountId }); 
        }
            public DbSet<Account> Accounts { get; set; }
            public DbSet<Address> Addresses { get; set; }
            public DbSet<Branch> Branches { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<CategoryDiscount> CategoryDiscounts { get; set; }
            public DbSet<ChildrenItem> ChildrenItems { get; set; }
            public DbSet<ChildrenItemCategory> ChildrenItemCategories { get; set; }
            public DbSet<ChildrenItemDiscount> ChildrenItemDiscounts { get; set; }
            public DbSet<ChildrenItemManufacturer> ChildrenItemManufacturers { get; set; }
            public DbSet<ChildrenItemTag> ChildrenItemTags { get; set; }
            public DbSet<ChildrenItemWarehouse> ChildrenItemWarehouses { get; set; }
            public DbSet<ClientOrder> ClientOrders { get; set; }
            public DbSet<OrderChildrenItem> OrderChildrenItems { get; set; }
            public DbSet<Country> Countries { get; set; }
            public DbSet<Discount> Discounts { get; set; }
            public DbSet<Manufacturer> Manufacturers { get; set; }
            public DbSet<ManufacturerDiscount> ManufacturerDiscounts { get; set; }
            public DbSet<OrderStatus> OrderStatuses { get; set; }
            public DbSet<PaymentOption> PaymentOptions { get; set; }
            public DbSet<ShippingOption> ShippingOptions { get; set; }
            public DbSet<Tag> Tags { get; set; }
            public DbSet<Warehouse> Warehouses { get; set; }
    }
}





