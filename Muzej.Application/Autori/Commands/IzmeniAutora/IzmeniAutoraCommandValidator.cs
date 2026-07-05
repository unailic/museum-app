using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;

namespace Muzej.Application.Autori.Commands.IzmeniAutora
{
    public class IzmeniAutoraCommandValidator : AbstractValidator<IzmeniAutoraCommand>
    {
        public IzmeniAutoraCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Ime).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Prezime).NotEmpty().MaximumLength(50);
            RuleFor(x => x.GodinaRodjenja).LessThanOrEqualTo(DateTime.Now.Year);
        }
    }
}
