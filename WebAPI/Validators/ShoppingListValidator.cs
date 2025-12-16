using FluentValidation;
using Models;
using Services.Interfaces;

namespace WebAPI.Validators
{
    public class ShoppingListValidator : AbstractValidator<ShoppingList>
    {
        public ShoppingListValidator(IGenericService<ShoppingList> service)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100).WithMessage("ala ma kota")
                .Must(x => x.Contains("Zakupy")).When(x => x.CreatedAt < DateTime.Now.Date.AddYears(-1)).WithMessage("Nazwa musi zawierać słowo \"Zakupy\"");

            RuleFor(x => x.CreatedAt)
                .LessThanOrEqualTo(DateTime.Now)
                .WithName("Data utworzenia");

            RuleFor(x => x.Name)
                .MustAsync(async (shoppingList, name, cancelationToken) =>
                {
                    var existingItems = await service.ReadAsync();
                    return !existingItems.Any(x => x.Name == name);
                })
                .WithMessage("Lista zakupów o takiej nazwie już istnieje.");
        }
    }
}
