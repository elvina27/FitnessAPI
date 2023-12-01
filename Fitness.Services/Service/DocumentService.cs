using AutoMapper;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Fitness.Services.Anchors;
using Fitness.Services.Contracts.Exceptions;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ServicesContracts;

namespace Fitness.Services.Implementations
{
    public class DocumentService : IDocumentService, IServiceAnchor
    {
        private readonly IDocumentReadRepository documentReadRepository;
        private readonly IMapper mapper;
        private readonly ICoachReadRepository coachReadRepository;

        public DocumentService(IDocumentReadRepository documentReadRepository, IMapper mapper, ICoachReadRepository coachReadRepository)
        {
            this.documentReadRepository = documentReadRepository;
            this.mapper = mapper;
            this.coachReadRepository = coachReadRepository;
        }

        async Task<IEnumerable<DocumentModel>> IDocumentService.GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await documentReadRepository.GetAllAsync(cancellationToken);
            var coaches = await coachReadRepository.GetByIdsAsync(result.Where(x => x.CoachId.HasValue).Select(x => x.CoachId!.Value).Distinct() ,cancellationToken);          

            var results = new List<DocumentModel>();

            foreach (var document in result)
            {
                var Model = mapper.Map<DocumentModel>(document);    
                Model.Coach = document.CoachId.HasValue &&
                                              coaches.TryGetValue(document.CoachId!.Value, out var coach)
                        ? mapper.Map<CoachModel>(coach)
                        : null;

                results.Add(Model);
            }

            return results;
        }

        async Task<DocumentModel?> IDocumentService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await documentReadRepository.GetByIdAsync(id, cancellationToken);

            if (item == null)
            {
                throw new TimeTableEntityNotFoundException<Document>(id);
            }

            var documentModel = mapper.Map<DocumentModel>(item);
            documentModel.Coach = item.CoachId.HasValue ?
                            mapper.Map<CoachModel>(await coachReadRepository.GetByIdAsync(item.CoachId.Value, cancellationToken))
                            : null;

            return documentModel;
        }
    }
}
