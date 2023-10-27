using VelocipedSite.DAL.Repositories.Interfaces;

namespace VelocipedSite.HostedServices;

public class ExpiredTokenCleanerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ExpiredTokenCleanerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var profileRepository = scope.ServiceProvider.GetRequiredService<IProfileRepository>();
                await profileRepository.RemoveExpiredTokens(stoppingToken);
            }
            await Task.Delay(60000, stoppingToken);
        }
    }
}