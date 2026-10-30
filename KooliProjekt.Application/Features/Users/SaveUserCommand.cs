using KooliProjekt.Application.Data;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Parooli hoiame andmebaasis ainult räsina (PasswordHash)
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
