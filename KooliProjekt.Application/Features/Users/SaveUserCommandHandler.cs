using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            User user;

            if (request.Id == 0)
            {
                if (string.IsNullOrEmpty(request.Password))
                {
                    return result.AddPropertyError("Password", "Password is required for a new user");
                }

                user = new User();
                _dbContext.Users.Add(user);
            }
            else
            {
                user = await _dbContext.Users.FindAsync(new object[] { request.Id }, cancellationToken);
                if (user == null)
                {
                    return result.AddPropertyError("Id", "Cannot find user with ID " + request.Id);
                }
            }

            user.Name = request.Name;
            user.Email = request.Email;
            user.Role = request.Role;

            // Olemasoleva kasutaja puhul jääb parool samaks, kui uut ei anta
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = HashPassword(request.Password);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }

        // Sama räsi, mida kasutab SeedData
        private static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}
