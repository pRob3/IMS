using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities;
public class PurchaseInventoryUseCase(IInventoryTransactionRepository inventoryTransactionRepository, IInventoryRepository inventoryRepository) : IPurchaseInventoryUseCase
{

    public async Task ExecuteAsync(string poNumber, Inventory inventory, int quantity, string doneBy)
    {
        // Insert record
        await inventoryTransactionRepository.PurchaseAsync(poNumber, inventory, quantity, doneBy, inventory.Price);

        // Increase quantity
        inventory.Quantity += quantity;
        await inventoryRepository.UpdateInventoryAsync(inventory);

    }
}
