using MediatR;
using Muzej.Application.Izlozbe.Dtos;
using Muzej.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Application.Izlozbe.Queries.GetIzlozbe
{
    public class GetIzlozbeQuery : IRequest<List<IzlozbaDto>>
    {
    }
}
