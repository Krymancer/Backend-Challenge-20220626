using Quartz;
using Scrapper;

namespace Jobs
{
    public class ScrappingJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var scrapper = new WebScrapper();
            scrapper.Scrap();
            return Task.FromResult(true);
        }
    }
}
