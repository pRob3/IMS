
using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces;
public interface IProductRepository
{
    Task AddProductAsync(Product inventory);
    Task DeleteProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
    Task<Product?> GetProductByIdAsync(int inventoryId);
    Task UpdateProductAsync(Product inventory);
}
