using AutoMapper;
using Fitness.API.Exceptions;
using Fitness.API.Models.CreateRequest;
using Fitness.API.Models.Request;
using Fitness.API.Models.Response;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ModelsRequest;
using Fitness.Services.Contracts.ServicesContracts;
using Fitness.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fitness.Api.Controllers
{
    /// <summary>
    /// CRUD контроллер по работе с элементами расписания
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    [ApiExplorerSettings(GroupName = "TimeTableItem")]
    public class TimeTableItemController : ControllerBase
    {
        private readonly ITimeTableItemService timeTableItemService;
        private readonly IMapper mapper;

        public TimeTableItemController(ITimeTableItemService timeTableItemService, IMapper mapper)
        {
            this.timeTableItemService = timeTableItemService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получить список элементов расписания
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TimeTableItemResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await timeTableItemService.GetAllAsync(cancellationToken);
            return Ok(result.Select(x => mapper.Map<TimeTableItemResponse>(x)));
        }

        /// <summary>
        /// Получить элемент расписания по Id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TimeTableItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([Required] Guid id, CancellationToken cancellationToken)
        {
            var result = await timeTableItemService.GetAllAsync(cancellationToken);
            var result2 = result.Select(x => mapper.Map<TimeTableItemResponse>(x));
            return Ok(result2);
        }

        /// <summary>
        /// Добавить элемент расписания
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(TimeTableItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody] CreateTimeTableItemRequest model, CancellationToken cancellationToken)
        {
            var timeTableItemModel = mapper.Map<TimeTableItemRequestModel>(model);
            var result = await timeTableItemService.AddAsync(timeTableItemModel, cancellationToken);
            return Ok(mapper.Map<TimeTableItemResponse>(result));
        }

        /// <summary>
        /// Изменить элемент расписания
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(TimeTableItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Edit(TimeTableItemRequest request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<TimeTableItemRequestModel>(request);
            var result = await timeTableItemService.EditAsync(model, cancellationToken);
            return Ok(mapper.Map<TimeTableItemResponse>(result));
        }

        /// <summary>
        /// Удалить элемент расписания по Id
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> Delete([Required] Guid id, CancellationToken cancellationToken)
        {
            await timeTableItemService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
