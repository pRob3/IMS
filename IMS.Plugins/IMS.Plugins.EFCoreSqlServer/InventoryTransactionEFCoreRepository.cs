using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer;

public class InventoryTransactionEFCoreRepository : IInventoryTransactionRepository
{
    private readonly IDbContextFactory<DBContext> _contextFactory;

    public InventoryTransactionEFCoreRepository(IDbContextFactory<DBContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType)
    {
        using var db = _contextFactory.CreateDbContext();


        var query = from it in db.InventoryTransactions
                    join inv in db.Inventories on it.InventoryId equals inv.Id
                    where (string.IsNullOrWhiteSpace(inventoryName) || inv.Name.ToLower().IndexOf(inventoryName.ToLower()) >= 0) &&
                        (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date) &&
                        (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date) &&
                        (!transactionType.HasValue || it.ActivityType == transactionType)
                    select it;

        return await query.Include(x => x.Inventory).ToListAsync();
    }

    public async Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price)
    {
        using var db = _contextFactory.CreateDbContext();

        db.InventoryTransactions?.Add(new InventoryTransaction
        {
            ProductionNumber = productionNumber,
            InventoryId = inventory.Id,
            QuantityBefore = inventory.Quantity,
            ActivityType = InventoryTransactionType.ProduceProduct,
            QuantityAfter = inventory.Quantity - quantityToConsume,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy,
            UnitPrice = price
        });

        await db.SaveChangesAsync();
    }

    public async Task PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
    {
        using var db = _contextFactory.CreateDbContext();

        db.InventoryTransactions?.Add(new InventoryTransaction
        {
            PONumber = poNumber,
            InventoryId = inventory.Id,
            QuantityBefore = inventory.Quantity,
            ActivityType = InventoryTransactionType.PurchaseInventory,
            QuantityAfter = inventory.Quantity + quantity,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy,
            UnitPrice = price
        });

        await db.SaveChangesAsync();
    }
}