﻿using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory;
public class ProductTransactionRepository : IProductTransactionRepository
{
    private List<ProductTransaction> _productTransactions = new List<ProductTransaction>();
    private readonly IProductRepository productRepository;
    private readonly IInventoryTransactionRepository inventoryTransactionRepository;
    private readonly IInventoryRepository inventoryRepository;
    public ProductTransactionRepository(IProductRepository productRepository,
        IInventoryTransactionRepository inventoryTransactionRepository,
        IInventoryRepository inventoryRepository)
    {
        this.productRepository = productRepository;
        this.inventoryTransactionRepository = inventoryTransactionRepository;
        this.inventoryRepository = inventoryRepository;
    }

    public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
    {

        var prod = await productRepository.GetProductByIdAsync(product.Id);
        if (prod is not null && prod.ProductInventories is not null && prod.ProductInventories.Count > 0)
        {
            foreach (var pi in prod.ProductInventories)
            {
                if (pi.Inventory is not null)
                {
                    // Add inventory transaction
                    await inventoryTransactionRepository
                        .ProduceAsync(
                                productionNumber,
                                pi.Inventory,
                                pi.InventoryQuantity * quantity,
                                doneBy,
                                -1
                        );

                    // decrease the inventories
                    var inv = await inventoryRepository.GetInventoryByIdAsync(pi.InventoryId);
                    inv.Quantity -= pi.InventoryQuantity * quantity;
                    await inventoryRepository.UpdateInventoryAsync(inv);
                }
            }
        }

        // add product transaction
        _productTransactions.Add(new ProductTransaction
        {
            ProductionNumber = productionNumber,
            ProductId = product.Id,
            QuantityBefore = product.Quantity,
            ActivityType = ProductTransactionType.ProduceProduct,
            QuantityAfter = product.Quantity + quantity,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy
        });
    }

    public Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
    {
        _productTransactions.Add(new ProductTransaction
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

        return Task.CompletedTask;
    }

    public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
    {
        var products = (await productRepository.GetProductsByNameAsync(string.Empty)).ToList();

        var query = from it in _productTransactions
                    join inv in products on it.ProductId equals inv.Id
                    where (string.IsNullOrWhiteSpace(productName) || inv.Name.ToLower().IndexOf(productName.ToLower()) >= 0) &&
                        (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date) &&
                        (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date) &&
                        (!transactionType.HasValue || it.ActivityType == transactionType)
                    select new ProductTransaction
                    {
                        Product = inv,
                        ProductTransactionId = it.ProductTransactionId,
                        SONumber = it.SONumber,
                        ProductionNumber = it.ProductionNumber,
                        ProductId = it.ProductId,
                        QuantityBefore = it.QuantityBefore,
                        ActivityType = it.ActivityType,
                        QuantityAfter = it.QuantityAfter,
                        TransactionDate = it.TransactionDate,
                        DoneBy = it.DoneBy,
                        UnitPrice = it.UnitPrice
                    };

        return query;
    }
}

