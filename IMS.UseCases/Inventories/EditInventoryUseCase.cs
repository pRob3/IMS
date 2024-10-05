using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories
{

    public class EditInventoryUseCase(IInventoryRepository inventoryRepository) : IEditInventoryUseCase
    {

        public async Task ExecuteAsync(Inventory inventory)
        {
            await inventoryRepository.UpdateInventoryAsync(inventory);
        }
    }
}
