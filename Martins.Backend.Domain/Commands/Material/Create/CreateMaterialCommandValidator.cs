using FluentValidation;

namespace Martins.Backend.Domain.Commands.Material.Create
{
    public class CreateMaterialCommandValidator : AbstractValidator<CreateMaterialCommand>
    {
        public CreateMaterialCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do material é obrigatório.")
                .MaximumLength(100).WithMessage("O nome não pode ter mais que 100 caracteres.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("A categoria é obrigatória.")
                .MaximumLength(50).WithMessage("A categoria não pode ter mais que 50 caracteres.");

            RuleFor(x => x.CurrentStock)
                .GreaterThanOrEqualTo(0).WithMessage("O estoque atual não pode ser negativo.");

            RuleFor(x => x.UnitOfMeasure)
                .IsInEnum().WithMessage("A unidade de medida informada é inválida.");

            RuleFor(x => x.TotalCost)
                .GreaterThanOrEqualTo(0).WithMessage("O custo total não pode ser negativo.");

            RuleFor(x => x.Supplier)
                .MaximumLength(100).WithMessage("O nome do fornecedor não pode ter mais que 100 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Supplier));
        }
    }
}
