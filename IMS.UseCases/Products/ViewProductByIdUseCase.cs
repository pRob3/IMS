using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;

namespace IMS.UseCases.Products;

public class ViewProductByIdUseCase(IProductRepository productRepository) : IViewProductByIdUseCase
{
    public async Task<Product?> ExecuteAsync(int productId)
    {
        return await productRepository.GetProductByIdAsync(productId);
    }
}
