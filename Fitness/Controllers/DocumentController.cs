using AutoMapper;
using Fitnes.API.Models.Response;
using Fitness.API.Exceptions;
using Fitness.API.Models.Response;
using Fitness.Services.Contracts.ServicesContracts;
using Fitness.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fitness.Api.Controllers
{
    /// <summary>
    /// CRUD контроллер по работе с документами
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    [ApiExplorerSettings(GroupName = "Document")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService documentService;
        private readonly IMapper mapper;

        public DocumentController(IDocumentService documentService, IMapper mapper)
        {
            this.documentService = documentService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получить список документы
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DocumentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await documentService.GetAllAsync(cancellationToken);
            return Ok(result.Select(x => mapper.Map<DocumentResponse>(x)));
        }

        /// <summary>
        /// Получить документы по Id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(DocumentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([Required] Guid id, CancellationToken cancellationToken)
        {
            var result = await documentService.GetAllAsync(cancellationToken);
            var result2 = result.Select(x => mapper.Map<DocumentResponse>(x));
            return Ok(result2);
        }
    }
}