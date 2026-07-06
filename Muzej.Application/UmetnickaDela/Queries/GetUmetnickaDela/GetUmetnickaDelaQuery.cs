using MediatR;
using Muzej.Application.UmetnickaDela.Dtos;
using Muzej.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Application.UmetnickaDela.Queries.GetUmetnickaDela
{
    public class GetUmetnickaDelaQuery : IRequest<List<UmetnickoDeloDto>>
    {
    }
}
