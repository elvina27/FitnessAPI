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

        /// <summary>
        /// Добавить занятие
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(StudyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody] CreateStudyRequest model, CancellationToken cancellationToken)
        {
            var studyModel = mapper.Map<StudyModel>(model);
            var result = await studyService.AddAsync(studyModel, cancellationToken);
            return Ok(mapper.Map<StudyResponse>(result));
        }

        /// <summary>
        /// Изменить занятие
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(StudyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Edit(StudyRequest request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<StudyModel>(request);
            var result = await studyService.EditAsync(model, cancellationToken);
            return Ok(mapper.Map<StudyResponse>(result));
        }

        /// <summary>
        /// Удалить занятие по Id
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> Delete([Required] Guid id, CancellationToken cancellationToken)
        {
            await studyService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
