using System;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    public class PredictionDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public int MatchId { get; set; }
        public string TournamentName { get; set; }
        public MatchStage Stage { get; set; }
        public DateTime MatchStartTime { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }

        public int PredictedHomeGoals { get; set; }
        public int PredictedAwayGoals { get; set; }
        public DateTime PredictedAt { get; set; }

        // Tegelik tulemus ja punktid on teada alles pärast mängu (null)
        public int? ActualHomeGoals { get; set; }
        public int? ActualAwayGoals { get; set; }
        public int? Points { get; set; }
    }
}
