using Services.Features.Products.Models;

namespace Services.Features.Collections.Models;

public class Collection
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
