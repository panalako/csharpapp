namespace CSharpApp.Application.Queries.Products.GetProductsBySlug;

public class GetProductsBySlugHandler(ICoreHttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<GetProductsBySlugHandler> logger) : IRequestHandler<GetProductsBySlugQuery, Product?>
{
    private readonly ICoreHttpClient _httpClient = httpClient;
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;
    private readonly ILogger<GetProductsBySlugHandler> _logger = logger;

    public async Task<Product?> Handle(GetProductsBySlugQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetHttpResponseMessageAsync($"{_restApiSettings.Products!}/slug/{request.ProductSlug}", cancellationToken);
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