using System;
using System.IO;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.Teams;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

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

            if (result.HasErrors)
            {
                return Result(result);
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

            if (result.HasErrors)
            {
                return Result(result);
            }

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

        // Failid (võistkonna lipp, logo jms.)

        [HttpPost("addfile")]
        public async Task<IActionResult> AddFile(int id, IFormFile file)
        {
            if (file == null)
            {
                var error = new OperationResult().AddPropertyError("file", "File is required.");

                return Result(error);
            }

            // Failinime ei võta kliendilt: nime genereerime ise, nii ei saa kaks faili
            // sama nime tõttu üksteist üle kirjutada ja ebaturvalist nime ei pääse läbi.
            // Laiendi võtame originaalnimest, validaator kontrollib, et see oleks pildi laiend.
            var extension = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString() + extension;
            using var inputStream = file.OpenReadStream();

            var command = new AddFileCommand
            {
                TeamId = id,
                FileName = fileName,
                InputStream = inputStream
            };
            var result = await _mediator.Send(command);

            return Result(result);
        }

        [HttpGet("getfile/{id}/{fileName}")]
        public async Task<IActionResult> GetFile(int id, string fileName)
        {
            var command = new GetFileCommand
            {
                TeamId = id,
                FileName = fileName
            };
            var result = await _mediator.Send(command);

            // Valideerimisviga (400) või faili pole (404)
            if (result.HasErrors || result.Value == null)
            {
                return Result(result);
            }

            var contentTypeProvider = new FileExtensionContentTypeProvider();
            if (!contentTypeProvider.TryGetContentType(result.Value.FileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            // File() sulgeb voo pärast vastuse saatmist
            return File(result.Value.Content, contentType);
        }

        [HttpGet("listfiles/{id}")]
        public async Task<IActionResult> ListFiles(int id)
        {
            var query = new ListFilesQuery { TeamId = id };
            var result = await _mediator.Send(query);

            return Result(result);
        }

        [HttpDelete("deletefile")]
        public async Task<IActionResult> DeleteFile([FromBody] DeleteFileCommand command)
        {
            var result = await _mediator.Send(command);

            return Result(result);
        }
    }
}
