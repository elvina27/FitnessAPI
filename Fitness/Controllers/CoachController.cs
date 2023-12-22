using AutoMapper;
using Fitness.API.Exceptions;
using Fitness.API.Models.CreateRequest;
using Fitness.API.Models.Request;
using Fitness.API.Models.Response;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ServicesContracts;
using Fitness.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fitness.Api.Controllers
{
    /// <summary>
    /// CRUD контроллер по работе с тренерами
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    [ApiExplorerSettings(GroupName = "Coach")]
    public class CoachController : ControllerBase
    {
        private readonly ICoachService coachService;
        private readonly IMapper mapper;

        public CoachController(ICoachService coachService, IMapper mapper)
        {
            this.coachService = coachService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получить список тренеров
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CoachResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await coachService.GetAllAsync(cancellationToken);
            return Ok(result.Select(x => mapper.Map<CoachResponse>(x)));
        }

        /// <summary>
        /// Получить тренера по Id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CoachResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([Required] Guid id, CancellationToken cancellationToken)
        {
            var item = await coachService.GetByIdAsync(id, cancellationToken);
            return Ok(mapper.Map<CoachResponse>(item));
        }

        /// <summary>
        /// Добавить тренера
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CoachResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody] CreateCoachRequest model, CancellationToken cancellationToken)
        {
            var coachModel = mapper.Map<CoachModel>(model);
            var result = await coachService.AddAsync(coachModel, cancellationToken);
            return Ok(mapper.Map<CoachResponse>(result));
        }

        /// <summary>
        /// Изменить тренера
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(CoachResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Edit(CoachRequest request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<CoachModel>(request);
            var result = await coachService.EditAsync(model, cancellationToken);
            return Ok(mapper.Map<CoachResponse>(result));
        }

        /// <summary>
        /// Удалить тренера по Id
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> Delete([Required] Guid id, CancellationToken cancellationToken)
        {
            await coachService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
