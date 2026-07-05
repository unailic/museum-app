using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;

namespace Muzej.Application.Ulaznice.Commands.KupiUlaznice
{
    public class KupiUlazniceCommand : IRequest<List<int>>
    {
        public string PosetilacId { get; set; }
        public TipPosetioca TipPosetioca { get; set; }
        public int IzlozbaId { get; set; }
        public int BrojKarata { get; set; }
    }
}
