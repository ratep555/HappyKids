using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.BirthdayOrders;
using Core.Entities.ChildrenItems;
using Core.Entities.Discounts;
using Core.Entities.Identity;
using Core.Entities.Orders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class HappyKidsContextSeed
    {
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager)
        {
            var roles = new List<ApplicationRole>
            {
                new ApplicationRole{Name = "Admin"},
                new ApplicationRole{Name = "Manager"},
                new ApplicationRole{Name = "Client"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            if (await userManager.Users.AnyAsync()) return;
            
            var admin = new ApplicationUser
            {
                DisplayName = "admin",
                Email = "bob@test.com",
                UserName = "admin",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");               
            await userManager.AddToRolesAsync(admin, new[] {"Admin"});                                        
        }

        public static async Task SeedEntitiesAsync(HappyKidsContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Accounts.Any())
                {
                    var accountsData = File.ReadAllText("../Infrastructure/Data/SeedData/accounts.json");
                    var accounts = JsonSerializer.Deserialize<List<Account>>(accountsData);

                    foreach (var item in accounts)
                    {
                        context.Accounts.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.BirthdayPackages.Any())
                {
                    var birthdaypackagesData = File.ReadAllText("../Infrastructure/Data/SeedData/birthdaypackages.json");
                    var birthdaypackages = JsonSerializer.Deserialize<List<BirthdayPackage>>(birthdaypackagesData);

                    foreach (var item in birthdaypackages)
                    {
                        context.BirthdayPackages.Add(item);
                    }
                    await context.SaveChangesAsync();
                } 

                if (!context.Countries.Any())
                {
                    var countriesData = File.ReadAllText("../Infrastructure/Data/SeedData/countries.json");
                    var countries = JsonSerializer.Deserialize<List<Country>>(countriesData);

                    foreach (var item in countries)
                    {
                        context.Countries.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.Categories.Any())
                {
                    var categoriesData = File.ReadAllText("../Infrastructure/Data/SeedData/categories.json");
                    var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

                    foreach (var item in categories)
                    {
                        context.Categories.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.ChildrenItems.Any())
                {
                    var childrenitemsData = File.ReadAllText("../Infrastructure/Data/SeedData/childrenitems.json");
                    var childrenitems = JsonSerializer.Deserialize<List<ChildrenItem>>(childrenitemsData);

                    foreach (var item in childrenitems)
                    {
                        context.ChildrenItems.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                
                if (!context.Branches.Any())
                {
                    var branchesData = File.ReadAllText("../Infrastructure/Data/SeedData/branches.json");
                    var branches = JsonSerializer.Deserialize<List<Branch>>(branchesData);

                    foreach (var item in branches)
                    {
                        context.Branches.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.KidActivities.Any())
                {
                    var kidActivitiesData = File.ReadAllText("../Infrastructure/Data/SeedData/kidactivities.json");
                    var kidActivities = JsonSerializer.Deserialize<List<KidActivity>>(kidActivitiesData);

                    foreach (var item in kidActivities)
                    {
                        context.KidActivities.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.Manufacturers.Any())
                {
                    var manufacturersData = File.ReadAllText("../Infrastructure/Data/SeedData/manufacturers.json");
                    var manufacturers = JsonSerializer.Deserialize<List<Manufacturer>>(manufacturersData);

                    foreach (var item in manufacturers)
                    {
                        context.Manufacturers.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.OrderStatuses.Any())
                {
                    var orderstatusesData = File.ReadAllText("../Infrastructure/Data/SeedData/orderstatuses.json");
                    var orderstatuses = JsonSerializer.Deserialize<List<OrderStatus>>(orderstatusesData);

                    foreach (var item in orderstatuses)
                    {
                        context.OrderStatuses.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.PaymentOptions.Any())
                {
                    var paymentoptionsData = File.ReadAllText("../Infrastructure/Data/SeedData/paymentoptions.json");
                    var paymentoptions = JsonSerializer.Deserialize<List<PaymentOption>>(paymentoptionsData);

                    foreach (var item in paymentoptions)
                    {
                        context.PaymentOptions.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.ShippingOptions.Any())
                {
                    var shippingoptionsData = File.ReadAllText("../Infrastructure/Data/SeedData/shippingoptions.json");
                    var shippingoptions = JsonSerializer.Deserialize<List<ShippingOption>>(shippingoptionsData);

                    foreach (var item in shippingoptions)
                    {
                        context.ShippingOptions.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.Tags.Any())
                {
                    var tagsData = File.ReadAllText("../Infrastructure/Data/SeedData/tags.json");
                    var tags = JsonSerializer.Deserialize<List<Tag>>(tagsData);

                    foreach (var item in tags)
                    {
                        context.Tags.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.Warehouses.Any())
                {
                    var warehousesData = File.ReadAllText("../Infrastructure/Data/SeedData/warehouses.json");
                    var warehouses = JsonSerializer.Deserialize<List<Warehouse>>(warehousesData);

                    foreach (var item in warehouses)
                    {
                        context.Warehouses.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<HappyKidsContext>();
                logger.LogError(ex.Message);
            }
        }
  
    }
}