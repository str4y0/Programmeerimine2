using System;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    // Turniiri detailvaates kuvatav mäng (turniiri nime pole vaja korrata)
    public class TournamentMatchDto
    {
        public int Id { get; set; }
        public MatchStage Stage { get; set; }
        public DateTime StartTime { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public int? HomeTeamGoals { get; set; }
        public int? AwayTeamGoals { get; set; }
    }
}
