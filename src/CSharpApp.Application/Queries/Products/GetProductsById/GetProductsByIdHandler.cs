namespace CSharpApp.Application.Queries.Products.GetProductsById;

public class GetProductsByIdHandler(ICoreHttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<GetProductsByIdHandler> logger) : IRequestHandler<GetProductsByIdQuery, Product?>
{
    private readonly ICoreHttpClient _httpClient = httpClient;
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;
    private readonly ILogger<GetProductsByIdHandler> _logger = logger;

    public async Task<Product?> Handle(GetProductsByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetHttpResponseMessageAsync($"{_restApiSettings.Products!}/{request.ProductId}", cancellationToken);
            response!.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var res = JsonSerializer.Deserialize<Product>(content);

            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError("Falied to retive data, {httpClientException}", ex.Message);
            return null;
        }
    }
}
