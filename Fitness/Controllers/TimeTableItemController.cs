using AutoMapper;
using Fitness.API.Exceptions;
using Fitness.API.Models.Response;
using Fitness.Services.Contracts.ServicesContracts;
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
    }
}
