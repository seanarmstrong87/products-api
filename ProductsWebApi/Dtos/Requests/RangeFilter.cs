namespace ProductsWebApi.Dtos.Requests;

/// <summary>
/// A DTO for defining a range from and to.
/// </summary>
/// <typeparam name="T"></typeparam>
public class RangeFilter<T>
{
    public T? From { get; set; }
    public T? To { get; set; }
}