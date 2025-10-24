using FluentValidation;

namespace Martins.Backend.Domain.Commands.Sale.Create
{
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().When(x => x.CustomerId != null)
                .WithMessage("O ID do cliente não pode ser vazio.");

            RuleFor(x => x.OrderStatus)
                .IsInEnum().WithMessage("O status do pedido é inválido.");

            RuleFor(x => x.OrderItems)
                .NotNull().WithMessage("A lista de itens do pedido é obrigatória.")
                .Must(x => x.Any()).WithMessage("A venda deve conter pelo menos um item.");

            RuleForEach(x => x.OrderItems)
                .ChildRules(item =>
                {
                    item.RuleFor(i => i.ProductId)
                        .NotEmpty().WithMessage("O ID do produto é obrigatório.");

                    item.RuleFor(i => i.Quantity)
                        .GreaterThan(0).WithMessage("A quantidade do item deve ser maior que zero.");

                    item.RuleFor(i => i.UnitPrice)
                        .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero.");
                });

            RuleFor(x => x.Observations)
                .MaximumLength(500).WithMessage("As observações não podem ter mais que 500 caracteres.");
        }
    }
}
