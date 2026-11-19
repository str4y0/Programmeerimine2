using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Teams
{
    public class ListFilesQueryValidator : AbstractValidator<ListFilesQuery>
    {
        public ListFilesQueryValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.TeamId)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(1).WithMessage("TeamId must be greater than or equal to 1.")
                .MustAsync(async (id, cancellationToken) =>
                    await dbContext.Teams.AnyAsync(t => t.Id == id, cancellationToken))
                .WithMessage(x => "Cannot find team with ID " + x.TeamId);
        }
    }
}
