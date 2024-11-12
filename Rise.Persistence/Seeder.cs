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
        SeedBookings();
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
        QuantityInStock = 15,
        QuantityOnOrder = 10,
        LowStock = 5,
        ClassRoomCode = "A101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Therapy Ball",
        Description = "A tool for physical therapy and rehabilitation exercises.",
        Barcode = "987654321098",
        QuantityInStock = 30,
        QuantityOnOrder = 5,
        LowStock = 10,
        ClassRoomCode = "B202",
        IsReservable = false,
    },
    new Product
    {
        Name = "Infrared Thermometer",
        Description = "A non-contact thermometer for measuring body temperature.",
        Barcode = "123450987654",
        QuantityInStock = 25,
        QuantityOnOrder = 12,
        LowStock = 5,
        ClassRoomCode = "C303",
        IsReservable = false,
    },
    new Product
    {
        Name = "Hand Sanitizer",
        Description = "An alcohol-based hand sanitizer for personal hygiene.",
        Barcode = "111122223333",
        QuantityInStock = 100,
        QuantityOnOrder = 50,
        LowStock = 100,
        ClassRoomCode = "D404",
        IsReservable = false,
    },
    new Product
    {
        Name = "Electric Wheelchair",
        Description = "A battery-powered wheelchair for mobility support.",
        Barcode = "555566667777",
        QuantityInStock = 8,
        QuantityOnOrder = 2,
        LowStock = 3,
        ClassRoomCode = "E505",
        IsReservable = false,
    },
    new Product
    {
        Name = "Sterile Syringe",
        Description = "Sterile disposable syringe",
        Barcode = "888800000001",
        QuantityInStock = 500,
        QuantityOnOrder = 200,
        ClassRoomCode = "S101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Sterile Needle",
        Description = "Sterile disposable needle",
        Barcode = "888800000002",
        QuantityInStock = 50,
        QuantityOnOrder = 150,
        LowStock = 250,
        ClassRoomCode = "S102",
        IsReservable = false,
    },
    new Product
    {
        Name = "Surgical Mask",
        Description = "Disposable medical surgical mask for protection.",
        Barcode = "888800000003",
        QuantityInStock = 1000,
        QuantityOnOrder = 500,
        LowStock = 250,
        ClassRoomCode = "S103",
        IsReservable = false,
    },
    new Product
    {
        Name = "Elastic Bandage",
        Description = "Elastic bandage for compression and support.",
        Barcode = "888800000004",
        QuantityInStock = 300,
        QuantityOnOrder = 100,
        LowStock = 500,
        ClassRoomCode = "F101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Sterile Gauze Pads",
        Description = "Sterile gauze pads for wound dressing.",
        Barcode = "888800000005",
        QuantityInStock = 400,
        QuantityOnOrder = 150,
        LowStock = 200,
        ClassRoomCode = "F102",
        IsReservable = false,
    },

    new Product
    {
        Name = "Oxygen Mask",
        Description = "Mask used for oxygen therapy.",
        Barcode = "888800000006",
        QuantityInStock = 150,
        QuantityOnOrder = 50,
        LowStock = 50,
        ClassRoomCode = "O101",
        IsReservable = false,
    },
    new Product
    {
        Name = "ECG Machine",
        Description = "Machine for recording the electrical activity of the heart.",
        Barcode = "888800000007",
        QuantityInStock = 10,
        QuantityOnOrder = 5,
        LowStock = 3,
        ClassRoomCode = "M101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Pulse Oximeter",
        Description = "Device to measure oxygen saturation.",
        Barcode = "888800000008",
        QuantityInStock = 200,
        QuantityOnOrder = 100,
        LowStock = 25,
        ClassRoomCode = "M102",
        IsReservable = false,
    },
    new Product
    {
        Name = "IV Drip Stand",
        Description = "Adjustable stand for holding IV drips.",
        Barcode = "888800000009",
        QuantityInStock = 50,
        QuantityOnOrder = 20,
        LowStock = 50,
        ClassRoomCode = "I101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Nebulizer",
        Description = "Device that administers medication in the form of a mist inhaled into the lungs.",
        Barcode = "888800000010",
        QuantityInStock = 40,
        QuantityOnOrder = 15,
        LowStock = 10,
        ClassRoomCode = "N101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Glucometer",
        Description = "Device to measure blood glucose levels.",
        Barcode = "888800000011",
        QuantityInStock = 120,
        QuantityOnOrder = 60,
        LowStock = 30,
        ClassRoomCode = "G101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Stethoscope",
        Description = "Acoustic medical device for auscultation.",
        Barcode = "888800000012",
        QuantityInStock = 80,
        QuantityOnOrder = 30,
        LowStock = 10,
        ClassRoomCode = "S104",
        IsReservable = false,
    },
    new Product
    {
        Name = "Defibrillator",
        Description = "Device that delivers a dose of electrical energy to the heart.",
        Barcode = "888800000013",
        QuantityInStock = 6,
        QuantityOnOrder = 3,
        LowStock = 2,
        ClassRoomCode = "D101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Surgical Gloves",
        Description = "Sterile gloves used in surgery.",
        Barcode = "888800000014",
        QuantityInStock = 500,
        QuantityOnOrder = 250,
        LowStock = 250,
        ClassRoomCode = "S105",
        IsReservable = false,
    },
    new Product
    {
        Name = "Wound Dressing",
        Description = "Sterile dressing for wound protection.",
        Barcode = "888800000015",
        QuantityInStock = 200,
        QuantityOnOrder = 150,
        LowStock = 500,
        ClassRoomCode = "W101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Blood Collection Tubes",
        Description = "Sterile tubes for collecting blood samples.",
        Barcode = "888800000016",
        QuantityInStock = 400,
        QuantityOnOrder = 200,
        LowStock = 500,
        ClassRoomCode = "B101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Suture Kit",
        Description = "Sterile kit for wound closure.",
        Barcode = "888800000017",
        QuantityInStock = 100,
        QuantityOnOrder = 50,
        LowStock = 100,
        ClassRoomCode = "K101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Crutches",
        Description = "Walking aid for mobility support.",
        Barcode = "888800000018",
        QuantityInStock = 30,
        QuantityOnOrder = 10,
        LowStock = 5,
        ClassRoomCode = "C101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Hydrogen Peroxide",
        Description = "Disinfectant used for cleaning wounds.",
        Barcode = "888800000019",
        QuantityInStock = 100,
        QuantityOnOrder = 40,
        LowStock = 25,
        ClassRoomCode = "H101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Splint",
        Description = "Device used to support and immobilize a limb or the spine.",
        Barcode = "888800000020",
        QuantityInStock = 40,
        QuantityOnOrder = 20,
        LowStock = 10,
        ClassRoomCode = "P101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Antiseptic Wipes",
        Description = "Sterile wipes used for cleaning the skin.",
        Barcode = "888800000021",
        QuantityInStock = 700,
        QuantityOnOrder = 350,
        LowStock = 500,
        ClassRoomCode = "A101",
        IsReservable = false,
    },
    new Product
    {
        Name = "Cervical Collar",
        Description = "Device used to support the neck.",
        Barcode = "888800000022",
        QuantityInStock = 15,
        QuantityOnOrder = 5,
        LowStock = 5,
        ClassRoomCode = "C102",
        IsReservable = false,
    },
    new Product
    {
        Name = "IV Cannula",
        Description = "Tube inserted into a vein for intravenous therapy.",
        Barcode = "888800000023",
        QuantityInStock = 500,
        QuantityOnOrder = 200,
        LowStock = 250,
        ClassRoomCode = "I102",
        IsReservable = false,
    },
    new Product
    {
        Name = "Blood Pressure Cuff",
        Description = "Inflatable cuff used to measure blood pressure.",
        Barcode = "888800000024",
        QuantityInStock = 150,
        QuantityOnOrder = 75,
        LowStock = 50,
        ClassRoomCode = "B102",
        IsReservable = false,
    },
    new Product
    {
        Name = "Hot Water Bottle",
        Description = "Bottle used for providing warmth.",
        Barcode = "888800000025",
        QuantityInStock = 80,
        QuantityOnOrder = 30,
        LowStock = 20,
        ClassRoomCode = "H102",
        IsReservable = false,
    },
    new Product
        {
            Name = "Portable Ultrasound Machine",
            Description = "A compact device for ultrasound imaging.",
            Barcode = "888800000030",
            QuantityInStock = 5,
            QuantityOnOrder = 2,
            LowStock = 1,
            ClassRoomCode = "U101",
            IsReservable = true,
        },
        new Product
        {
            Name = "Rehabilitation Treadmill",
            Description = "Treadmill for physical rehabilitation exercises.",
            Barcode = "888800000031",
            QuantityInStock = 3,
            QuantityOnOrder = 1,
            LowStock = 1,
            ClassRoomCode = "R101",
            IsReservable = true,
        },
        new Product
        {
            Name = "Mobile X-Ray Unit",
            Description = "A portable X-ray machine for mobile imaging.",
            Barcode = "888800000032",
            QuantityInStock = 2,
            QuantityOnOrder = 1,
            LowStock = 1,
            ClassRoomCode = "X101",
            IsReservable = true,
        },
        new Product
        {
            Name = "Patient Monitor",
            Description = "Device to monitor vital signs of patients.",
            Barcode = "888800000033",
            QuantityInStock = 6,
            QuantityOnOrder = 2,
            LowStock = 1,
            ClassRoomCode = "P102",
            IsReservable = true,
        },
        new Product
        {
            Name = "Bone Densitometer",
            Description = "Device to measure bone density.",
            Barcode = "888800000034",
            QuantityInStock = 4,
            QuantityOnOrder = 2,
            LowStock = 1,
            ClassRoomCode = "B103",
            IsReservable = true,
        },
        new Product
        {
            Name = "Electric Patient Bed",
            Description = "Adjustable electric bed for patient comfort.",
            Barcode = "888800000035",
            QuantityInStock = 8,
            QuantityOnOrder = 4,
            LowStock = 2,
            ClassRoomCode = "E506",
            IsReservable = true,
        },
        new Product
        {
            Name = "Respirator",
            Description = "Device for artificial respiration.",
            Barcode = "888800000036",
            QuantityInStock = 5,
            QuantityOnOrder = 3,
            LowStock = 2,
            ClassRoomCode = "R102",
            IsReservable = true,
        },
        new Product
        {
            Name = "Portable ECG Device",
            Description = "ECG machine for mobile use.",
            Barcode = "888800000037",
            QuantityInStock = 10,
            QuantityOnOrder = 5,
            LowStock = 2,
            ClassRoomCode = "E102",
            IsReservable = true,
        },
        new Product
        {
            Name = "Dialysis Machine",
            Description = "Machine for blood purification (dialysis).",
            Barcode = "888800000038",
            QuantityInStock = 3,
            QuantityOnOrder = 1,
            LowStock = 1,
            ClassRoomCode = "D102",
            IsReservable = true,
        },
        new Product
        {
            Name = "Infusion Pump",
            Description = "Device for controlled infusion of fluids.",
            Barcode = "888800000039",
            QuantityInStock = 12,
            QuantityOnOrder = 4,
            LowStock = 3,
            ClassRoomCode = "I103",
            IsReservable = true,
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

        // Assign categories to the new products
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
        products[10].Categories = new List<Category> { categories[0], categories[2] };
        products[11].Categories = new List<Category> { categories[3] };
        products[12].Categories = new List<Category> { categories[3], categories[2] };
        products[13].Categories = new List<Category> { categories[0], categories[1] };
        products[14].Categories = new List<Category> { categories[0], categories[2] };
        products[15].Categories = new List<Category> { categories[0] };
        products[16].Categories = new List<Category> { categories[5], categories[0] };
        products[17].Categories = new List<Category> { categories[5], categories[4] };
        products[18].Categories = new List<Category> { categories[5], categories[6] };
        products[19].Categories = new List<Category> { categories[4] };
        products[20].Categories = new List<Category> { categories[6], categories[4] };
        products[21].Categories = new List<Category> { categories[5] };
        products[22].Categories = new List<Category> { categories[1], categories[6] };
        products[23].Categories = new List<Category> { categories[6], categories[0] };
        products[24].Categories = new List<Category> { categories[0], categories[1] };
        products[25].Categories = new List<Category> { categories[0], categories[5] };
        products[26].Categories = new List<Category> { categories[6], categories[2] };
        products[27].Categories = new List<Category> { categories[6] };
        products[28].Categories = new List<Category> { categories[5] };
        products[29].Categories = new List<Category> { categories[1], categories[6] };
        products[30].Categories = new List<Category> { categories[1] };
        products[31].Categories = new List<Category> { categories[2], categories[3] };
        products[32].Categories = new List<Category> { categories[3] };
        products[33].Categories = new List<Category> { categories[4] };
        products[34].Categories = new List<Category> { categories[1], categories[4] };
        products[35].Categories = new List<Category> { categories[2] };
        products[36].Categories = new List<Category> { categories[3] };
        products[37].Categories = new List<Category> { categories[4] };
        products[38].Categories = new List<Category> { categories[1] };
        products[39].Categories = new List<Category> { categories[2] };

        categories[0].Products = [products[4]];
        categories[1].Products = [products[1]];
        categories[3].Products = [products[2], products[0]];
        categories[4].Products = [products[3], products[7]];
        categories[5].Products = [products[5], products[6]];
        categories[6].Products = [products[8], products[9]];

        dbContext.SaveChanges();
    }

    private void SeedBookings()
    {
        var firstReservableProduct = dbContext.Products.FirstOrDefault(p => p.IsReservable);

        if (firstReservableProduct != null)
        {
            var bookings = new List<Booking>
            {
                new Booking(firstReservableProduct.Id, "user1", DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-5)),
                new Booking(firstReservableProduct.Id, "user2", DateTime.UtcNow.AddDays(-8), DateTime.UtcNow.AddDays(-3)),
                new Booking(firstReservableProduct.Id, "user3", DateTime.UtcNow.AddDays(-6), DateTime.UtcNow.AddDays(-1))
            };

            dbContext.Booking.AddRange(bookings);
            dbContext.SaveChanges();
        }
    }

}
