using AutoMapper;
using Fitness.API.Exceptions;
using Fitness.API.Models.Response;
using Fitness.Services.Contracts.ServicesContracts;
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
    }
}
