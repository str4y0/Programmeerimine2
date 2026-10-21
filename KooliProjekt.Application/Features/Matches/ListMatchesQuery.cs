using System.Collections.Generic;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Matches
{
    public class ListMatchesQuery : IRequest<IEnumerable<Match>>
    {
    }
}
