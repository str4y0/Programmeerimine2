using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Teams
{
    public class SaveTeamCommandValidator : AbstractValidator<SaveTeamCommand>
    {
        public SaveTeamCommandValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");

            // Võistkonna nimi on kohustuslik, kuni 100 märki ja unikaalne
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
                .MustAsync(async (command, name, cancellationToken) =>
                    !await dbContext.Teams.AnyAsync(t => t.Name == name && t.Id != command.Id, cancellationToken))
                .WithMessage("A team with the same name already exists.");
        }
    }
}
