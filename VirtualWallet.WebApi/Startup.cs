using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VirtualWallet.Application;
using VirtualWallet.Application.Interfaces;
using VirtualWallet.Infrastructure.Identity;
using VirtualWallet.Infrastructure.Persistence;
using VirtualWallet.Infrastructure.Shared;
using VirtualWallet.WebApi.Extensions;
using VirtualWallet.WebApi.Services;

namespace VirtualWallet.WebApi
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

            services.AddApplicationLayer();
            services.AddIdentityInfrastructure(Configuration);
            services.AddPersistenceInfrastructure(Configuration);
            services.AddSharedInfrastructure(Configuration);
            services.AddSwaggerExtension();
            services.AddControllers();
            services.AddApiVersioningExtension();
            services.AddHealthChecks();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerExtension();
            app.UseErrorHandlingMiddleware();
            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
