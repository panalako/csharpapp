using CSharpApp.Api.Behaviors;
using CSharpApp.Api.Endpoints;
using CSharpApp.Api.Middlewares.AuthMiddlewares;
using CSharpApp.Api.Middlewares.PerformanceMiddlewares;
using CSharpApp.Api.Middlewares.ValidationMiddlewares;
using CSharpApp.Application.Queries.Products.GetAllProducts;
using CSharpApp.Infrastructure.ValidationExtentions;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();

builder.Services.AddAuthTokenProvider();
builder.Services.AddValidators();

builder.Services.AddMediatR(config => 
{
    config.RegisterServicesFromAssemblyContaining<GetAllProductsHandler>();
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    
});

var app = builder.Build();
app.UseMiddleware<PerformanceMiddleware>();

app.CreateApiVersionSet();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<TokenRefreshMiddleware>();
app.UseMiddleware<ValidationMiddleware>();
app.MapApiEndpoints();

//app.UseHttpsRedirection();

app.Run();