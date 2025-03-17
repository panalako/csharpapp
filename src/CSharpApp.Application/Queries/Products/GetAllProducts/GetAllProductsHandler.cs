namespace CSharpApp.Application.Queries.Products.GetAllProducts;

public class GetAllProductsHandler(ICoreHttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<GetAllProductsHandler> logger) : IRequestHandler<GetAllProductsQuery, IReadOnlyCollection<Product>?>
{
    private readonly ICoreHttpClient _httpClient = httpClient;
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;
    private readonly ILogger<GetAllProductsHandler> _logger = logger;

    public async Task<IReadOnlyCollection<Product>?> Handle(GetAllProductsQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetHttpResponseMessageAsync(_restApiSettings.Products!, cancellationToken);
            if(response is null) return null;
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var res = JsonSerializer.Deserialize<List<Product>>(content);

            return res?.AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError("Falied to retive data, {httpClientException}", ex.Message);
            return null;
        }
    }
}