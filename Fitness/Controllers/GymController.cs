using AutoMapper;
using Fitnes.API.Models.CreateRequest;
using Fitnes.API.Models.Response;
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
    /// CRUD контроллер по работе с залами
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    [ApiExplorerSettings(GroupName = "Gym")]
    public class GymController : ControllerBase
    {
        private readonly IGymService gymService;
        private readonly IMapper mapper;

        public GymController(IGymService gymService, IMapper mapper)
        {
            this.gymService = gymService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получить список залов
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GymResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await gymService.GetAllAsync(cancellationToken);
            return Ok(result.Select(x => mapper.Map<GymResponse>(x)));
        }

        /// <summary>
        /// Получить зал по Id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(GymResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([Required] Guid id, CancellationToken cancellationToken)
        {
            var item = await gymService.GetByIdAsync(id, cancellationToken);
            return Ok(mapper.Map<GymResponse>(item));
        }

        /// <summary>
        /// Добавить зал
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(GymResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody] CreateGymRequest model, CancellationToken cancellationToken)
        {
            var gymModel = mapper.Map<GymModel>(model);
            var result = await gymService.AddAsync(gymModel, cancellationToken);
            return Ok(mapper.Map<GymResponse>(result));
        }

        /// <summary>
        /// Изменить зал
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(GymResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Edit(GymRequest request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<GymModel>(request);
            var result = await gymService.EditAsync(model, cancellationToken);
            return Ok(mapper.Map<GymResponse>(result));
        }

        /// <summary>
        /// Удалить зал по Id
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> Delete([Required] Guid id, CancellationToken cancellationToken)
        {
            await gymService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
