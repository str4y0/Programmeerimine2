using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class GetTournamentQuery : IRequest<Tournament>
    {
        public int Id { get; set; }
    }
}
