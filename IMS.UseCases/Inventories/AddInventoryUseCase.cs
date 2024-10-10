using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories;

public class AddInventoryUseCase(IInventoryRepository inventoryRepository) : IAddInventoryUseCase
{
    public async Task ExecuteAsync(Inventory inventory)
    {
        await inventoryRepository.AddInventoryAsync(inventory);
    }
}
