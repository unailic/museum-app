using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;

namespace Muzej.Application.Autori.Commands.KreirajAutora
{
    public class KreirajAutoraCommandValidator : AbstractValidator<KreirajAutoraCommand>
    {
        public KreirajAutoraCommandValidator()
        {
            RuleFor(x => x.Ime)
                .NotEmpty().WithMessage("Ime je obavezno.")
                .MaximumLength(50);

            RuleFor(x => x.Prezime)
                .NotEmpty().WithMessage("Prezime je obavezno.")
                .MaximumLength(50);

            RuleFor(x => x.GodinaRodjenja)
                .LessThanOrEqualTo(DateTime.Now.Year)
                .WithMessage("Godina rodjenja ne moze biti u buducnosti.");
        }
    }
}