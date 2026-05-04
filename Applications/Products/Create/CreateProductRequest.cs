using System.ComponentModel.DataAnnotations;

namespace Applications.Products.Create
{
    public record CreateProductRequest(
        string? Name,
        decimal? Price,
        int CategoryId);
}
    