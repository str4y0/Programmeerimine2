using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class TournamentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<TournamentMatchDto> Matches { get; set; }

        public TournamentDto()
        {
            Matches = new List<TournamentMatchDto>();
        }
    }
}
