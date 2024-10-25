public class BarcodeService
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