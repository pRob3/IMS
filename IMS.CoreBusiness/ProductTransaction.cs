using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness;

public class ProductTransaction
{
    public int ProductTransactionId { get; set; }

    // Sales Order Number
    public string SONumber { get; set; } = string.Empty;
    public string ProductionNumber { get; set; } = string.Empty;

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int QuantityBefore { get; set; }

    [Required]
    public ProductTransactionType ActivityType { get; set; }

    [Required]
    public int QuantityAfter { get; set; }

    // Selling price
    public double? UnitPrice { get; set; }

    [Required]
    public DateTime TransactionDate { get; set; }

    [Required]
    public string DoneBy { get; set; } = string.Empty;

    public Product? Product { get; set; }
}

