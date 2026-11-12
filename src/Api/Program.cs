using Services.Features.Products.UseCases.CreateProduct;
using Services.Features.Products.UseCases.GetProducts;
using Services.Features.Products.UseCases.GetProduct;
using Services.Features.Products.UseCases.DeleteProduct;
using Services.Features.Products.UseCases.UpdateProduct;

using Services.Features.Collections.UseCases.CreateCollection;
using Services.Features.Collections.UseCases.GetCollections;
using Services.Features.Collections.UseCases.GetCollection;
using Services.Features.Collections.UseCases.DeleteCollection;
using Services.Features.Collections.UseCases.UpdateCollection;

using Microsoft.EntityFrameworkCore;
using Services.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Registra o armazenamento em memória
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

// Registra os endpoints de produto
new CreateProduct().MapEndpoints(app);
new GetProducts().MapEndpoints(app);
new GetProduct().MapEndpoints(app);
new UpdateProduct().MapEndpoints(app);
new DeleteProduct().MapEndpoints(app);

// Registra osendpoints de coleção
new CreateCollection().MapEndpoints(app);
new GetCollections().MapEndpoints(app);
new GetCollection().MapEndpoints(app);
new UpdateCollection().MapEndpoints(app);
new DeleteCollection().MapEndpoints(app);


app.Run();
