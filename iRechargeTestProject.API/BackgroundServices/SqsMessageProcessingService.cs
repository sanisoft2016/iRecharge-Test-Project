using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using iRechargeTestProject.Application.Service;
using Microsoft.Extensions.DependencyInjection;

namespace iRechargeTestProject.API.BackgroundServices
{
    public class SqsMessageProcessingService : BackgroundService
    {
        private readonly ILogger<SqsMessageProcessingService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _pollingInterval = TimeSpan.FromSeconds(10); // Poll every 10 seconds

        public SqsMessageProcessingService(
            ILogger<SqsMessageProcessingService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SQS Message Processing Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Polling for SQS messages...");
                    
                    // Create a scope for the scoped service
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var sqsProductUpdateService = 
                            scope.ServiceProvider.GetRequiredService<SqsProductUpdateService>();
                        await sqsProductUpdateService.SubscribeAndUpdateProductAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing SQS messages");
                }

                await Task.Delay(_pollingInterval, stoppingToken);
            }

            _logger.LogInformation("SQS Message Processing Service is stopping.");
        }
    }
}