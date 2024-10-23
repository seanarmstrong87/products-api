namespace ProductsWebApi.Services.Models;

/// <summary>
/// A service model representing a product which ensures all required fields are provided.
/// </summary>
public class Product
{
    public Product(string name, string description, double price, ColourChoice? colour)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        
        Name = name;
        Description = description;
        Price = price;
        Colour = colour ?? ColourChoice.NotApplicable;
    }
    
    /// <summary>
    /// Id of the product. Only required once the product is persisted.
    /// </summary>
    public int Id { get; set; }
    
    public string Name { get; }
    
    public string Description { get; }
    
    public double Price { get; }
    
    public ColourChoice Colour { get; }
    
    /// <summary>
    /// Date of creation of the product.
    /// </summary>
    public DateTime? CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}