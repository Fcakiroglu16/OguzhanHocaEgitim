using System.ComponentModel.DataAnnotations;

namespace Applications.Products.Create
{
    public record CreateProductRequest(
        [Required(ErrorMessage = "isim alanı boş olamaz")]
        string? Name,
        [Required(ErrorMessage = "fiyat alanı boş olamaz")]
        decimal? Price);
}