using ProductionGrade.Data;
using ProductionGrade.Models;

namespace ProductionGrade.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Seed Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Electronics" },
                    new Category { Name = "Books" },
                    new Category { Name = "Clothing" }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            // Seed Products
            if (!context.Products.Any())
            {
                var electronicsCategory = context.Categories.First(c => c.Name == "Electronics");
                var booksCategory = context.Categories.First(c => c.Name == "Books");
                var clothingCategory = context.Categories.First(c => c.Name == "Clothing");

                var products = new List<Product>
                {
                    new Product
                    {
                        SKU = "LAPTOP-001",
                        Name = "Gaming Laptop",
                        Description = "High performance laptop",
                        Price = 1500,
                        StockQuantity = 10,
                        CategoryId = electronicsCategory.Id
                    },
                    new Product
                    {
                        SKU = "BOOK-001",
                        Name = "C# Programming Guide",
                        Description = "Learn C# with examples",
                        Price = 50,
                        StockQuantity = 100,
                        CategoryId = booksCategory.Id
                    },
                    new Product
                    {
                        SKU = "CLOTH-001",
                        Name = "T-Shirt",
                        Description = "Comfortable cotton T-shirt",
                        Price = 20,
                        StockQuantity = 200,
                        CategoryId = clothingCategory.Id
                    }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
