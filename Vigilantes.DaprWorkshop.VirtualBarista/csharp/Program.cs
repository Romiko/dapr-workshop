using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Vigilantes.DaprWorkshop.VirtualBarista.Models;

namespace Vigilantes.DaprWorkshop.VirtualBarista
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
                .Destructure.ByTransforming<OrderItemSummary>(ois => new { MenuItemId = ois.MenuItemId, MenuItemName = ois.MenuItemName, Quantity = ois.Quantity })
                .Destructure.ByTransforming<OrderSummary>(os => new { OrderId = os.OrderId, StoreId = os.StoreId, CustomerName = os.CustomerName, LoyaltyId = os.LoyaltyId, OrderItemCount = os.OrderItems.Count, OrderTotal = os.OrderTotal })
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
