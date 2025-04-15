using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
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

        public DbInitializer(StoreDbContext context)
        {
            _context = context;
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
    }
}

