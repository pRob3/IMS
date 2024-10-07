using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces;
public interface IInventoryRepository
{
    Task AddInventortyAsync(Inventory inventory);
    Task DeleteInventoryByIdAsync(int id);
    Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name);
    Task<Inventory> GetInventoryByIdAsync(int inventoryId);
    Task UpdateInventoryAsync(Inventory inventory);
}
