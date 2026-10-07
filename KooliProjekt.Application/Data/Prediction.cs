using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    [Index(nameof(UserId), nameof(MatchId), IsUnique = true)]
    public class Prediction
    {
        public int Id { get; set; }

        [Range(0, int.MaxValue)]
        public int HomeTeamGoals { get; set; }

        [Range(0, int.MaxValue)]
        public int AwayTeamGoals { get; set; }

        public DateTime PredictedAt { get; set; }

        public int? Points { get; set; }

        public Match Match { get; set; }
        public int MatchId { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}
