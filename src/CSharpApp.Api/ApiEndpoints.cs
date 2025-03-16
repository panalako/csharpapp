namespace CSharpApp.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api/v{version:apiVersion}";

    public static class Products
    {
        private const string ProductsBase = $"{ApiBase}/products";

        public const string GetAll = ProductsBase;
        public const string GetById = $"{ProductsBase}/{{productId:int}}";
        public const string GetBySlug = $"{ProductsBase}/{{productSlug}}";
        public const string Create = ProductsBase;
    }
}