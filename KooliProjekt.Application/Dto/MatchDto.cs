using System;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    public class MatchDto
    {
        public int Id { get; set; }

        public int TournamentId { get; set; }
        public string TournamentName { get; set; }

        public MatchStage Stage { get; set; }
        public DateTime StartTime { get; set; }

        public int? HomeTeamId { get; set; }
        public string HomeTeamName { get; set; }
        public int? AwayTeamId { get; set; }
        public string AwayTeamName { get; set; }

        public int? HomeTeamGoals { get; set; }
        public int? AwayTeamGoals { get; set; }

        // Mitu kasutajat on sellele mängule ennustuse teinud
        public int PredictionCount { get; set; }
    }
}
