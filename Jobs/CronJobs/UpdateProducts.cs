using Microsoft.Extensions.Hosting;

namespace Jobs.CronJobs
{
    public class UpdateProducts : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Every day at 8am local time
            using var timer = new CronTimer("0 8 * * *", TimeZoneInfo.Local);

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                // Do work
            }
        }
    }
}
