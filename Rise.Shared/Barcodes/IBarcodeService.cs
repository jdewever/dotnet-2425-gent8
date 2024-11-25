namespace Rise.Shared.Barcodes;

public interface IBarcodeService
{
    Task<BarcodeResponse> GetNewBarcode();
    bool IsValidBarcode(string barcode);
    Task<string> GetImage(string barcode);
}