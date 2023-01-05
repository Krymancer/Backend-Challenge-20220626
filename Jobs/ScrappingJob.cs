using Application.Products;
using Quartz;
using Scrapper;

namespace Jobs
{
    public class ScrappingJob : IJob
    {
        private readonly WebScrapper _scrapper;

        public ScrappingJob(IProductService productService)
        {
            _scrapper = new WebScrapper(productService);
        }
        public Task Execute(IJobExecutionContext context)
        {
            return _scrapper.Scrap();
        }
    }
}
