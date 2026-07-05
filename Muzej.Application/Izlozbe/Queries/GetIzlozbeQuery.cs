using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;

namespace Muzej.Application.Izlozbe.Queries.GetIzlozbe
{
    public class GetIzlozbeQuery : IRequest<List<Izlozba>>
    {
    }
}
