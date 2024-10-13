using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer;
public class ProductEFCoreRepository : IProductRepository
{
    private readonly IDbContextFactory<IMSContext> _contextFactory;

    public ProductEFCoreRepository(IDbContextFactory<IMSContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task AddProductAsync(Product obj)
    {
        using var db = _contextFactory.CreateDbContext();

        db.Products?.Add(obj);
        FlagInventoryUnchanged(obj, db);

        await db.SaveChangesAsync();
    }

    public async Task DeleteProductByIdAsync(int id)
    {
        using var db = _contextFactory.CreateDbContext();
        var product = await db.Products.Include(x => x.ProductInventories)
            .ThenInclude(x => x.Inventory)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
        {
            return;
        }

        db.Products?.Remove(product);
        await db.SaveChangesAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        using var db = _contextFactory.CreateDbContext();

        var product = await db.Products
            .Include(x => x.ProductInventories)
            .ThenInclude(x => x.Inventory)
            .FirstOrDefaultAsync(x => x.Id == productId);

        if (product is not null)
        {
            return product;
        }

        return new Product();
    }

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        using var db = _contextFactory.CreateDbContext();
        return await db.Products.Where(x => x.Name.ToLower().IndexOf(name.ToLower()) >= 0)?.ToListAsync();
    }

    public async Task UpdateProductAsync(Product obj)
    {
        using var db = _contextFactory.CreateDbContext();

        var prod = await db.Products
            .Include(x => x.ProductInventories)
            .FirstOrDefaultAsync(x => x.Id == obj.Id);

        if (prod is not null)
        {
            prod.Name = obj.Name;
            prod.Price = obj.Price;
            prod.Quantity = obj.Quantity;
            prod.ProductInventories = obj.ProductInventories;
            FlagInventoryUnchanged(obj, db);

            await db.SaveChangesAsync();
        }
    }

    private void FlagInventoryUnchanged(Product product, IMSContext db)
    {
        if (product?.ProductInventories is not null && product.ProductInventories.Count > 0)
        {
            foreach (var productInventory in product.ProductInventories)
            {
                if (productInventory.Inventory is not null)
                {
                    db.Entry(productInventory.Inventory).State = EntityState.Unchanged;
                }
            }
        }
    }
}

