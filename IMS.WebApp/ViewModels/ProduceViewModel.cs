using IMS.CoreBusiness;
using IMS.WebApp.ViewModelsValidations;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels;

public class ProduceViewModel
{
    [Required]
    public string ProductionNumber { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "You have to select an inventory.")]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity need to be greater or equal to 1.")]
    [Produce_EnsureEnoughInventoryQuantity]
    public int QuantityToProduce { get; set; }

    public Product? Product { get; set; } = null;
}
