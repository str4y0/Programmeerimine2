using System;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Matches
{
    public class SaveMatchCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public MatchStage Stage { get; set; }
        public DateTime StartTime { get; set; }

        // Võistkond võib olla veel teadmata (null)
        public int? HomeTeamId { get; set; }
        public int? AwayTeamId { get; set; }

        // Tulemus on teada alles pärast mängu (null)
        public int? HomeTeamGoals { get; set; }
        public int? AwayTeamGoals { get; set; }
    }
}
