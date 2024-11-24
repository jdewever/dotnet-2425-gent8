// todo: rewrite this, include in CartService or rename to ScanService?
public class ScanBarcodeService
{
    private string? barcode;
    public string? Barcode
    {
        get => barcode;
        set
        {
            if (barcode != value)
            {
                barcode = value;
                OnBarcodeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public event EventHandler? OnBarcodeChanged;
}