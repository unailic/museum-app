using MediatR;
using Muzej.Application.Izlozbe.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Application.Izlozbe.Queries.GetIzlozbaById
{
    public class GetIzlozbaByIdQuery : IRequest<IzlozbaDetaljiDto?>
    {
        public int Id { get; set; }
    }
}