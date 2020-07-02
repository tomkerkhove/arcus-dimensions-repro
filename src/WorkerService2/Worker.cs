using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService2
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var contextualInformation = new Dictionary<string, object>
                {
                    {"Repo_Name", "Foo"}
                };
                _logger.LogMetric( "Image Pulls", 10, contextualInformation);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
