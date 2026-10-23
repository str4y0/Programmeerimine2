namespace KooliProjekt.Application.Dto
{
    // NB! Ei sisalda PasswordHash'i ega e-posti
    public class UserListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalPoints { get; set; }
    }
}
