using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Teams
{
    public class GetTeamQuery : IRequest<OperationResult<TeamDto>>
    {
        public int Id { get; set; }
    }
}
