namespace ProductsWebApi.Dtos;

/// <summary>
/// A DTO for getting or updating a product.
/// </summary>
public class GetProductDto : PostProductDto
{
    
    public int Id { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}