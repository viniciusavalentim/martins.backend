using FluentValidation;
using Martins.Backend.Domain.Commands.Product.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martins.Backend.Domain.Commands.Product.Update
{
    internal class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                .MaximumLength(100).WithMessage("O nome não pode ter mais que 100 caracteres.");

            RuleFor(x => x.ProfitMarginPorcent)
                .InclusiveBetween(0, 100).WithMessage("A margem de lucro deve estar entre 0% e 100%.");

            RuleFor(x => x.BillOfMaterials)
                .NotNull().WithMessage("A lista de materiais é obrigatória.")
                .Must(x => x.Any()).WithMessage("O produto deve ter pelo menos um material na lista.");

            RuleForEach(x => x.BillOfMaterials)
                .ChildRules(material =>
                {
                    material.RuleFor(m => m.QuantityUsed)
                        .GreaterThan(0).WithMessage("A quantidade usada deve ser maior que zero.");
                });
        }
    }
}
