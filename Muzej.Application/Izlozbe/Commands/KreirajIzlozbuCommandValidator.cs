using FluentValidation;

namespace Muzej.Application.Izlozbe.Commands.KreirajIzlozbu
{
    public class KreirajIzlozbuCommandValidator : AbstractValidator<KreirajIzlozbuCommand>
    {
        public KreirajIzlozbuCommandValidator()
        {
            RuleFor(x => x.Naziv).NotEmpty().WithMessage("Naziv je obavezan.");
            RuleFor(x => x.Cena).GreaterThanOrEqualTo(0).WithMessage("Cena mora biti pozitivna.");
            RuleFor(x => x.Kapacitet).GreaterThan(0).WithMessage("Kapacitet mora biti veci od nule.");
            RuleFor(x => x.DatumZavrsetka)
                .GreaterThan(x => x.DatumPocetka)
                .WithMessage("Datum zavrsetka mora biti posle datuma pocetka.");
        }
    }
}