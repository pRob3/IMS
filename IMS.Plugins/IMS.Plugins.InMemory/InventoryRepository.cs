using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory;

public class InventoryRepository : IInventoryRepository
{
    private List<Inventory> _inventories;

    public InventoryRepository()
    {
        _inventories = new List<Inventory>
        {
            new Inventory { InventoryId = 1, Name = "Bike Seat", Quantity = 10, Price = 2 },
            new Inventory { InventoryId = 2, Name = "Bike Body", Quantity = 10, Price = 15 },
            new Inventory { InventoryId = 2, Name = "Bike Wheels", Quantity = 20, Price = 8 },
            new Inventory { InventoryId = 3, Name = "Bike Pedals", Quantity = 20, Price = 1 },
        };
    }

    public Task AddInventortyAsync(Inventory inventory)
    {
        if (_inventories.Any(i => i.Name.Equals(inventory.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return Task.CompletedTask;
        }

        var maxId = _inventories.Max(i => i.InventoryId);
        inventory.InventoryId = maxId + 1;

        _inventories.Add(inventory);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return await Task.FromResult(_inventories.AsEnumerable());
        }

        return _inventories.Where(i => i.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
    }

    public Task<Inventory> GetInventoryByIdAsync(int inventoryId)
    {
        return Task.FromResult(_inventories.First(i => i.InventoryId == inventoryId));
    }

    public Task UpdateInventoryAsync(Inventory inventory)
    {
        if (_inventories.Any(i => i.InventoryId != inventory.InventoryId && i.
            Name.Equals(inventory.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return Task.CompletedTask;
        }

        var invToUpdate = _inventories.FirstOrDefault(i => i.InventoryId == inventory.InventoryId);
        if (invToUpdate is not null)
        {
            invToUpdate.Name = inventory.Name;
            invToUpdate.Quantity = inventory.Quantity;
            invToUpdate.Price = inventory.Price;
        }

        return Task.CompletedTask;
    }
}
