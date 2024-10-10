using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;

namespace IMS.UseCases.Products;

public class DeleteProductUseCase(IProductRepository productRepository) : IDeleteProductUseCase
{
    public async Task ExecuteAsync(int id)
    {
        await productRepository.DeleteProductByIdAsync(id);
    }
}
