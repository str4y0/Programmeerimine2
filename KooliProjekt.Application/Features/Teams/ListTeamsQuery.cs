using System.Collections.Generic;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Teams
{
    public class ListTeamsQuery : IRequest<IEnumerable<Team>>
    {
    }
}
