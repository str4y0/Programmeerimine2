using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, OperationResult<PagedResult<UserListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListUsersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<UserListItemDto>>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<UserListItemDto>>();

            // NB! Teadlikult ei võeta kaasa PasswordHash'i ega e-posti
            result.Value = await _dbContext.Users
                .AsNoTracking()
                .OrderBy(user => user.Name)
                .ThenBy(user => user.Id)
                .Select(user => new UserListItemDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    TotalPoints = _dbContext.Predictions
                        .Where(prediction => prediction.UserId == user.Id)
                        .Sum(prediction => prediction.Points ?? 0)
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
