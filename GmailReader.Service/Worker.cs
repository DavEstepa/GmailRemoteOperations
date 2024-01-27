using GmailReader.Infrastructure.Services;

namespace GmailReader.Service
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
            var mmfHandler = new MemoryMappedFileHandler();
            while (!stoppingToken.IsCancellationRequested)
            {
                mmfHandler.ReadValue();

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                //await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
