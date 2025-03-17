using System.Text.Json;
using CSharpApp.Api;
using CSharpApp.Core.Dtos;

using Microsoft.AspNetCore.Mvc.Testing;


namespace CSharpApp.Tests;

public class ProductsEndpointsTests
{
    private HttpClient _httpClient;
    private readonly string ApiVersion = "1";
    private readonly string PathVersionVariable = "{version:apiVersion}";

    public ProductsEndpointsTests()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        _httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Fact]
    public async Task GetAllProductsSuccesfully()
    {
        var path = ApiEndpoints.Products.GetAll;
        var response = await _httpClient.GetAsync(path.Replace(PathVersionVariable, ApiVersion));
        var result = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(result));
    }

    [Fact]
    public async Task GetAllProductsFailiure()
    {
        await using var application = new MediatorReturnsNullProducts();
        var pghttpClient = application.CreateDefaultClient();

        var path = ApiEndpoints.Products.GetAll;
        var response = await pghttpClient.GetAsync(path.Replace(PathVersionVariable, ApiVersion));
        var result = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<List<Product>>(result);
        Assert.Empty(json!);
    }
}
