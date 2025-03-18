using CSharpApp.Core.Dtos.CategoriesDtos;

namespace CSharpApp.Application.Queries.Categories.GetCategoriesBySlug;

public class GetCategoriesBySlugHandler(ICoreHttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<GetCategoriesBySlugHandler> logger) : IRequestHandler<GetCategoriesBySlugQuery, CategoriesResponse?>
{
    private readonly ICoreHttpClient _httpClient = httpClient;
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;
    private readonly ILogger<GetCategoriesBySlugHandler> _logger = logger;

    public async Task<CategoriesResponse?> Handle(GetCategoriesBySlugQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetHttpResponseMessageAsync($"{_restApiSettings.Categories!}/slug/{request.CategorySlug}", cancellationToken);
            if(response is null) return null;
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var res = JsonSerializer.Deserialize<CategoriesResponse>(content);

            return res;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("Falied to retive data, {httpClientException}", ex.Message);
            return null;
        }
    }
}