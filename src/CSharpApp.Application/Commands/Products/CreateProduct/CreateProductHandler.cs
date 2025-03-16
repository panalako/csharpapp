using System.Text;

namespace CSharpApp.Application.Commands.Products.CreateProduct;

public class CreateProductHandler(ICoreHttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<CreateProductHandler> logger) : IRequestHandler<CreateProductCommand, Product?>
{
    private readonly ICoreHttpClient _httpClient = httpClient;
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;
    private readonly ILogger<CreateProductHandler> _logger = logger;

    public async Task<Product?> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        HttpResponseMessage? response = new();
        try
        {
            var json = JsonSerializer.Serialize(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            response = await _httpClient.GetHttpResponseMessageAsync(_restApiSettings.Products!, data, cancellationToken);
            response!.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var res = JsonSerializer.Deserialize<Product>(content);

            return res;
        }
        catch (Exception ex)
        {
            var responseError = response!.Content.ReadAsStringAsync();
            _logger.LogError("Falied to retive data, {httpClientException}, {response.Content}", ex.Message, responseError.Result);
            return null;
        }
    }
}