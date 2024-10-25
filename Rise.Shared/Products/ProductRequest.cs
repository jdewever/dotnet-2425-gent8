namespace Rise.Shared.Products;

public class ProductRequest
{
    public class Index
    {
        public IList<int>? CategoryIds { get; set; } = [];
        public string? Location { get; set; }
        public int? MaxInStock { get; set; }
        public int? MinInStock { get; set; }
        public int? MaxOnOrder { get; set; }
        public int? MinOnOrder { get; set; }
        public string? Searchterm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
    }
}