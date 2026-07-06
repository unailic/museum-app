using MediatR;
using Muzej.Application.Autori.Dtos;
using Muzej.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Application.Autori.Queries.GetAutori
{
    public class GetAutoriQuery : IRequest<List<AutorDto>>
    {
    }
}
