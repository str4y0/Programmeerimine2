using System;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    // Kasutaja detailvaates kuvatav ennustus (kasutaja nime pole vaja korrata)
    public class UserPredictionDto
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public string TournamentName { get; set; }
        public MatchStage Stage { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public int PredictedHomeGoals { get; set; }
        public int PredictedAwayGoals { get; set; }
        public int? Points { get; set; }
        public DateTime PredictedAt { get; set; }
    }
}
