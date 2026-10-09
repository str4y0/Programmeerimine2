using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KooliProjekt.Application.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            SeedUsers(context);
            SeedTournaments(context);
            SeedTeams(context);
            SeedMatches(context);
            SeedPredictions(context);
        }

        private static void SeedUsers(ApplicationDbContext context)
        {
            var names = new[]
            {
                "Mart Tamm", "Liis Saar", "Jaan Kask", "Kati Mägi", "Andres Sepp",
                "Maria Kukk", "Peeter Rebane", "Anna Pärn", "Tõnu Kivi", "Helen Oja",
                "Rein Lepik", "Marika Koppel", "Priit Raud", "Triin Luik", "Karl Põld",
                "Elina Vaher", "Siim Kuusk", "Merili Rätsep", "Ott Pärtel", "Laura Mets",
                "Hannes Teder", "Kertu Laur", "Madis Org", "Piret Sild", "Raivo Tamme",
                "Evelin Kallas", "Indrek Aas", "Annika Hunt", "Urmas Lill", "Karin Jõgi",
                "Henri Vares", "Getter Puusepp", "Oskar Rand", "Kerli Salu", "Taavi Kangur"
            };

            var users = new List<User>();
            for (var i = 0; i < names.Length; i++)
            {
                var email = ToEmail(names[i]);
                users.Add(new User
                {
                    Name = names[i],
                    Email = email,
                    PasswordHash = HashPassword("Parool" + (i + 1) + "!"),
                    Role = i == 0 ? UserRole.Administrator : UserRole.Participant
                });
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private static void SeedTournaments(ApplicationDbContext context)
        {
            var tournaments = GetTournamentInfo()
                .Select(t => new Tournament { Name = t.Name })
                .ToList();

            context.Tournaments.AddRange(tournaments);
            context.SaveChanges();
        }

        private static void SeedTeams(ApplicationDbContext context)
        {
            var names = new[]
            {
                "Brazil", "Germany", "Argentina", "France", "Italy",
                "Spain", "England", "Netherlands", "Portugal", "Belgium",
                "Croatia", "Uruguay", "Mexico", "United States", "Sweden",
                "Denmark", "Switzerland", "Poland", "Czech Republic", "Russia",
                "Serbia", "Colombia", "Chile", "Japan", "South Korea",
                "Morocco", "Senegal", "Nigeria", "Cameroon", "Ghana",
                "Australia", "Austria", "Hungary", "Turkey", "Scotland"
            };

            var teams = names.Select(n => new Team { Name = n }).ToList();

            context.Teams.AddRange(teams);
            context.SaveChanges();
        }

        private static void SeedMatches(ApplicationDbContext context)
        {
            var random = new Random(2026);
            var info = GetTournamentInfo();
            var tournaments = context.Tournaments.OrderBy(t => t.Id).ToList();
            var teamIds = context.Teams.OrderBy(t => t.Id).Select(t => t.Id).ToList();

            var matches = new List<Match>();
            for (var i = 0; i < tournaments.Count; i++)
            {
                var tournament = tournaments[i];
                var finalDate = info[i].FinalDate;
                var played = finalDate < new DateTime(2026, 10, 1);

                // Poolfinaal ja finaal. Tulevikus toimuval turniiril pole võistkonnad ega tulemus veel teada.
                foreach (var stage in new[] { MatchStage.SemiFinal, MatchStage.Final })
                {
                    var match = new Match
                    {
                        TournamentId = tournament.Id,
                        Stage = stage,
                        StartTime = stage == MatchStage.Final ? finalDate : finalDate.AddDays(-4)
                    };

                    if (played)
                    {
                        var homeIndex = random.Next(teamIds.Count);
                        var awayIndex = random.Next(teamIds.Count - 1);
                        if (awayIndex >= homeIndex)
                        {
                            awayIndex++;
                        }

                        match.HomeTeamId = teamIds[homeIndex];
                        match.AwayTeamId = teamIds[awayIndex];
                        match.HomeTeamGoals = RandomGoals(random);
                        match.AwayTeamGoals = RandomGoals(random);
                    }

                    matches.Add(match);
                }
            }

            context.Matches.AddRange(matches);
            context.SaveChanges();
        }

        private static void SeedPredictions(ApplicationDbContext context)
        {
            var random = new Random(2027);
            var users = context.Users.OrderBy(u => u.Id).ToList();

            // Ennustused tehakse ainult mängudele, mille võistkonnad on teada ja mis on 2000. aastast või hiljem
            var matches = context.Matches
                .Where(m => m.HomeTeamId != null && m.AwayTeamId != null && m.StartTime.Year >= 2000)
                .OrderBy(m => m.Id)
                .ToList();

            var predictions = new List<Prediction>();
            for (var u = 0; u < users.Count; u++)
            {
                // Iga kasutaja ennustab kolme erinevat mängu (kasutaja + mäng on unikaalne)
                for (var k = 0; k < 3; k++)
                {
                    var match = matches[(u * 2 + k) % matches.Count];
                    var prediction = new Prediction
                    {
                        UserId = users[u].Id,
                        MatchId = match.Id,
                        HomeTeamGoals = RandomGoals(random),
                        AwayTeamGoals = RandomGoals(random),
                        PredictedAt = match.StartTime.AddHours(-random.Next(1, 72))
                    };

                    if (match.HomeTeamGoals.HasValue && match.AwayTeamGoals.HasValue)
                    {
                        prediction.Points = CalculatePoints(prediction, match);
                    }

                    predictions.Add(prediction);
                }
            }

            context.Predictions.AddRange(predictions);
            context.SaveChanges();
        }

        // 3 punkti täpse skoori eest, 1 punkt õige tulemuse (võit/viik) eest, muidu 0
        private static int CalculatePoints(Prediction prediction, Match match)
        {
            if (prediction.HomeTeamGoals == match.HomeTeamGoals && prediction.AwayTeamGoals == match.AwayTeamGoals)
            {
                return 3;
            }

            var predicted = Math.Sign(prediction.HomeTeamGoals - prediction.AwayTeamGoals);
            var actual = Math.Sign(match.HomeTeamGoals.Value - match.AwayTeamGoals.Value);

            return predicted == actual ? 1 : 0;
        }

        private static int RandomGoals(Random random)
        {
            // Väiksemad skoorid on tõenäolisemad
            var value = random.Next(100);
            if (value < 30) return 0;
            if (value < 60) return 1;
            if (value < 82) return 2;
            if (value < 93) return 3;
            return 4;
        }

        private static string ToEmail(string name)
        {
            var text = name.ToLowerInvariant()
                .Replace('ä', 'a').Replace('ö', 'o').Replace('õ', 'o').Replace('ü', 'u')
                .Replace(' ', '.');

            return text + "@example.com";
        }

        private static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }

        private static List<(string Name, DateTime FinalDate)> GetTournamentInfo()
        {
            return new List<(string, DateTime)>
            {
                ("FIFA World Cup 1930", new DateTime(1930, 7, 30)),
                ("FIFA World Cup 1934", new DateTime(1934, 6, 10)),
                ("FIFA World Cup 1938", new DateTime(1938, 6, 19)),
                ("FIFA World Cup 1950", new DateTime(1950, 7, 16)),
                ("FIFA World Cup 1954", new DateTime(1954, 7, 4)),
                ("FIFA World Cup 1958", new DateTime(1958, 6, 29)),
                ("FIFA World Cup 1962", new DateTime(1962, 6, 17)),
                ("FIFA World Cup 1966", new DateTime(1966, 7, 30)),
                ("FIFA World Cup 1970", new DateTime(1970, 6, 21)),
                ("FIFA World Cup 1974", new DateTime(1974, 7, 7)),
                ("FIFA World Cup 1978", new DateTime(1978, 6, 25)),
                ("FIFA World Cup 1982", new DateTime(1982, 7, 11)),
                ("FIFA World Cup 1986", new DateTime(1986, 6, 29)),
                ("FIFA World Cup 1990", new DateTime(1990, 7, 8)),
                ("FIFA World Cup 1994", new DateTime(1994, 7, 17)),
                ("FIFA World Cup 1998", new DateTime(1998, 7, 12)),
                ("FIFA World Cup 2002", new DateTime(2002, 6, 30)),
                ("FIFA World Cup 2006", new DateTime(2006, 7, 9)),
                ("FIFA World Cup 2010", new DateTime(2010, 7, 11)),
                ("FIFA World Cup 2014", new DateTime(2014, 7, 13)),
                ("FIFA World Cup 2018", new DateTime(2018, 7, 15)),
                ("FIFA World Cup 2022", new DateTime(2022, 12, 18)),
                ("UEFA Euro 1980", new DateTime(1980, 6, 22)),
                ("UEFA Euro 1984", new DateTime(1984, 6, 27)),
                ("UEFA Euro 1988", new DateTime(1988, 6, 25)),
                ("UEFA Euro 1992", new DateTime(1992, 6, 26)),
                ("UEFA Euro 1996", new DateTime(1996, 6, 30)),
                ("UEFA Euro 2000", new DateTime(2000, 7, 2)),
                ("UEFA Euro 2004", new DateTime(2004, 7, 4)),
                ("UEFA Euro 2008", new DateTime(2008, 6, 29)),
                ("UEFA Euro 2012", new DateTime(2012, 7, 1)),
                ("UEFA Euro 2016", new DateTime(2016, 7, 10)),
                ("UEFA Euro 2020", new DateTime(2021, 7, 11)),
                ("UEFA Euro 2024", new DateTime(2024, 7, 14)),
                ("UEFA Euro 2028", new DateTime(2028, 7, 9))
            };
        }
    }
}
