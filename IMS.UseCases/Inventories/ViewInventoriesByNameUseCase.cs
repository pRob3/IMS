using IMS.CoreBusiness;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories;
public class ViewInventoriesByNameUseCase(IInventoryRepository inventoryRepository) : IViewInventoriesByNameUseCase
{
    public async Task<IEnumerable<Inventory>> ExecuteAsync(string name = "")
    {
        return await inventoryRepository.GetInventoriesByNameAsync(name);
    }
}
