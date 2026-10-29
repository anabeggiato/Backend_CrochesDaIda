using System.Collections.Concurrent;
using Services.Features.Products.Models;

namespace Services.Persistence;

public class ProductStore
{
    public ConcurrentDictionary<Guid, Product> Products { get; } = new();
}