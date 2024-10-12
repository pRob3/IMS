using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelsValidations;

public class Sell_EnsureEnoughProductQuantity : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var model = (ViewModels.SellViewModel)validationContext.ObjectInstance;

        if (model is not null)
        {
            if (model.Product is not null)
            {
                if (model.Product.Quantity < model.QuantityToSell)
                {
                    return new ValidationResult($"There isn't enough product. There is only {model.Product.Quantity} in the inventory.", new[] { validationContext.MemberName });
                }
            }
        }

        return ValidationResult.Success;
    }
}
