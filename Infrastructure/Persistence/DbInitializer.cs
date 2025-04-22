using Domain.Contracts;
using Domain.Identity;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;
        private readonly StoreIdentityDbContext _identityDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(StoreDbContext context,
            StoreIdentityDbContext identityDbContext
            ,UserManager<AppUser> userManager
            ,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _identityDbContext = identityDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {

            try
            {
                // Create DateBase If it dosen't Exists && Apply To Any Pending Migrations
                if (_context.Database.GetPendingMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }

                // Data Seeding 

                // Seeding productType From Json File

                if (!_context.productTypes.Any())
                {
                    // 1. Read All Data From Types Json File as String
                    var typesDate = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");

                    // 2. Transform string To C# Object [List<ProductTypes>]

                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesDate);

                    // 3. Add List<ProductTypes> To DataBase

                    if (types is not null && types.Any())
                    {
                        await _context.productTypes.AddRangeAsync(types);
                        await _context.SaveChangesAsync();
                    }
                }

                // Seeding productBrands From Json File

                if (!_context.productBrands.Any())
                {
                    // 1. Read All Data From brands Json File as String
                    var brandsDate = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");

                    // 2. Transform string To C# Object [List<ProductTypes>]

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsDate);

                    // 3. Add List<ProductBrand> To DataBase

                    if (brands is not null && brands.Any())
                    {
                        await _context.productBrands.AddRangeAsync(brands);
                        await _context.SaveChangesAsync();
                    }
                }



                // Seeding products From Json File



                if (!_context.products.Any())
                {
                    // 1. Read All Data From products Json File as String
                    var productDate = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");

                    // 2. Transform string To C# Object [List<ProductTypes>]

                    var products = JsonSerializer.Deserialize<List<Product>>(productDate);

                    // 3. Add List<Product> To DataBase

                    if (products is not null && products.Any())
                    {
                        await _context.products.AddRangeAsync(products);
                        await _context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task InitializeIdentityAsync()
        {
            // Create DateBase If it dosen't Exists && Apply To Any Pending Migrations
            if (_identityDbContext.Database.GetPendingMigrations().Any())
            {
               await _identityDbContext.Database.MigrateAsync();
            }

            if(!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Admin"
                });
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "SuperAdmin"
                });
            }

            // Seeding 
            if (!_userManager.Users.Any())
            {
                var superAdminUser = new AppUser()
                {
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "01124033585",
                };
                var adminUser = new AppUser()
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "01124033585",
                };

               await _userManager.CreateAsync(superAdminUser, "P@ssW0rd");
               await _userManager.CreateAsync(adminUser, "P@ssW0rd");


               await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
               await _userManager.AddToRoleAsync(adminUser, "Admin");
            }

        }


    }
}

