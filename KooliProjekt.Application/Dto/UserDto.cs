using System.Collections.Generic;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    // NB! Ei sisalda PasswordHash'i ega e-posti
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
        public int TotalPoints { get; set; }
        public IList<UserPredictionDto> Predictions { get; set; }

        public UserDto()
        {
            Predictions = new List<UserPredictionDto>();
        }
    }
}
