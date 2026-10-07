using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KooliProjekt.Application.Data
{
    public class Match
    {
        public int Id { get; set; }

        public MatchStage Stage { get; set; }

        public DateTime StartTime { get; set; }

        [Range(0, int.MaxValue)]
        public int? HomeTeamGoals { get; set; }

        [Range(0, int.MaxValue)]
        public int? AwayTeamGoals { get; set; }

        public Tournament Tournament { get; set; }
        public int TournamentId { get; set; }

        // Võistkond võib olla veel teadmata, seega võõrvõti on int?
        public Team HomeTeam { get; set; }
        public int? HomeTeamId { get; set; }

        public Team AwayTeam { get; set; }
        public int? AwayTeamId { get; set; }
    }
}
