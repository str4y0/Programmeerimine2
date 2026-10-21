using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Teams
{
    public class GetTeamQuery : IRequest<Team>
    {
        public int Id { get; set; }
    }
}
