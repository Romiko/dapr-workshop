using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Vigilantes.DaprWorkshop.LoyaltyService.Models;

namespace Vigilantes.DaprWorkshop.LoyaltyService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.With(new UtcTimestampEnricher())
                .Destructure.ByTransforming<OrderSummary>(os => new { OrderId = os.OrderId, StoreId = os.StoreId, CustomerName = os.CustomerName, LoyaltyId = os.LoyaltyId, OrderItemCount = os.OrderItems.Count, OrderTotal = os.OrderTotal })
                .Destructure.ByTransforming<LoyaltySummary>(ls => new { LoyaltyId = ls.LoyaltyId, CustomerName = ls.CustomerName, PointsEarned = ls.PointsEarned, PointTotal = ls.PointTotal })
                .WriteTo.Console(outputTemplate: "[{UtcTimestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    class UtcTimestampEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory pf)
        {
            logEvent.AddPropertyIfAbsent(pf.CreateProperty("UtcTimestamp", logEvent.Timestamp.UtcDateTime));
        }
    }
}
