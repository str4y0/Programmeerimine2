using System.Collections.Generic;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class ListTournamentsQuery : IRequest<IEnumerable<Tournament>>
    {
    }
}
