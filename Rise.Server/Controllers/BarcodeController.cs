using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rise.Shared.Barcodes;

namespace Rise.Server.Controllers;

[ApiController]
[Route("api/barcodes/")]
[Authorize]
public class BarcodeController : ControllerBase
{
    private readonly IBarcodeService barcodeService;

    public BarcodeController(IBarcodeService barcodeService)
    {
        this.barcodeService = barcodeService;
    }

    // get a new barcode that's not in use
    // since this endpoint checks the database state (and will result in a change later, it is better to use POST)
    // the result is not idempotent en causes a change, hence POST
    [HttpPost]
    [Authorize(Roles = "Administrator, InventoryManager")]
    public async Task<BarcodeResponse> GetNewBarcode()
    {
        return await barcodeService.GetNewBarcode();
    }

    // get the image of a product by barcode
    [HttpGet("{barcode}/image")]
    [AllowAnonymous]
    public async Task<ActionResult> GetBarcodeImage(string barcode)
    {
        var imageBase64 = await barcodeService.GetImage(barcode);
        var imageBytes = Convert.FromBase64String(imageBase64);
        return File(imageBytes, "image/png");
    }
}