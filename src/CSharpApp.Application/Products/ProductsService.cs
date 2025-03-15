namespace CSharpApp.Application.Products;

public class ProductsService : IProductsService
{
    private readonly ICoreHttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(IOptions<RestApiSettings> restApiSettings, 
        ILogger<ProductsService> logger, ICoreHttpClient httpClient)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Product>?> GetProducts()
    {   
        var response = new HttpResponseMessage();

        try
        {
            response = await _httpClient.GetHttpResponseMessageAsync(_restApiSettings.Products!);
        }
        catch (Exception ex)
        {
            _logger.LogError("Falied to retive data, {httpClientException}", ex.Message);
            return null;
        }

        response!.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var res = JsonSerializer.Deserialize<List<Product>>(content);
        
        return res?.AsReadOnly();
    }
}