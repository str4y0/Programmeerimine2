using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class GetUserQuery : IRequest<User>
    {
        public int Id { get; set; }
    }
}
