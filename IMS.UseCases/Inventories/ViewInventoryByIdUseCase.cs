using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories;
public class ViewInventoryByIdUseCase(IInventoryRepository inventoryRepository) : IViewInventoryByIdUseCase
{
    public async Task<Inventory> ExecuteAsync(int inventoryId)
    {
        return await inventoryRepository.GetInventoryByIdAsync(inventoryId);
    }
}
