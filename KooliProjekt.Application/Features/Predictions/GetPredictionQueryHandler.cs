using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Predictions
{
    public class GetPredictionQueryHandler : IRequestHandler<GetPredictionQuery, Prediction>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetPredictionQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Prediction> Handle(GetPredictionQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Predictions.FindAsync(new object[] { request.Id }, cancellationToken);

            return result;
        }
    }
}
