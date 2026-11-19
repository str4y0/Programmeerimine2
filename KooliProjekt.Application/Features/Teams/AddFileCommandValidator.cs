using System;
using System.IO;
using FluentValidation;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Files;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Teams
{
    public class AddFileCommandValidator : AbstractValidator<AddFileCommand>
    {
        // Lubatud on ainult pildid. SVG jätame välja, sest see võib sisaldada skripte.
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public AddFileCommandValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.TeamId)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(1).WithMessage("TeamId must be greater than or equal to 1.")
                .MustAsync(async (id, cancellationToken) =>
                    await dbContext.Teams.AnyAsync(t => t.Id == id, cancellationToken))
                .WithMessage(x => "Cannot find team with ID " + x.TeamId);

            RuleFor(x => x.FileName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("FileName is required.")
                .MaximumLength(255).WithMessage("FileName must not exceed 255 characters.")
                .Must(FileNameHelper.IsSafeFileName).WithMessage("FileName is not a valid file name.")
                .Must(HasAllowedExtension).WithMessage("Only image files are allowed: " + string.Join(", ", AllowedExtensions));

            RuleFor(x => x.InputStream)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("File content is required.")
                .Must(stream => !stream.CanSeek || stream.Length > 0).WithMessage("File must not be empty.")
                .Must(stream => !stream.CanSeek || stream.Length <= MaxFileSize).WithMessage("File must not be larger than 5 MB.");
        }

        private static bool HasAllowedExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            return Array.Exists(AllowedExtensions, allowed => string.Equals(allowed, extension, StringComparison.OrdinalIgnoreCase));
        }
    }
}
