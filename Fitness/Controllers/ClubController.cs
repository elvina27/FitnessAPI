using AutoMapper;
using Fitness.API.Exceptions;
using Fitness.API.Models.CreateRequest;
using Fitness.API.Models.Request;
using Fitness.API.Models.Response;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ServicesContracts;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fitness.Api.Controllers
{
    /// <summary>
    /// CRUD контроллер по работе с клубами
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    [ApiExplorerSettings(GroupName = "Club")]
    public class ClubController : ControllerBase
    {
        private readonly IClubService clubService;
        private readonly IMapper mapper;

        public ClubController(IClubService clubService, IMapper mapper)
        {
            this.clubService = clubService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получить список клубов
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClubResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await clubService.GetAllAsync(cancellationToken);
            return Ok(result.Select(x => mapper.Map<ClubResponse>(x)));
        }

        /// <summary>
        /// Получить клуб по Id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ClubResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([Required] Guid id, CancellationToken cancellationToken)
        {
            var item = await clubService.GetByIdAsync(id, cancellationToken);
            return Ok(mapper.Map<ClubResponse>(item));
        }

        /// <summary>
        /// Добавить клуб
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ClubResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody] CreateClubRequest model, CancellationToken cancellationToken)
        {
            var clubModel = mapper.Map<ClubModel>(model);
            var result = await clubService.AddAsync(clubModel, cancellationToken);
            return Ok(mapper.Map<ClubResponse>(result));
        }

        /// <summary>
        /// Изменить клуб
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(ClubResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Edit(ClubRequest request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<ClubModel>(request);
            var result = await clubService.EditAsync(model, cancellationToken);
            return Ok(mapper.Map<ClubResponse>(result));
        }

        /// <summary>
        /// Удалить клуб по Id
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> Delete([Required] Guid id, CancellationToken cancellationToken)
        {
            await clubService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
