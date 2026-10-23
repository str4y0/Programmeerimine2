using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class GetTournamentQuery : IRequest<OperationResult<TournamentDto>>
    {
        public int Id { get; set; }
    }
}
