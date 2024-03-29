using API.Helpers;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class AppServiceExtensions
    {
        /// <summary>
        /// Collection of application services, we generated special class for them so that startup wolud not be too messy
        /// See Startup.cs for more details
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, 
            IConfiguration config)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<ITokenService, TokenService>();
            
            services.AddScoped<IFileStorageService, StoreInApplicationService>();
            services.AddHttpContextAccessor();
            
            services.AddDbContext<HappyKidsContext>(options =>
               options.UseSqlServer(
                   config.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.UseNetTopologySuite()));

             services.AddSingleton<IConnectionMultiplexer>(c => {
                var configuration = ConfigurationOptions.Parse(config
                    .GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            services.AddAutoMapper(typeof(MappingHelper).Assembly);

            services.AddSingleton(provider => new MapperConfiguration(config =>
            {
                var geometryFactory = provider.GetRequiredService<GeometryFactory>();
                config.AddProfile(new MappingHelper(geometryFactory));
            }).CreateMapper());

            services.AddSingleton<GeometryFactory>(NtsGeometryServices
               .Instance.CreateGeometryFactory(srid: 4326));

            return services;
        }
    }
}