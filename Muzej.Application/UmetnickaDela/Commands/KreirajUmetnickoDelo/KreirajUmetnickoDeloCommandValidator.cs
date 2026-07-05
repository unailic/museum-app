using FluentValidation;
using Muzej.Domain.Entities;

namespace Muzej.Application.UmetnickaDela.Commands.KreirajUmetnickoDelo
{
    public class KreirajUmetnickoDeloCommandValidator : AbstractValidator<KreirajUmetnickoDeloCommand>
    {
        public KreirajUmetnickoDeloCommandValidator()
        {
            RuleFor(x => x.Naziv).NotEmpty().WithMessage("Naziv je obavezan.");
            RuleFor(x => x.GodinaNastanka)
                .LessThanOrEqualTo(DateTime.Now.Year)
                .WithMessage("Godina nastanka ne moze biti u budućnosti.");
            RuleFor(x => x.AutorId).GreaterThan(0).WithMessage("Mora se izabrati autor.");

            RuleFor(x => x.Materijal)
                .NotEmpty()
                .When(x => x.TipDela == TipUmetnickogDela.Skulptura)
                .WithMessage("Materijal je obavezan za skulpturu.");

            RuleFor(x => x.Tehnika)
                .NotEmpty()
                .When(x => x.TipDela == TipUmetnickogDela.Slika)
                .WithMessage("Tehnika je obavezna za sliku.");
        }
    }
}