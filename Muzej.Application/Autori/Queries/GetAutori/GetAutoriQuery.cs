using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;

namespace Muzej.Application.Autori.Queries.GetAutori
{
    public class GetAutoriQuery : IRequest<List<Autor>>
    {
    }
}
