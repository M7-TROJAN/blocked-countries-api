using A_Tecchnologies_Assignment.Services;

namespace A_Tecchnologies_Assignment.BackgroundJobs;
public class TemporalBlockCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TemporalBlockCleanupService> _logger;

    public TemporalBlockCleanupService(IServiceProvider serviceProvider, ILogger<TemporalBlockCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[BackgroundService] TemporalBlockCleanupService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Create a scoped instance for IBlockService
                using (var scope = _serviceProvider.CreateScope())
                {
                    var blockService = scope.ServiceProvider.GetRequiredService<IBlockService>();
                    blockService.RemoveExpiredBlocks();
                }

                _logger.LogInformation($"[Cleanup] Expired temporal blocks removed at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Cleanup Error] {ex.Message}");
            }

            // Sleep for 5 minutes before running again
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }

        _logger.LogInformation("[BackgroundService] TemporalBlockCleanupService stopped.");
    }
}


/*
using A_Tecchnologies_Assignment.Services;

namespace A_Tecchnologies_Assignment.BackgroundJobs;

public class TemporalBlockCleanupService : BackgroundService
{
    private readonly IBlockService _blockService;

    public TemporalBlockCleanupService(IBlockService blockService)
    {
        _blockService = blockService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("[BackgroundService] TemporalBlockCleanupService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _blockService.RemoveExpiredBlocks();
                Console.WriteLine($"[Cleanup] Expired temporal blocks removed at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cleanup Error] {ex.Message}");
            }

            // Sleep for 5 minutes before running again
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }

        Console.WriteLine("[BackgroundService] TemporalBlockCleanupService stopped.");
    }
}
*/