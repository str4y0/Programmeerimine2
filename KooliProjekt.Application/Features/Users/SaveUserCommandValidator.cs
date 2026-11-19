using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommandValidator : AbstractValidator<SaveUserCommand>
    {
        public SaveUserCommandValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            // E-post on kohustuslik, kuni 100 märki, korrektse kujuga ja unikaalne
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
                .EmailAddress().WithMessage("Email is not a valid email address.")
                .MustAsync(async (command, email, cancellationToken) =>
                    !await dbContext.Users.AnyAsync(u => u.Email == email && u.Id != command.Id, cancellationToken))
                .WithMessage("A user with the same email already exists.");

            // Uuel kasutajal on parool kohustuslik. Olemasoleva kasutaja puhul
            // jääb parool samaks, kui uut ei anta.
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required for a new user.")
                .When(x => x.Id == 0);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Password));

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Role must be one of: Participant, Administrator.");
        }
    }
}
