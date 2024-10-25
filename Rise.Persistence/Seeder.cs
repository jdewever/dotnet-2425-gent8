using Rise.Domain.DomainClasses;

namespace Rise.Persistence;

public class Seeder
{
    private readonly ApplicationDbContext dbContext;

    public Seeder(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Seed()
    {
        if (HasAlreadyBeenSeeded())
            return;

        SeedProductsAndCategories();
    }

    private bool HasAlreadyBeenSeeded()
    {
        return dbContext.Products.Any() || dbContext.Categories.Any();
    }

    private void SeedProductsAndCategories()
    {
        var products = new List<Product>
        {
            new Product
            {
                Name = "Blood Pressure Monitor",
                Description = "A device for monitoring blood pressure.",
                Barcode = "123456789012",
                QuantityInStock = 0,
                QuantityOnOrder = 10,
                ClassRoomCode = "A101",
            },
            new Product
            {
                Name = "Therapy Ball",
                Description = "A tool for physical therapy and rehabilitation exercises.",
                Barcode = "987654321098",
                QuantityInStock = 30,
                QuantityOnOrder = 5,
                ClassRoomCode = "B202",
            },
            new Product
            {
                Name = "Infrared Thermometer",
                Description = "A non-contact thermometer for measuring body temperature.",
                Barcode = "123450987654",
                QuantityInStock = 25,
                QuantityOnOrder = 12,
                ClassRoomCode = "C303",
            },
            new Product
            {
                Name = "Hand Sanitizer",
                Description = "An alcohol-based hand sanitizer for personal hygiene.",
                Barcode = "111122223333",
                QuantityInStock = 100,
                QuantityOnOrder = 50,
                ClassRoomCode = "D404",
            },
            new Product
            {
                Name = "Electric Wheelchair",
                Description = "A battery-powered wheelchair for mobility support.",
                Barcode = "555566667777",
                QuantityInStock = 8,
                QuantityOnOrder = 2,
                ClassRoomCode = "E505",
            },
            new Product
            {
                Name = "Sterile Syringe",
                Description = "Sterile disposable syringe",
                Barcode = "888800000001",
                QuantityInStock = 500,
                QuantityOnOrder = 200,
                ClassRoomCode = "S101",
            },
            new Product
            {
                Name = "Sterile Needle",
                Description = "Sterile disposable needle",
                Barcode = "888800000002",
                QuantityInStock = 600,
                QuantityOnOrder = 150,
                ClassRoomCode = "S102",
            },
            new Product
            {
                Name = "Surgical Mask",
                Description = "Disposable medical surgical mask for protection.",
                Barcode = "888800000003",
                QuantityInStock = 1000,
                QuantityOnOrder = 500,
                ClassRoomCode = "S103",
            },
            new Product
            {
                Name = "Elastic Bandage",
                Description = "Elastic bandage for compression and support.",
                Barcode = "888800000004",
                QuantityInStock = 300,
                QuantityOnOrder = 100,
                ClassRoomCode = "F101",
            },
            new Product
            {
                Name = "Sterile Gauze Pads",
                Description = "Sterile gauze pads for wound dressing.",
                Barcode = "888800000005",
                QuantityInStock = 400,
                QuantityOnOrder = 150,
                ClassRoomCode = "F102",
            }
        };
        var categories = new List<Category>
        {
            new Category { Name = "Medical Devices" },
            new Category { Name = "Therapy Tools" },
            new Category { Name = "Wellness Supplies" },
            new Category { Name = "Monitoring Equipment" },
            new Category { Name = "Sanitary Products" },
            new Category { Name = "Surgical Supplies" },
            new Category { Name = "First Aid" } 
        };
        dbContext.Categories.AddRange(categories);
        dbContext.Products.AddRange(products);
        
        //relation between product and categories
        products[0].Categories = [categories[3], categories[2]];
        products[1].Categories = [categories[1], categories[2], categories[2]];
        products[2].Categories = [categories[3]];
        products[3].Categories = [categories[4]];
        products[4].Categories = [categories[0]];
        products[5].Categories = [categories[5]];
        products[6].Categories = [categories[5]];
        products[7].Categories = [categories[4]];
        products[8].Categories = [categories[6]];
        products[9].Categories = [categories[6]];
        
        //relation between category and products
        categories[0].Products = [products[4]];
        categories[1].Products = [products[1]];
        categories[3].Products = [products[2], products[0]];
        categories[4].Products = [products[3], products[7]];
        categories[5].Products = [products[5], products[6]];
        categories[6].Products = [products[8], products[9]];
        
        dbContext.Categories.AddRange(categories);
        dbContext.Products.AddRange(products);
        dbContext.SaveChanges();
    }
}
