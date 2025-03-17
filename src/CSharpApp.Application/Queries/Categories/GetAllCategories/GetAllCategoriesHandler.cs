using CSharpApp.Core.Dtos.CategoriesDtos;

namespace CSharpApp.Application.Queries.Categories.GetAllCategories;

public class GetAllCategoriesHandler(ICoreHttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<GetAllCategoriesHandler> logger) : IRequestHandler<GetAllCategoriesQuery, IReadOnlyCollection<CategoriesResponse>?>
{
    private readonly ICoreHttpClient _httpClient = httpClient;
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;
    private readonly ILogger<GetAllCategoriesHandler> _logger = logger;

    public async Task<IReadOnlyCollection<CategoriesResponse>?> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetHttpResponseMessageAsync(_restApiSettings.Categories!, cancellationToken);
            if(response is null) return null;
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var res = JsonSerializer.Deserialize<List<CategoriesResponse>>(content);

            return res?.AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError("Falied to retive data, {httpClientException}", ex.Message);
            return null;
        }
    }
}