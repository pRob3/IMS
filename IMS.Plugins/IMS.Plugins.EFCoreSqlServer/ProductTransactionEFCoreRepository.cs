using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer;
public class ProductTransactionEFCoreRepository : IProductTransactionRepository
{
    private readonly IDbContextFactory<IMSContext> _contextFactory;
    private readonly IProductRepository _productRepository;
    private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
    private readonly IInventoryRepository _inventoryRepository;

    public ProductTransactionEFCoreRepository(IDbContextFactory<IMSContext> contextFactory, IProductRepository productRepository,
        IInventoryTransactionRepository inventoryTransactionRepository,
        IInventoryRepository inventoryRepository)
    {
        _contextFactory = contextFactory;
        _productRepository = productRepository;
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _inventoryRepository = inventoryRepository;
    }

    public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
    {
        using var db = _contextFactory.CreateDbContext();

        var prod = await _productRepository.GetProductByIdAsync(product.Id);
        if (prod is not null && prod.ProductInventories is not null && prod.ProductInventories.Count > 0)
        {
            foreach (var pi in prod.ProductInventories)
            {
                if (pi.Inventory is not null)
                {
                    // Add inventory transaction
                    await _inventoryTransactionRepository
                        .ProduceAsync(
                                productionNumber,
                                pi.Inventory,
                                pi.InventoryQuantity * quantity,
                                doneBy,
                                -1
                        );

                    // decrease the inventories
                    var inv = await _inventoryRepository.GetInventoryByIdAsync(pi.InventoryId);
                    inv.Quantity -= pi.InventoryQuantity * quantity;
                    await _inventoryRepository.UpdateInventoryAsync(inv);
                }
            }
        }

        // add product transaction
        db.ProductTransactions?.Add(new ProductTransaction
        {
            ProductionNumber = productionNumber,
            ProductId = product.Id,
            QuantityBefore = product.Quantity,
            ActivityType = ProductTransactionType.ProduceProduct,
            QuantityAfter = product.Quantity + quantity,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy
        });

        await db.SaveChangesAsync();
    }

    public async Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
    {
        using var db = _contextFactory.CreateDbContext();

        db.ProductTransactions?.Add(new ProductTransaction
        {
            ActivityType = ProductTransactionType.SellProduct,
            SONumber = salesOrderNumber,
            ProductId = product.Id,
            QuantityBefore = product.Quantity,
            QuantityAfter = product.Quantity - quantity,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy,
            UnitPrice = unitPrice
        });

        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
    {

        using var db = _contextFactory.CreateDbContext();

        var query = from pt in db.ProductTransactions
                    join prod in db.Products on pt.ProductId equals prod.Id
                    where (string.IsNullOrWhiteSpace(productName) || prod.Name.ToLower().IndexOf(productName.ToLower()) >= 0) &&
                        (!dateFrom.HasValue || pt.TransactionDate >= dateFrom.Value.Date) &&
                        (!dateTo.HasValue || pt.TransactionDate <= dateTo.Value.Date) &&
                        (!transactionType.HasValue || pt.ActivityType == transactionType)
                    select pt;

        return await query.Include(x => x.Product).ToListAsync();
    }
}

