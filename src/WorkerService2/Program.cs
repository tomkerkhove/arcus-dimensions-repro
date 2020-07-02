using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace WorkerService2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var logger = new LoggerConfiguration()
                        .WriteTo.Sink<CustomSink>()
                        .CreateLogger();

                    services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger));
                    services.AddHostedService<Worker>();
                });
    }

    public class CustomSink : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            if (logEvent.MessageTemplate.Text.StartsWith("Metric"))
            {
                bool contextFound = logEvent.Properties.TryGetAsDictionary("Context", out IReadOnlyDictionary<ScalarValue, LogEventPropertyValue> context);
                if (contextFound)
                {
                    foreach (KeyValuePair<ScalarValue, LogEventPropertyValue> contextProperty in context)
                    {
                        var value = contextProperty.Value.ToDecentString();
                    }
                }
            }
        }
    }
}
