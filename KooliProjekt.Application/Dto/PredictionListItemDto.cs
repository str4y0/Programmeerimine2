using System;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    public class PredictionListItemDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string TournamentName { get; set; }
        public MatchStage Stage { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }

        public int PredictedHomeGoals { get; set; }
        public int PredictedAwayGoals { get; set; }

        // null kuni mäng pole läbi
        public int? Points { get; set; }
        public DateTime PredictedAt { get; set; }
    }
}
