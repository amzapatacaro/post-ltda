using DataAccess;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class PostLtdaServiceCollectionExtensions
    {
        public static IServiceCollection AddPostLtdaPersistence(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<JujuTestContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Development"))
            );

            services.AddScoped<IBaseModel<Customer>>(sp => new BaseModel<Customer>(
                sp.GetRequiredService<JujuTestContext>()
            ));
            services.AddScoped<IBaseModel<Post>>(sp => new BaseModel<Post>(
                sp.GetRequiredService<JujuTestContext>()
            ));

            services.AddScoped<CustomerService>();
            services.AddScoped<ICustomerService>(sp => sp.GetRequiredService<CustomerService>());
            services.AddScoped<PostService>();
            services.AddScoped<IPostService>(sp => sp.GetRequiredService<PostService>());

            return services;
        }
    }
}
