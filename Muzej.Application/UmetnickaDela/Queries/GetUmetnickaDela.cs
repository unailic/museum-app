using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;

namespace Muzej.Application.UmetnickaDela.Queries.GetUmetnickaDela
{
    public class GetUmetnickaDelaQuery : IRequest<List<UmetnickoDelo>>
    {
    }
}
