using AutoMapper;
using Fitness.API.Exceptions;
using Fitness.API.Models.Response;
using Fitness.Services.Contracts.ServicesContracts;
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
    }
}
