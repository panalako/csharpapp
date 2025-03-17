using System.Net;
using System.Text.Json;
using CSharpApp.Application.Queries.Products.GetAllProducts;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CSharpApp.Tests;

public class GetAllProductsHandlerTest
{
    private readonly Mock<ICoreHttpClient> _httpClientMock;
    private readonly Mock<IOptions<RestApiSettings>> _restApiSettingsMock;
    private readonly Mock<ILogger<GetAllProductsHandler>> _loggerMock;
    private readonly GetAllProductsHandler _handler;

    public GetAllProductsHandlerTest()
    {
        _httpClientMock = new Mock<ICoreHttpClient>();
        _restApiSettingsMock = new Mock<IOptions<RestApiSettings>>();
        _loggerMock = new Mock<ILogger<GetAllProductsHandler>>();

        var apiSettings = new RestApiSettings { BaseUrl = "https://test.com/", Products = "test" };
        _restApiSettingsMock.Setup(r => r.Value).Returns(apiSettings);

        _handler = new GetAllProductsHandler(
            _httpClientMock.Object,
            _restApiSettingsMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnProducts_WhenApiCallIsSuccessful()
    {
        var jsonProduct1 = $"{{\"id\":2,\"title\":\"Classic Red Pullover Hoodie\",\"slug\":\"classic-red-pullover-hoodie\",\"price\":100,\"description\":\"Elevate your casual wardrobe with our Classic Red Pullover Hoodie attire.\",\"category\":{{\"id\":1,\"name\":\"Clothess\",\"slug\":\"clothess\",\"image\":\"https://i.imgur.com/QkIa5tT.jpeg\",\"creationAt\":\"2025-03-16T16:55:44.000Z\",\"updatedAt\":\"2025-03-17T14:52:38.000Z\"}},\"images\":[\"https://i.imgur.com/1twoaDy.jpeg\",\"https://i.imgur.com/FDwQgLy.jpeg\",\"https://i.imgur.com/kg1ZhhH.jpeg\"],\"creationAt\":\"2025-03-16T16:55:44.000Z\",\"updatedAt\":\"2025-03-16T17:02:23.000Z\"}}";
        var jsonProduct2 = $"{{\"id\":3,\"title\":\"Classic Heather Gray Hoodie\",\"slug\":\"classic-heather-gray-hoodie\",\"price\":69,\"description\":\"Stay cozy and stylish with our Classic Heather Gray Hoodie.\",\"category\":{{\"id\":1,\"name\":\"Clothess\",\"slug\":\"clothess\",\"image\":\"https://i.imgur.com/QkIa5tT.jpeg\",\"creationAt\":\"2025-03-16T16:55:44.000Z\",\"updatedAt\":\"2025-03-17T14:52:38.000Z\"}},\"images\":[\"https://i.imgur.com/cHddUCu.jpeg\",\"https://i.imgur.com/CFOjAgK.jpeg\",\"https://i.imgur.com/wbIMMme.jpeg\"],\"creationAt\":\"2025-03-16T16:55:44.000Z\",\"updatedAt\":\"2025-03-16T16:55:44.000Z\"}}";

        var products = new List<Product>
        {
            JsonSerializer.Deserialize<Product>(jsonProduct1)!,
            JsonSerializer.Deserialize<Product>(jsonProduct2)!,
        };
        var productsJson = JsonSerializer.Serialize(products);
        var productsStringContent = new StringContent( productsJson, System.Text.Encoding.UTF8, "application/json" );
        HttpResponseMessage response =  new( HttpStatusCode.OK ) {Content =  productsStringContent };


        _httpClientMock.Setup(c => c.GetHttpResponseMessageAsync(It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult(response)!);

        var query = new GetAllProductsQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        _httpClientMock.Verify(c => c.GetHttpResponseMessageAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenApiCallFails()
    {
        List<Product?> products = [];
        var productsJson = JsonSerializer.Serialize(products);
        var productsStringContent = new StringContent( productsJson, System.Text.Encoding.UTF8, "application/json" );
        HttpResponseMessage response =  new( HttpStatusCode.OK ) {Content =  productsStringContent };

        var query = new GetAllProductsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
         _httpClientMock.Verify(c => c.GetHttpResponseMessageAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);
    }
}
