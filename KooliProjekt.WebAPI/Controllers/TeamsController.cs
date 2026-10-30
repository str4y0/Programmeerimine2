using System.Threading.Tasks;
using KooliProjekt.Application.Features.Teams;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class TeamsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public TeamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var query = new GetTeamQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Json(result);
        }

        [HttpGet("list")]
        public async Task<IActionResult> List(int page = 1, int pageSize = 10)
        {
            var query = new ListTeamsQuery
            {
                Page = page,
                PageSize = pageSize
            };
            var result = await _mediator.Send(query);

            return Json(result);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SaveTeamCommand command)
        {
            var result = await _mediator.Send(command);

            return Result(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteTeamCommand command)
        {
            var result = await _mediator.Send(command);

            return Result(result);
        }
    }
}
