﻿using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer;
public class InventoryEFCoreRepository : IInventoryRepository
{
    private readonly IDbContextFactory<DBContext> _contextFactory;

    public InventoryEFCoreRepository(IDbContextFactory<DBContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task AddInventoryAsync(Inventory inventory)
    {
        using var db = _contextFactory.CreateDbContext();

        db.Inventories?.Add(inventory);
        await db.SaveChangesAsync();
    }

    public async Task DeleteInventoryByIdAsync(int id)
    {
        using var db = _contextFactory.CreateDbContext();
        var inventory = db.Inventories?.Find(id);
        if (inventory is null)
        {
            return;
        }

        db.Inventories?.Remove(inventory);
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
    {
        using var db = _contextFactory.CreateDbContext();
        return await db.Inventories.Where(x => x.Name.ToLower().IndexOf(name.ToLower()) >= 0)?.ToListAsync();
    }

    public async Task<Inventory> GetInventoryByIdAsync(int inventoryId)
    {
        using var db = _contextFactory.CreateDbContext();
        var inventory = await db.Inventories.FindAsync(inventoryId);
        if (inventory is not null)
        {
            return inventory;
        }

        return new Inventory();
    }

    public async Task UpdateInventoryAsync(Inventory inventory)
    {
        using var db = _contextFactory.CreateDbContext();
        var inv = await db.Inventories.FindAsync(inventory.Id);

        if (inv is not null)
        {
            inv.Name = inventory.Name;
            inv.Price = inventory.Price;
            inv.Quantity = inventory.Quantity;

            await db.SaveChangesAsync();
        }
    }
}

