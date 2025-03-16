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

    public static class Categories
    {
        private const string CategoryBase = $"{ApiBase}/categories";

        public const string GetAll = CategoryBase;
        public const string GetById = $"{CategoryBase}/{{categoryId:int}}";
        public const string GetBySlug = $"{CategoryBase}/{{categorySlug}}";
        public const string Create = CategoryBase;
    }
}