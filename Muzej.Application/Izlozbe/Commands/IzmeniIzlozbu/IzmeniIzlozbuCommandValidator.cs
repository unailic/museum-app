using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;

namespace Muzej.Application.Izlozbe.Commands.IzmeniIzlozbu
{
    public class IzmeniIzlozbuCommandValidator : AbstractValidator<IzmeniIzlozbuCommand>
    {
        public IzmeniIzlozbuCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Naziv).NotEmpty();
            RuleFor(x => x.Cena).GreaterThanOrEqualTo(0);
            RuleFor(x => x.DatumZavrsetka).GreaterThan(x => x.DatumPocetka);
        }
    }
}
