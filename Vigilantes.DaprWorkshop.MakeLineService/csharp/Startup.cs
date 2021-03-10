using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Vigilantes.DaprWorkshop.MakeLineService
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://127.0.0.1:8080")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddMvc(opts =>
            {
                opts.InputFormatters.Insert(0, new CloudNative.CloudEvents.CloudEventJsonInputFormatter());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSubscribeHandler();
            });
            app.UseCloudEvents();
        }
    }
}
