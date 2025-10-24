using FluentValidation;

namespace Martins.Backend.Domain.Commands.Product.Produce
{
    public class ProduceProductCommandValidator : AbstractValidator<ProduceProductCommand>
    {
        public ProduceProductCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("O ID do produto é obrigatório.");

            RuleFor(x => x.QuantityToProduce)
                .GreaterThan(0).WithMessage("A quantidade a produzir deve ser maior que zero.");
        }
    }
}
