using AreaAccountApi.Filters;
using AreaAccountApi.Services;
using AreaAccountApi.Sieve.Filters;
using AreaAccountData.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Sieve.Services;

namespace AreaAccountApi
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
            services.AddDbContext<AreaAccountContext>();
            services.AddScoped<SieveProcessor>();
            services.AddControllers(options => options.Filters.Add<HttpGlobalExceptionFilter>());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AreaAccountApi", Version = "v1" });
            });
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISieveCustomFilterMethods, AreaAccountCustomFilters>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AreaAccountApi v1"));
            }

            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetService<AreaAccountContext>();
            context.Database.EnsureCreated();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}