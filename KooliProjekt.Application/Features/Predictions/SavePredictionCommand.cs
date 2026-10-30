using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Predictions
{
    public class SavePredictionCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MatchId { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
    }
}
