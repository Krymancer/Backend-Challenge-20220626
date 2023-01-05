using Domain.Products;

namespace Application.Products
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAsync();
        Task<Product?> GetAsync(string id);
        Task CreateAsync(Product newProduct);
        Task UpdateAsync(string id, Product updatedProduct);
        Task RemoveAsync(string id);
        Task<IEnumerable<Product>> GetPaged(int page, int pageSize);
    }
}
