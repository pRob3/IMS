using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories;

public class DeleteInventoryUseCase(IInventoryRepository inventoryRepository) : IDeleteInventoryUseCase
{

    public async Task ExecuteAsync(int id)
    {
        await inventoryRepository.DeleteInventoryByIdAsync(id);
    }
}
