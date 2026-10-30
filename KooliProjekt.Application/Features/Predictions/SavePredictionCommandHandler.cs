using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Predictions
{
    public class SavePredictionCommandHandler : IRequestHandler<SavePredictionCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SavePredictionCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SavePredictionCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var userExists = await _dbContext.Users
                .AnyAsync(user => user.Id == request.UserId, cancellationToken);
            if (!userExists)
            {
                return result.AddPropertyError("UserId", "Cannot find user with ID " + request.UserId);
            }

            var matchExists = await _dbContext.Matches
                .AnyAsync(match => match.Id == request.MatchId, cancellationToken);
            if (!matchExists)
            {
                return result.AddPropertyError("MatchId", "Cannot find match with ID " + request.MatchId);
            }

            Prediction prediction;

            if (request.Id == 0)
            {
                prediction = new Prediction
                {
                    PredictedAt = DateTime.UtcNow
                };
                _dbContext.Predictions.Add(prediction);
            }
            else
            {
                prediction = await _dbContext.Predictions.FindAsync(new object[] { request.Id }, cancellationToken);
                if (prediction == null)
                {
                    return result.AddPropertyError("Id", "Cannot find prediction with ID " + request.Id);
                }
            }

            // PredictedAt ja Points ei tule kliendilt: aja paneb server, punktid arvutatakse hiljem
            prediction.UserId = request.UserId;
            prediction.MatchId = request.MatchId;
            prediction.HomeTeamGoals = request.HomeTeamGoals;
            prediction.AwayTeamGoals = request.AwayTeamGoals;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
