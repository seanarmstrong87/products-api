using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductsWebApi.Dtos;

/// <summary>
/// A DTO for Posting or putting a product.
/// </summary>
public class PostProductDto
{

    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter<ColourChoice>))]
    public ColourChoice? Colour { get; set; }
}