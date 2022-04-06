using Microsoft.Extensions.DependencyInjection;
using Play.Catalog.Dtos;
using Play.Catalog.Entities;
using Play.Catalog.Services;
using Play.Common.MongoDb;

namespace Play.Catalog
{

    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddMongo()
                    .AddRepository<Item>("items");

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<ICatalogService, CatalogService>();

            return services;
        }
    }
}