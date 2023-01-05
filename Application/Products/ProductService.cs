using Data;
using Domain.Products;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Application.Products
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductService(
            IOptions<OpenFoodFactsDataBaseSettings> openFoodFactsDataBaseSettings)
        {
            var mongoClient = new MongoClient(
                openFoodFactsDataBaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                openFoodFactsDataBaseSettings.Value.Database);

            _productCollection = mongoDatabase.GetCollection<Product>(
                openFoodFactsDataBaseSettings.Value.ProductsColletion);
        }

        public async Task<IEnumerable<Product>> GetAsync() =>
            await _productCollection.Find(_ => true).ToListAsync();

        public async Task<Product?> GetByCodeAsync(string code) =>
            await _productCollection.Find(p => p.Code == code).FirstOrDefaultAsync();

        public async Task<Product?> GetAsync(string id) =>
            await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product newProduct) =>
            await _productCollection.InsertOneAsync(newProduct);

        public async Task UpdateAsync(string id, Product updatedProduct) =>
            await _productCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _productCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<IEnumerable<Product>> GetPaged(int page, int pageSize)
        {
            return await _productCollection.Find(_ => true).Skip(page * pageSize).Limit(pageSize).ToListAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                await CreateAsync(product);
            }
        }
    }
}