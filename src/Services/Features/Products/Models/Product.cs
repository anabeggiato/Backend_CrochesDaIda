using Services.Features.Collections.Models;
using Services.Features.Categories.Models;

namespace Services.Features.Products.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public float Height { get; set; }
    public float Width { get; set; }
    public float Weight { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public Guid CollectionId { get; set; }
    public Collection Collection { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public Category Category {get;set;} = null;
}
