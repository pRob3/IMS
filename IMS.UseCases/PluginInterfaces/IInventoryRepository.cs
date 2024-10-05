using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces;
public interface IInventoryRepository
{
    Task AddInventortyAsync(Inventory inventory);
    Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name);
}
