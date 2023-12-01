using AutoMapper;
using Fitness.API.Exceptions;
using Fitness.API.Models.Response;
using Fitness.Services.Contracts.ServicesContracts;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fitness.Api.Controllers
{
    /// <summary>
    /// CRUD контроллер по работе с занятиями
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    [ApiExplorerSettings(GroupName = "Study")]
    public class StudyController : ControllerBase
    {
        private readonly IStudyService studyService;
        private readonly IMapper mapper;

        public StudyController(IStudyService studyService, IMapper mapper)
        {
            this.studyService = studyService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получить список занятий
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudyResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await studyService.GetAllAsync(cancellationToken);
            return Ok(result.Select(x => mapper.Map<StudyResponse>(x)));
        }

        /// <summary>
        /// Получить занятие по Id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(StudyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([Required] Guid id, CancellationToken cancellationToken)
        {
            var item = await studyService.GetByIdAsync(id, cancellationToken);
            return Ok(mapper.Map<StudyResponse>(item));
        }
    }
}
