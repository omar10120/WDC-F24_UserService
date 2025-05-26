using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WDC_F24.Application.Interfaces;
using WDC_F24.infrastructure.Data;
using WDC_F24.Infrastructure.Messaging;






namespace WDC_F24.infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly("WDC-F24.infrastructure")
            ));

            //services.AddSwaggerGen(option =>
            //{ 
            //    option.SwaggerDoc("UserApp", new OpenApiInfo { Title = "User App API", Version = "2.0" });
            //});

            // Dynamic Authorization Policies
           

            services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();

            return services;
        }
    }
}
