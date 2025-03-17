using CSharpApp.Application.Queries.Products.GetAllProducts;
using CSharpApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace CSharpApp.Tests;

internal class MediatorReturnsNullProducts(string environment = "Development") : WebApplicationFactory<Program>
{
    private readonly string _environment = environment;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(_environment);

        builder.ConfigureServices(services =>
        {
            services.AddScoped(sp =>
            {
                var mockMediator = new Mock<IMediator>();
                IReadOnlyCollection<Product>? res = null;

                mockMediator
                    .Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(res));

                return mockMediator.Object;

            });
        });

        return base.CreateHost(builder);
    }
}
