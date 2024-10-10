using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;

namespace IMS.UseCases.Products;

public class AddProductUseCase(IProductRepository productRepository) : IAddProductUseCase
{
    public async Task ExecuteAsync(Product product)
    {
        await productRepository.AddProductAsync(product);
    }
}
