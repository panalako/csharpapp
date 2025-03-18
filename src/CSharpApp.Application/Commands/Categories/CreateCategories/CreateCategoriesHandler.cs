using System.Text;
using CSharpApp.Core.Dtos.CategoriesDtos;

namespace CSharpApp.Application.Commands.Categories.CreateCategories;

public class CreateCategoriesHandler(ICoreHttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<CreateCategoriesHandler> logger) : IRequestHandler<CreateCategoriesCommand, CategoriesResponse?>
{
    private readonly ICoreHttpClient _httpClient = httpClient;
    private readonly RestApiSettings _restApiSettings = restApiSettings.Value;
    private readonly ILogger<CreateCategoriesHandler> _logger = logger;

    public async Task<CategoriesResponse?> Handle(CreateCategoriesCommand request, CancellationToken cancellationToken)
    {
        HttpResponseMessage? response = new();
        try
        {
            var json = JsonSerializer.Serialize(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            response = await _httpClient.GetHttpResponseMessageAsync(_restApiSettings.Categories!, data, cancellationToken);
            if(response is null) return null;
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var res = JsonSerializer.Deserialize<CategoriesResponse>(content);

            return res;
        }
        catch (Exception ex)
        {
            var responseError = response!.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Falied to retive data, {httpClientException}, {response.Content}", ex.Message, responseError.Result);
            throw new HttpRequestException($"{responseError.Result}");
        }
    }
}