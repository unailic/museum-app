using MediatR;
using Muzej.Application.Ulaznice.Dtos;
using Muzej.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Application.Ulaznice.Queries.GetMojeUlaznice
{
    public class GetMojeUlazniceQuery : IRequest<List<UlaznicaDto>>
    {
        public string PosetilacId { get; set; }
    }
}
