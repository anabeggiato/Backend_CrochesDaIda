using Services.Features.Products.UseCases.CreateProduct;
using Services.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Registra o armazenamento em memória
builder.Services.AddSingleton<ProductStore>();

var app = builder.Build();

// Registra os endpoints
new CreateProduct().MapEndpoints(app);

app.Run();