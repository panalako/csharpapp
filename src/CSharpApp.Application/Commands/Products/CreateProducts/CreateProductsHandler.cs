using System.Text;
using System.Text.Json.Serialization;

namespace CSharpApp.Application.Commands.Products.CreateProducts;

public class CreateProductHandler(ICoreHttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<CreateProductHandler> logger) : IRequestHandler<CreateProductsCommand, Product?>
{
    private readonly ICoreHttpClient _httpClient = httpClient;
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;
    private readonly ILogger<CreateProductHandler> _logger = logger;

    public async Task<Product?> Handle(CreateProductsCommand request, CancellationToken cancellationToken)
    {
        HttpResponseMessage? response = new();
        try
        {
            var json = JsonSerializer.Serialize(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            response = await _httpClient.GetHttpResponseMessageAsync(_restApiSettings.Products!, data, cancellationToken);
            if(response is null) return null;
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var res = JsonSerializer.Deserialize<Product>(content);

            return res;
        }
        catch (HttpRequestException ex)
        {
            var responseError = response!.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Falied to retive data, {httpClientException}, {response.Content}", ex.Message, responseError.Result);
            throw new HttpRequestException($"{responseError.Result}");
        }
    }
}