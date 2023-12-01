using AutoMapper;
using Fitness.API.Exceptions;
using Fitness.API.Models.Response;
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
    }
}
