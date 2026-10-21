using System.Collections.Generic;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Predictions
{
    public class ListPredictionsQuery : IRequest<IEnumerable<Prediction>>
    {
    }
}
