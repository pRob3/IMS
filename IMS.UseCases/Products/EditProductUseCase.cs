using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;

namespace IMS.UseCases.Products;

public class EditProductUseCase(IProductRepository productRepository) : IEditProductUseCase
{
    public async Task ExecuteAsync(Product product)
    {
        await productRepository.UpdateProductAsync(product);
    }
}
