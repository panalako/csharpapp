namespace CSharpApp.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api/v{version:apiVersion}";

    public static class Products
    {
        private const string ProductsBase = $"{ApiBase}/products";

        public const string Create = ProductsBase;
    }
}