namespace Rise.Shared.Products;

public class ProductRequest
{
    public class Index
    {
        public IList<int>? CategoryIds { get; set; } = [];
        public string? Location { get; set; }
    }
}