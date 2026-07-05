using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;

namespace Muzej.Application.Ulaznice.Commands.KupiUlaznice
{
    public class KupiUlazniceCommandValidator : AbstractValidator<KupiUlazniceCommand>
    {
        public KupiUlazniceCommandValidator()
        {
            RuleFor(x => x.PosetilacId).NotEmpty().WithMessage("PosetilacId je obavezan.");
            RuleFor(x => x.IzlozbaId).GreaterThan(0).WithMessage("Mora se izabrati izlozba.");
            RuleFor(x => x.BrojKarata).GreaterThan(0).WithMessage("Broj karata mora biti veci od nule.");
        }
    }
}
