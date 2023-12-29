using AutoMapper;
using Fitness.Common.Entity.InterfaceDB;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Fitness.Repositories.Contracts.WriteRepositoriesContracts;
using Fitness.Repositories.ReadRepositories;
using Fitness.Repositories.WriteRepositories;
using Fitness.Services.Anchors;
using Fitness.Services.Contracts.Exceptions;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ModelsRequest;
using Fitness.Services.Contracts.ServicesContracts;
using Fitness.Services.Validator;
using System.Net.Sockets;

namespace Fitness.Services.Implementations
{
    /// <inheritdoc cref="IDocumentService"/>
    public class DocumentService : IDocumentService, IServiceAnchor
    {
        private readonly IDocumentReadRepository documentReadRepository;
        private readonly IDocumentWriteRepository documentWriteRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ICoachReadRepository coachReadRepository;
        private readonly IServiceValidatorService validatorService;

        public DocumentService(IDocumentReadRepository documentReadRepository,
            IDocumentWriteRepository documentWriteRepository,
            IMapper mapper, ICoachReadRepository coachReadRepository,
            IServiceValidatorService validatorService, IUnitOfWork unitOfWork)
        {
            this.documentReadRepository = documentReadRepository;
            this.documentWriteRepository = documentWriteRepository;
            this.mapper = mapper;
            this.coachReadRepository = coachReadRepository;
            this.validatorService = validatorService;
            this.unitOfWork = unitOfWork;
        }

        async Task<IEnumerable<DocumentModel>> IDocumentService.GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await documentReadRepository.GetAllAsync(cancellationToken);
            var coaches = await coachReadRepository.GetByIdsAsync(result.Where(x => x.CoachId.HasValue).Select(x => x.CoachId!.Value).Distinct(), cancellationToken);

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

        async Task IDocumentService.DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var targetDocument = await documentReadRepository.GetByIdAsync(id, cancellationToken);

            if (targetDocument == null)
            {
                throw new TimeTableEntityNotFoundException<Document>(id);
            }

            if (targetDocument.DeletedAt.HasValue)
            {
                throw new TimeTableInvalidOperationException($"Документ с идентификатором {id} уже удален");
            }

            documentWriteRepository.Delete(targetDocument);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        async Task<DocumentModel> IDocumentService.EditAsync(DocumentRequestModel model, CancellationToken cancellationToken)
        {
            await validatorService.ValidateAsync(model, cancellationToken);

            var document = await documentReadRepository.GetByIdAsync(model.Id, cancellationToken);

            if (document == null)
            {
                throw new TimeTableEntityNotFoundException<Document>(model.Id);
            }

            document = mapper.Map<Document>(model);
            documentWriteRepository.Update(document);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return await GetDocumentModelOnMapping(document, cancellationToken);
        }

        async private Task<DocumentModel> GetDocumentModelOnMapping(Document document, CancellationToken cancellationToken)
        {
            var documentModel = mapper.Map<DocumentModel>(document);
            documentModel.Coach = mapper.Map<CoachModel>(document.CoachId.HasValue
                ? await coachReadRepository.GetByIdAsync(document.CoachId.Value, cancellationToken)
                : null);


            return documentModel;
        }

        async Task<DocumentModel> IDocumentService.AddAsync(DocumentRequestModel model, CancellationToken cancellationToken)
        {
            model.Id = Guid.NewGuid();
            await validatorService.ValidateAsync(model, cancellationToken);

            var document = mapper.Map<Document>(model);
            documentWriteRepository.Add(document);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return await GetDocumentModelOnMapping(document, cancellationToken);
        }
    }
}

