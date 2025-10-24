using FluentValidation;

namespace Martins.Backend.Domain.Commands.Material.AddStock
{
    public class AddStockCommandValidator : AbstractValidator<AddStockCommand>
    {
        public AddStockCommandValidator()
        {
            RuleFor(x => x.MaterialId)
                .NotEmpty().WithMessage("O ID do material é obrigatório.");

            RuleFor(x => x.QuantityToAdd)
                .GreaterThan(0).WithMessage("A quantidade adicionada deve ser maior que zero.");

            RuleFor(x => x.totalCost)
                .GreaterThanOrEqualTo(0).WithMessage("O custo total não pode ser negativo.");

            RuleFor(x => x.Supplier)
                .MaximumLength(100).WithMessage("O nome do fornecedor não pode ter mais que 100 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Supplier));
        }
    }
}
