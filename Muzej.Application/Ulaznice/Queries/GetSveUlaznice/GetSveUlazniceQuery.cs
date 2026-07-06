using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Application.Ulaznice.Dtos;

namespace Muzej.Application.Ulaznice.Queries.GetSveUlaznice
{
    public class GetSveUlazniceQuery : IRequest<List<AdminUlaznicaDto>>
    {
    }
}
