using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, IEnumerable<User>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListUsersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Users.ToListAsync(cancellationToken);

            return result;
        }
    }
}
