using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory;

public class ProductRepository : IProductRepository
{
    private List<Product> _products;

    public ProductRepository()
    {
        _products = new List<Product>
        {
            new Product { Id = 1, Name = "Bike", Quantity = 10, Price = 150 },
            new Product { Id = 2, Name = "Car", Quantity = 10, Price = 24000 }
        };
    }

    public Task AddProductAsync(Product product)
    {
        if (_products.Any(i => i.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return Task.CompletedTask;
        }

        var maxId = _products.Max(i => i.Id);
        product.Id = maxId + 1;

        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task DeleteProductByIdAsync(int id)
    {
        var product = _products.FirstOrDefault(i => i.Id == id);
        if (product is not null)
        {
            _products.Remove(product);
        }

        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return await Task.FromResult(_products.AsEnumerable());
        }

        return _products.Where(i => i.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        var prod = _products.FirstOrDefault(i => i.Id == productId);

        var newProd = new Product();
        if (prod is not null)
        {
            newProd.Id = prod.Id;
            newProd.Name = prod.Name;
            newProd.Price = prod.Price;
            newProd.Quantity = prod.Quantity;
            newProd.ProductInventories = new List<ProductInventory>();

            if (prod.ProductInventories is not null && prod.ProductInventories.Count > 0)
            {
                foreach (var prodInv in prod.ProductInventories)
                {
                    var newProdInv = new ProductInventory
                    {
                        InventoryId = prodInv.InventoryId,
                        ProductId = prodInv.ProductId,
                        Product = prod,
                        Inventory = new Inventory(),
                        InventoryQuantity = prodInv.InventoryQuantity
                    };
                    if (prodInv.Inventory != null)
                    {
                        newProdInv.Inventory.Id = prodInv.Inventory.Id;
                        newProdInv.Inventory.Name = prodInv.Inventory.Name;
                        newProdInv.Inventory.Price = prodInv.Inventory.Price;
                        newProdInv.Inventory.Quantity = prodInv.Inventory.Quantity;
                    }

                    newProd.ProductInventories.Add(newProdInv);
                }
            }
        }

        return await Task.FromResult(newProd);
    }

    public Task UpdateProductAsync(Product product)
    {
        if (_products.Any(i => i.Id != product.Id && i.
            Name.Equals(product.Name.ToLower())))
        {
            return Task.CompletedTask;
        }

        var prod = _products.FirstOrDefault(i => i.Id == product.Id);
        if (prod is not null)
        {
            prod.Name = product.Name;
            prod.Quantity = product.Quantity;
            prod.Price = product.Price;
            prod.ProductInventories = product.ProductInventories;
        }

        return Task.CompletedTask;
    }
}
