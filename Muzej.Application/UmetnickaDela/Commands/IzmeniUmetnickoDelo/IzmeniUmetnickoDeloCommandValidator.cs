using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;

namespace Muzej.Application.UmetnickaDela.Commands.IzmeniUmetnickoDelo
{
    public class IzmeniUmetnickoDeloCommandValidator : AbstractValidator<IzmeniUmetnickoDeloCommand>
    {
        public IzmeniUmetnickoDeloCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Naziv).NotEmpty();
            RuleFor(x => x.GodinaNastanka).LessThanOrEqualTo(DateTime.Now.Year);
            RuleFor(x => x.AutorId).GreaterThan(0);
        }
    }
}