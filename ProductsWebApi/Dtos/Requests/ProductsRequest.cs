namespace ProductsWebApi.Dtos.Requests;

public class ProductsRequest
{
    public ColourChoice? Colour { get; set; }
    
    public RangeFilter<double>? Price { get; set; }
}