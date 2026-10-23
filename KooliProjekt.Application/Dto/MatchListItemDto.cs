using System;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    public class MatchListItemDto
    {
        public int Id { get; set; }
        public string TournamentName { get; set; }
        public MatchStage Stage { get; set; }
        public DateTime StartTime { get; set; }

        // Võistkond võib olla veel teadmata (null)
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }

        // Tulemus on teada alles pärast mängu (null)
        public int? HomeTeamGoals { get; set; }
        public int? AwayTeamGoals { get; set; }
    }
}
