using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;

namespace Muzej.Application.Ulaznice.Queries.GetMojeUlaznice
{
    public class GetMojeUlazniceQuery : IRequest<List<Ulaznica>>
    {
        public string PosetilacId { get; set; }
    }
}
