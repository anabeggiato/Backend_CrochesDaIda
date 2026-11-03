using Services.Features.Products.UseCases.CreateProduct;
using Services.Features.Products.UseCases.GetProducts;
using Services.Features.Products.UseCases.GetProduct;
using Services.Features.Products.UseCases.DeleteProduct;
using Services.Features.Products.UseCases.UpdateProduct;
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

// Registra os endpoints
new CreateProduct().MapEndpoints(app);
new GetProducts().MapEndpoints(app);
new GetProduct().MapEndpoints(app);
new UpdateProduct().MapEndpoints(app);
new DeleteProduct().MapEndpoints(app);

app.Run();
