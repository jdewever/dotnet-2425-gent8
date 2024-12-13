using Rise.Shared.Barcodes;
using BarcodeStandard;
using SkiaSharp;
using System.Text;
using Rise.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Rise.Services.Barcodes;

public class BarcodeService : IBarcodeService
{
    private readonly ApplicationDbContext dbContext;

    public BarcodeService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<BarcodeResponse> GetNewBarcode()
    {
        var random = new Random();
        string? barcode = null;
        long sum;
        long checksum;

        while (barcode == null || await dbContext.Products.AnyAsync(p => p.Barcode == barcode))
        {
            barcode = random.NextInt64(100000000000, 999999999999).ToString();

            sum = 0;
            for (int i = 0; i < 12; i++)
            {
                sum += (i % 2 == 0) ? long.Parse(barcode[i].ToString()) : long.Parse(barcode[i].ToString()) * 3;
            }
            checksum = (10 - sum % 10) % 10;

            barcode = (long.Parse(barcode) * 10 + checksum).ToString();
        }
        return new BarcodeResponse { Barcode = barcode };
    }

    public bool IsValidBarcode(string barcode)
    {
        if (barcode.Length != 13 || !barcode.All(char.IsDigit))
        {
            return false;
        }

        long sum = 0;
        long checksum = long.Parse(barcode[12].ToString());
        for (int i = 0; i < 12; i++)
        {
            sum += (i % 2 == 0) ? long.Parse(barcode[i].ToString()) : long.Parse(barcode[i].ToString()) * 3;
        }

        return (sum + checksum) % 10 == 0;
    }

    public Task<string> GetImage(string barcode)
    {
        var b = new Barcode(barcode, BarcodeStandard.Type.Ean13);
        b.ImageFormat = SKEncodedImageFormat.Png;
        b.IncludeLabel = true;

        // Use Arial if available, otherwise use DejaVu or default (can be installed on Linux in Docker)
        var typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Normal)
                       ?? SKTypeface.FromFamilyName("DejaVu Sans", SKFontStyle.Normal)
                       ?? SKTypeface.Default;
        b.LabelFont = new SKFont
        {
            Typeface = typeface,
            Size = 20,
        };
        b.Encode(BarcodeStandard.Type.Ean13, barcode, 300, 160).ToString();

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(b.ToJson()));
        SaveData saveData = Barcode.FromJson(stream);

        return Task.FromResult(saveData.Image);
    }
}