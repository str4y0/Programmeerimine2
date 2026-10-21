using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Matches
{
    public class GetMatchQuery : IRequest<Match>
    {
        public int Id { get; set; }
    }
}
