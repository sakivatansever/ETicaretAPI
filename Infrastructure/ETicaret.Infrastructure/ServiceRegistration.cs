using ETicaret.Infrastructure.Enums;
using ETicaret.Infrastructure.Services;
using ETicaret.Infrastructure.Services.Storage;
using ETicaret.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Application.Abstraction.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService,StorageService>();  
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T:class,IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection,StorageType storageType) 
        {
            switch (storageType)
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                   // serviceCollection.AddScoped<IStorage, >();
                    break;
                case StorageType.AWS:
                    break;
                default:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
