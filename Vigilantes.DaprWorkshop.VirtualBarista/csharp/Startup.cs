using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Vigilantes.DaprWorkshop.VirtualBarista.Controllers;

namespace Vigilantes.DaprWorkshop.VirtualBarista
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
            .AddDapr(builder => builder
            .UseHttpEndpoint(Configuration.GetValue<string>("DAPR_HTTP_ENDPOINT"))
            .UseGrpcEndpoint(Configuration.GetValue<string>("DAPR_GRPC_ENDPOINT")));
            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseCloudEvents();
        }
    }
}
