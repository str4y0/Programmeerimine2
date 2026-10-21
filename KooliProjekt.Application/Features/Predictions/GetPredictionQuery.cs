using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Predictions
{
    public class GetPredictionQuery : IRequest<Prediction>
    {
        public int Id { get; set; }
    }
}
