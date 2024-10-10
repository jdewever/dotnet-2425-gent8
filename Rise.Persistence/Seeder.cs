using Rise.Domain.Products;

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

        SeedProducts();
    }

    private bool HasAlreadyBeenSeeded()
    {
        return dbContext.Products.Any();
    }

    private void SeedProducts()
    {
        var products = new List<Product>
        {
            new Product
            {
                Name = "Blood Pressure Monitor",
                Description = "A device for monitoring blood pressure.",
                Barcode = "123456789012",
                QuantityInStock = 15,
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

        dbContext.Products.AddRange(products);
        dbContext.SaveChanges();
    }
}
