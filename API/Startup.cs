using Business;
using DataAccess;
using DataAccess.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace ProjectAPI.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<JujuTestContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Development"))
            );

            services.AddScoped<IBaseModel<Customer>>(sp => new BaseModel<Customer>(
                sp.GetRequiredService<JujuTestContext>()
            ));
            services.AddScoped<IBaseModel<Post>>(sp => new BaseModel<Post>(
                sp.GetRequiredService<JujuTestContext>()
            ));

            services.AddScoped<CustomerService>();
            services.AddScoped<ICustomerService>(sp => sp.GetRequiredService<CustomerService>());
            services.AddScoped<IBaseService<Customer>>(sp =>
                sp.GetRequiredService<CustomerService>()
            );
            services.AddScoped<PostService>();
            services.AddScoped<IPostService>(sp => sp.GetRequiredService<PostService>());
            services.AddScoped<IBaseService<Post>>(sp => sp.GetRequiredService<PostService>());

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // ======== CONFIGURACIÓN DE SWAGGER =========
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TestAPI", Version = "v1" });
            });
            services.AddHttpContextAccessor();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ======== CONFIGURACIÓN DE SWAGGER =========
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "TestAPI v1");
            });

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseSession();
            app.UseMvc();
        }
    }
}
