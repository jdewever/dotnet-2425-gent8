using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Rise.Domain.DomainClasses;

namespace Rise.Persistence;

public class Seeder
{
    private readonly ApplicationDbContext dbContext;
    private readonly IMinioClient minio;
    private readonly string minioBucket;
    private readonly string minioUrl;
    private readonly bool isDevelopment;

    public Seeder(ApplicationDbContext dbContext, IMinioClient minio, string minioUrl, string minioBucket, bool isDevelopment)
    {
        this.dbContext = dbContext;
        this.minio = minio;
        this.minioBucket = minioBucket;
        this.minioUrl = minioUrl;
        this.isDevelopment = isDevelopment;
    }

    public async Task Seed()
    {
        if (HasAlreadyBeenSeeded())
            return;

        await DeleteAllImagesAsync();
        await UploadImageAsync("default.png", "../Rise.Persistence/Seeding/default.png");

        await SeedProductsAndCategories();
        SeedBookings();
    }

    private bool HasAlreadyBeenSeeded()
    {
        return dbContext.Products.Any() || dbContext.Categories.Any();
    }

    private async Task SeedProductsAndCategories()
    {
        var products = new List<Product>
    {
    new Product
    {
        Name = "Bloeddrukmeter",
        Description = "Een apparaat om de bloeddruk van een patiënt te meten. Laat toe de bleoddruk van een patient te meten zonder in de bloedvaten te prikken. Ook wel sphygmomanometer, tensiometer of polsdrukmeter genoemd.",
        Barcode = "2128621336990",
        QuantityInStock = 20,
        QuantityOnOrder = 5,
        LowStock = 5,
        ClassRoomCode = "B.4012",
        IsReservable = true,
        ImageUrl = await UploadImageAsync("bloeddrukmeter.png", "../Rise.Persistence/Seeding/bloeddrukmeter.png"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Injectienaald",
        Description = "Een injectienaald is een medisch instrument dat wordt gebruikt voor het toedienen van vloeistoffen zoals medicijnen of vaccins direct in het lichaam, meestal via een intramusculaire of subcutane injectie. Het bestaat uit een holle naald en een spuit waarmee de vloeistof kan worden toegediend.",
        Barcode = "7786005654037",
        QuantityInStock = 1000,
        QuantityOnOrder = 200,
        LowStock = 500,
        ClassRoomCode = "B.4012",
        IsReservable = false,
        ImageUrl = await UploadImageAsync("injectienaald.png", "../Rise.Persistence/Seeding/injectienaald.png"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Klassieke Thermometer",
        Description = "Apparaat dat wordt gebruikt om de lichaamstemperatuur te meten. Het wordt vaak gebruikt om koorts te detecteren.",
        Barcode = "2793159743291",
        QuantityInStock = 20,
        QuantityOnOrder = 0,
        LowStock = 5,
        ClassRoomCode = "B.4012",
        IsReservable = true,
        ImageUrl = await UploadImageAsync("thermometer.png", "../Rise.Persistence/Seeding/thermometer.png"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Infrarood Thermometer",
        Description = "Een infrarood thermometer is een thermometer is een contactloze thermometer die de temperatuur van een oppervlak meet zonder het oppervlak aan te raken.",
        Barcode = "8977149310036",
        QuantityInStock = 25,
        QuantityOnOrder = 12,
        LowStock = 5,
        ClassRoomCode = "B.4012",
        IsReservable = true,
        ImageUrl = await UploadImageAsync("infrarood-thermometer.jpg", "../Rise.Persistence/Seeding/infrarood-thermometer.jpg"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Stethoscoop",
        Description = "Een medisch instrument dat wordt gebruikt om naar interne geluiden in het lichaam te luisteren, zoals hart- en longgeluiden. Het bestaat uit een borststuk dat op de huid wordt geplaatst, verbonden met oordopjes door middel van flexibele slangen. Het is een cruciaal hulpmiddel voor artsen bij het stellen van diagnoses.",
        Barcode = "6950263680928",
        QuantityInStock = 20,
        QuantityOnOrder = 9,
        LowStock = 30,
        ClassRoomCode = "B.2012",
        IsReservable = true,
        ImageUrl = await UploadImageAsync("stethoscoop.png", "../Rise.Persistence/Seeding/stethoscoop.png"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Elektrische rolstoel",
        Description = "Een rolstoel die wordt aangedreven, gemakkelijk voor gebruik door de patient zelf. Bedoeld voor mensen die niet in staat zijn om een handmatige rolstoel te duwen of die extra ondersteuning nodig hebben bij het verplaatsen.",
        Barcode = "1344607852440",
        QuantityInStock = 8,
        QuantityOnOrder = 2,
        LowStock = 5,
        ClassRoomCode = "A.401",
        IsReservable = true,
        ImageUrl = await UploadImageAsync("electric-ws.png", "../Rise.Persistence/Seeding/electric-ws.png"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Rolstoel",
        Description = "Een rolstoel is een stoel op wielen die wordt gebruikt door mensen die moeite hebben met lopen of helemaal niet kunnen lopen. Het biedt mobiliteit en onafhankelijkheid voor mensen met een handicap of letsel. Moderne rolstoelen kunnen handmatig worden aangedreven of elektronisch worden bediend.",
        Barcode = "7919454827619",
        QuantityInStock = 10,
        QuantityOnOrder = 0,
        LowStock = 25,
        ClassRoomCode = "S.101",
        IsReservable = true,
        ImageUrl = await UploadImageAsync("rolstoel.png", "../Rise.Persistence/Seeding/rolstoel.png"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Krukken",
        Description = "Worden gebruikt door mensen die een blessure hebben of die moeite hebben met lopen. Ze helpen het lichaamsgewicht te verdelen en ondersteunen de mobiliteit tijdens het lopen. Krukken kunnen worden aangepast aan de lengte van de gebruiker en zijn meestal gemaakt van lichtgewicht materialen zoals aluminium.",
        Barcode = "4994804485384",
        QuantityInStock = 40,
        QuantityOnOrder = 20,
        LowStock = 50,
        ClassRoomCode = "S.101",
        IsReservable = true,
        ImageUrl = await UploadImageAsync("krukken.png", "../Rise.Persistence/Seeding/krukken.png"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Bloedglucosemeter",
        Description = "Een bloedglucosemeter is een draagbaar apparaat dat wordt gebruikt door mensen met diabetes om hun bloedsuikerspiegel te controleren. Het apparaat werkt door een kleine druppel bloed op een teststrip te analyseren, wat essentieel is voor het beheren van diabetes en het aanpassen van de medicatie of voeding.",
        Barcode = "2778717870893",
        QuantityInStock = 20,
        QuantityOnOrder = 0,
        LowStock = 5,
        ClassRoomCode = "B.4012",
        IsReservable = true,
        ImageUrl = await UploadImageAsync("bgm.png", "../Rise.Persistence/Seeding/bgm.png"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Elastisch Verband",
        Description = "Een elastisch verband is een flexibel verband dat wordt gebruikt om wonden te bedekken en te beschermen. Het is gemaakt van zacht, rekbaar materiaal dat comfortabel is om te dragen en de beweging niet beperkt. Het kan worden gebruikt voor het fixeren van verbanden, het ondersteunen van gewrichten of het verlichten van zwelling.",
        Barcode = "1501097441756",
        QuantityInStock = 300,
        QuantityOnOrder = 100,
        LowStock = 500,
        ClassRoomCode = "B.4011",
        IsReservable = false,
        ImageUrl = await UploadImageAsync("el-verb.png", "../Rise.Persistence/Seeding/el-verb.png"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Steriele Gaaskompressen",
        Description = "Steriele gaaskompressen zijn kleine, steriele doekjes die worden gebruikt voor het bedekken en beschermen van wonden. Ze helpen bij het absorberen van bloed en andere lichaamsvloeistoffen, en voorkomen dat vuil en bacteriën de wond binnendringen. Steriele gaaskompressen zijn een essentieel onderdeel van elke EHBO-kit en worden vaak gebruikt in combinatie met verbanden.",
        Barcode = "3793699087502",
        QuantityInStock = 400,
        QuantityOnOrder = 150,
        LowStock = 200,
        ClassRoomCode = "B.4011",
        IsReservable = false,
        ImageUrl = await UploadImageAsync("gaas.png", "../Rise.Persistence/Seeding/gaas.png"),
        IsHidden = false,
    },

    new Product
    {
        Name = "Zuurstofmasker",
        Description = "Een zuurstofmasker is een medisch hulpmiddel dat wordt gebruikt om extra zuurstof toe te dienen aan patiënten die moeite hebben met ademhalen of die extra zuurstof nodig hebben. Het masker bedekt de neus en mond van de patiënt en wordt aangesloten op een zuurstofbron om geconcentreerde zuurstof te leveren.",
        Barcode = "9132449329907",
        QuantityInStock = 150,
        QuantityOnOrder = 50,
        LowStock = 100,
        ClassRoomCode = "B.4010",
        IsReservable = false,
        ImageUrl = await UploadImageAsync("zuurstofmasker.jpg", "../Rise.Persistence/Seeding/zuurstofmasker.jpg"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Elektrocardiogram (ECG) Machine",
        Description = "Een elektrocardiogram (ECG, EKG) machine is een medisch apparaat dat wordt gebruikt om de elektrische activiteit van het hart te meten en op te nemen. Het apparaat registreert de hartslag en de hartslagvariatie, en kan helpen bij het diagnosticeren van hartaandoeningen zoals aritmieën, hartaanvallen en andere hartaandoeningen.",
        Barcode = "7252594591591",
        QuantityInStock = 10,
        QuantityOnOrder = 5,
        LowStock = 3,
        ClassRoomCode = "C.2010",
        IsReservable = true,
        ImageUrl = await UploadImageAsync("ecg-machine.png", "../Rise.Persistence/Seeding/ecg-machine.png"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Inhalator",
        Description = "Een inhalator is een medisch apparaat dat wordt gebruikt om medicijnen rechtstreeks in de longen te brengen via inademing. Het wordt vaak voorgeschreven aan mensen met astma of chronische obstructieve longziekte (COPD) om ademhalingsproblemen te verlichten door luchtwegen te openen en ontstekingen te verminderen.",
        Barcode = "3737312589301",
        QuantityInStock = 200,
        QuantityOnOrder = 100,
        LowStock = 25,
        ClassRoomCode = "B.4012",
        IsReservable = true,
        ImageUrl = await UploadImageAsync("inhalator.jpg", "../Rise.Persistence/Seeding/inhalator.jpg"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Zuurstoftank",
        Description = "Een zuurstoftank is een cilinder gevuld met samengeperste zuurstof die wordt gebruikt om mensen te helpen die problemen hebben met ademhalen of die extra zuurstof nodig hebben vanwege ademhalingsaandoeningen. De zuurstof wordt via een masker of neuscanule aan de patiënt toegediend.",
        Barcode = "3797151253207",
        QuantityInStock = 30,
        QuantityOnOrder = 10,
        LowStock = 50,
        ClassRoomCode = "Z.101",
        IsReservable = false,
        ImageUrl = await UploadImageAsync("zuurstoftank.jpg", "../Rise.Persistence/Seeding/zuurstoftank.jpg"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Verband",
        Description = "Verband wordt gebruikt voor het bedekken en beschermen van wonden, het ondersteunen van gewrichten, en het immobiliseren van ledematen na een blessure. Het kan steriel of niet-steriel zijn, afhankelijk van het gebruik, en is een essentieel onderdeel van elke EHBO-kit.",
        Barcode = "1234142880520",
        QuantityInStock = 100,
        QuantityOnOrder = 1000,
        LowStock = 500,
        ClassRoomCode = "B.4011",
        IsReservable = false,
        ImageUrl = await UploadImageAsync("verband.jpg", "../Rise.Persistence/Seeding/verband.jpg"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Kleine pleister",
        Description = "Pleisters voor kleine wonden en schaafwonden. Ze zijn gemaakt van een zacht, flexibel materiaal dat de wond beschermt en het genezingsproces bevordert. Pleisters zijn een essentieel onderdeel van elke EHBO-kit en kunnen worden gebruikt voor snijwonden, schaafwonden, blaren en andere kleine verwondingen.",
        Barcode = "8844616580431",
        QuantityInStock = 120,
        QuantityOnOrder = 600,
        LowStock = 1000,
        ClassRoomCode = "B.4011",
        IsReservable = false,
        ImageUrl = await UploadImageAsync("pleister.jpg", "../Rise.Persistence/Seeding/pleister.jpg"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Eilandpleister",
        Description = "Grote pleisters voor het bedekken van grotere wonden en snijwonden. Ze zijn gemaakt van een zacht, absorberend materiaal dat de wond beschermt en het genezingsproces bevordert. Eilandpleisters zijn een essentieel onderdeel van elke EHBO-kit en kunnen worden gebruikt voor snijwonden, schaafwonden, brandwonden en andere grote verwondingen.",
        Barcode = "6807449252199",
        QuantityInStock = 80,
        QuantityOnOrder = 300,
        LowStock = 100,
        ClassRoomCode = "B.4011",
        IsReservable = false,
        ImageUrl = await UploadImageAsync("eilandpleister.jpg", "../Rise.Persistence/Seeding/eilandpleister.jpg"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Defibrillator",
        Description = "Een defibrillator is een medisch apparaat dat wordt gebruikt om een plotselinge hartstilstand te behandelen door een elektrische schok toe te dienen aan het hart. Het apparaat kan levens redden door het herstellen van een normaal hartritme en het voorkomen van de dood",
        Barcode = "5467200503178",
        QuantityInStock = 6,
        QuantityOnOrder = 3,
        LowStock = 5,
        ClassRoomCode = "D.101",
        IsReservable = true,
        ImageUrl = await UploadImageAsync("defib.png", "../Rise.Persistence/Seeding/defib.png"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Medische handschoenen",
        Description = "Medische handschoenen voor gebruik in de gezondheidszorg. Ze zijn gemaakt van latex, nitril of vinyl en bieden bescherming tegen infecties en besmettingen. Medische handschoenen zijn een essentieel onderdeel van elke medische kit en worden gebruikt bij het uitvoeren van procedures, het behandelen van patiënten en het voorkomen van kruisbesmetting.",
        Barcode = "5865626226662",
        QuantityInStock = 500,
        QuantityOnOrder = 250,
        LowStock = 1000,
        ClassRoomCode = "B.4012",
        IsReservable = false,
        ImageUrl = await UploadImageAsync("hs.png", "../Rise.Persistence/Seeding/hs.png"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Chirurgische schaar",
        Description = "Een chirurgische schaar is een medisch instrument dat wordt gebruikt om weefsel te knippen en te verwijderen tijdens chirurgische ingrepen. Het bestaat uit twee scherpe bladen die aan een handvat zijn bevestigd en kan worden gebruikt voor het knippen van verbanden, hechtingen, huid en andere weefsels.",
        Barcode = "2448086833385",
        QuantityInStock = 200,
        QuantityOnOrder = 0,
        LowStock = 100,
        ClassRoomCode = "B.4011",
        IsReservable = false,
        ImageUrl = await UploadImageAsync("schaar.png", "../Rise.Persistence/Seeding/schaar.png"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Bloedafnamebuisjes",
        Description = "Afnamebuisjes voor het verzamelen van bloedmonsters voor laboratoriumtests. Ze zijn gemaakt van plastic of glas en bevatten anticoagulantia om het bloed te bewaren en te voorkomen dat het stolt. Bloedafnamebuisjes zijn een essentieel onderdeel van elke bloedafnamekit en worden gebruikt voor diagnostische tests en medische onderzoeken.",
        Barcode = "5329093380307",
        QuantityInStock = 400,
        QuantityOnOrder = 200,
        LowStock = 500,
        ClassRoomCode = "B.4011",
        IsReservable = false,
        IsHidden = false,
    },
    new Product
    {
        Name = "Hechtdraad",
        Description = "Hechtdraad voor het sluiten van wonden en chirurgische incisies. Het is gemaakt van steriel materiaal zoals zijde, nylon of polypropyleen en wordt gebruikt om de huid en weefsels te hechten na een operatie of letsel. Hechtdraad is een essentieel onderdeel van elke EHBO-kit en wordt gebruikt door artsen en verpleegkundigen om wonden te sluiten en te genezen.",
        Barcode = "1511125934709",
        QuantityInStock = 100,
        QuantityOnOrder = 50,
        LowStock = 200,
        ClassRoomCode = "B.4011",
        IsReservable = false,
        IsHidden = false,
    },
    new Product
    {
        Name = "Nietjespistool",
        Description = "Een nietjespistool is een medisch instrument dat wordt gebruikt om wonden te sluiten en te hechten met metalen nietjes. Het apparaat is snel en gemakkelijk te gebruiken en biedt een alternatief voor traditionele hechtingen. Het wordt vaak gebruikt voor het sluiten van chirurgische incisies en traumatische wonden.",
        Barcode = "1511125934709",
        QuantityInStock = 3,
        QuantityOnOrder = 0,
        LowStock = 1,
        ClassRoomCode = "B.4012",
        IsReservable = false,
        IsHidden = true,
    },
    new Product
    {
        Name = "Warmwaterkruik",
        Description = "Een warmwaterkruik is een medisch hulpmiddel dat wordt gebruikt om warmte te leveren aan het lichaam voor pijnverlichting en comfort. Het bestaat uit een rubberen zak die wordt gevuld met warm water en op de huid wordt geplaatst om spierpijn, krampen en andere aandoeningen te verlichten.",
        Barcode = "8549797229116",
        QuantityInStock = 80,
        QuantityOnOrder = 30,
        LowStock = 20,
        ClassRoomCode = "B.4012",
        IsReservable = false,
        ImageUrl = await UploadImageAsync("warmwaterkruik.jpg", "../Rise.Persistence/Seeding/warmwaterkruik.jpg"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Gehoorapparaat",
        Description = "Een gehoorapparaat is een medisch hulpmiddel dat wordt gebruikt om gehoorverlies te compenseren en geluiden te versterken voor mensen met gehoorproblemen. Het apparaat bestaat uit een microfoon, versterker en luidspreker die samenwerken om geluiden op te vangen, te versterken en door te geven aan het oor.",
        Barcode = "7259742316020",
        QuantityInStock = 10,
        QuantityOnOrder = 100,
        LowStock = 100,
        ClassRoomCode = "C.2001",
        IsReservable = false,
        IsHidden = false,
    },
    new Product
    {
        Name = "Mobiele röntgenmachine",
        Description = "Een mobiele röntgenmachine is een draagbaar medisch apparaat dat wordt gebruikt om röntgenfoto's te maken van patiënten op locatie. Het apparaat is compact en gemakkelijk te verplaatsen, waardoor het ideaal is voor noodsituaties en medische missies.",
        Barcode = "7566065164000",
        QuantityInStock = 2,
        QuantityOnOrder = 1,
        LowStock = 1,
        ClassRoomCode = "D.101",
        IsReservable = true,
        ImageUrl = await UploadImageAsync("prgm.jpg", "../Rise.Persistence/Seeding/prgm.jpg"),
        IsHidden = false,
    },
    new Product
    {
        Name = "Bloeddrukmeter voor pols",
        Description = "Een polsbloeddrukmeter is een draagbaar apparaat dat wordt gebruikt om de bloeddruk van een patiënt te meten via de pols. Het apparaat is compact en gemakkelijk te gebruiken, waardoor het ideaal is voor thuisgebruik en onderweg.",
        Barcode = "5034056696899",
        QuantityInStock = 20,
        QuantityOnOrder = 50,
        LowStock = 20,
        ClassRoomCode = "B.4012",
        IsReservable = true,
        IsHidden = false,
    },
    new Product
    {
        Name = "Elektrisch ziekenhuisbed",
        Description = "Een elektrisch ziekenhuisbed is een medisch hulpmiddel dat wordt gebruikt om patiënten comfortabel te positioneren en te verplaatsen in een ziekenhuisomgeving. Het bed kan worden aangepast aan de behoeften van de patiënt en biedt extra ondersteuning en veiligheid tijdens het herstelproces.",
        Barcode = "1463097558004",
        QuantityInStock = 8,
        QuantityOnOrder = 4,
        LowStock = 2,
        ClassRoomCode = "A.401",
        IsReservable = true,
        IsHidden = false,
    },
};

        var categories = new List<Category>
    {
        new Category { Name = "Diagnostische apparatuur" },
        new Category { Name = "Injectie en toediening" },
        new Category { Name = "Wondverzorging" },
        new Category { Name = "Hygiëne" },
        new Category { Name = "Mobiliteitshulpmiddelen" },
        new Category { Name = "Chirurgische instrumenten" },
        new Category { Name = "Fysiotherapieapparatuur" },
        new Category { Name = "Ademhalingshulpmiddelen"},
        new Category { Name = "Medicatie en medicijnkast" },
        new Category { Name = "EHBO-kits" },
    };

        dbContext.Categories.AddRange(categories);
        dbContext.Products.AddRange(products);

        // Assign categories to the new products
        products[0].Categories = [categories[0]];
        products[1].Categories = [categories[1]];
        products[2].Categories = [categories[0]];
        products[3].Categories = [categories[0]];
        products[4].Categories = [categories[0]];
        products[5].Categories = [categories[4]];
        products[6].Categories = [categories[4]];
        products[7].Categories = [categories[4]];
        products[8].Categories = [categories[2]];
        products[9].Categories = [categories[2]];
        products[10].Categories = [categories[7], categories[2]];
        products[11].Categories = [categories[0]];
        products[12].Categories = [categories[0]];
        products[13].Categories = [categories[0]];
        products[14].Categories = [categories[0]];
        products[15].Categories = [categories[2]];
        products[16].Categories = [categories[2]];
        products[17].Categories = [categories[2]];
        products[18].Categories = [categories[4]];
        products[19].Categories = [categories[2]];
        products[20].Categories = [categories[4]];
        products[21].Categories = [categories[4]];
        products[22].Categories = [categories[4]];
        products[23].Categories = [categories[4]];
        products[24].Categories = [categories[4]];
        products[25].Categories = [categories[4], categories[5]];
        products[26].Categories = [categories[0], categories[4]];
        products[27].Categories = [categories[0]];
        products[28].Categories = [categories[4]];

        dbContext.SaveChanges();
    }

    private void SeedBookings()
    {
        var firstReservableProduct = dbContext.Products.FirstOrDefault(p => p.IsReservable);

        if (firstReservableProduct?.Id != null)
        {
            var bookings = new List<Booking>
            {
                new Booking(firstReservableProduct, "auth0|6708f85072e161294340f1fd", new DateTime(DateTime.UtcNow.Year, 11, 20, 8, 0, 0, DateTimeKind.Utc), new DateTime(DateTime.UtcNow.Year, 11, 21, 16, 0, 0, DateTimeKind.Utc)),
                new Booking(firstReservableProduct, "auth0|6708f85072e161294340f1fd", new DateTime(DateTime.UtcNow.Year, 11, 3, 8, 0, 0, DateTimeKind.Utc), new DateTime(DateTime.UtcNow.Year, 11, 6, 16, 0, 0, DateTimeKind.Utc)),
                new Booking(firstReservableProduct, "auth0|670d2d182ecfb6f5bdcca195", new DateTime(DateTime.UtcNow.Year, 11, 25, 8, 0, 0, DateTimeKind.Utc), new DateTime(DateTime.UtcNow.Year, 11, 25, 10, 0, 0, DateTimeKind.Utc)),
                new Booking(firstReservableProduct, "auth0|670d2d182ecfb6f5bdcca195", new DateTime(DateTime.UtcNow.Year, 10, 18, 8, 0, 0, DateTimeKind.Utc), new DateTime(DateTime.UtcNow.Year, 10, 19, 10, 0, 0, DateTimeKind.Utc)),
                new Booking(firstReservableProduct, "auth0|670d2d182ecfb6f5bdcca195", new DateTime(DateTime.UtcNow.Year, 9, 21, 8, 0, 0, DateTimeKind.Utc), new DateTime(DateTime.UtcNow.Year, 9, 21, 10, 0, 0, DateTimeKind.Utc)),
                new Booking(firstReservableProduct, "auth0|670d2d182ecfb6f5bdcca195", new DateTime(DateTime.UtcNow.Year, 11, 20, 8, 0, 0, DateTimeKind.Utc), new DateTime(DateTime.UtcNow.Year, 11, 21, 16, 0, 0, DateTimeKind.Utc)),
                new Booking(firstReservableProduct, "auth0|670d2d182ecfb6f5bdcca195", new DateTime(DateTime.UtcNow.Year, 11, 3, 8, 0, 0, DateTimeKind.Utc), new DateTime(DateTime.UtcNow.Year, 11, 6, 16, 0, 0, DateTimeKind.Utc)),
            };

            dbContext.Booking.AddRange(bookings);
            dbContext.SaveChanges();
        }
    }


    // Minio helpers
    public async Task DeleteAllImagesAsync()
    {
        if (!isDevelopment)
            return;

        try
        {
            IAsyncEnumerable<Item> observable = minio.ListObjectsEnumAsync(new ListObjectsArgs().WithBucket(minioBucket));
            await foreach (Item item in observable)
            {
                await minio.RemoveObjectAsync(new RemoveObjectArgs().WithBucket(minioBucket).WithObject(item.Key));
            }
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while deleting the images", e);
        }
    }

    // upload local image to minio from ./Seeding/
    public async Task<string> UploadImageAsync(string objectName, string filePath)
    {
        if (!isDevelopment)
            return $"{minioUrl}/{minioBucket}/{objectName}";

        try
        {
            await minio.PutObjectAsync(new PutObjectArgs()
                .WithBucket(minioBucket)
                .WithObject(objectName)
                .WithFileName(filePath));
            return $"{minioUrl}/{minioBucket}/{objectName}";
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while uploading the image", e);
        }
    }
}
