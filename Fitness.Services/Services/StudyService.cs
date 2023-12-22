using AutoMapper;
using Fitness.Common.Entity.InterfaceDB;
using Fitness.Context.Contracts.Models;
using Fitness.Repositories.Contracts.ReadRepositoriesContracts;
using Fitness.Repositories.Contracts.WriteRepositoriesContracts;
using Fitness.Services.Anchors;
using Fitness.Services.Contracts.Exceptions;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ServicesContracts;
using Fitness.Services.Validator;

namespace Fitness.Services.Implementations
{
    /// <inheritdoc cref="IStudyService"/>
    public class StudyService : IStudyService, IServiceAnchor
    {
        private readonly IStudyWriteRepository studyWriteRepository;
        private readonly IStudyReadRepository studyReadRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IServiceValidatorService validatorService;

        public StudyService(IStudyWriteRepository studyWriteRepository, IStudyReadRepository studyReadRepository,
            IUnitOfWork unitOfWork, IMapper mapper, IServiceValidatorService validatorService)
        {
            this.studyReadRepository = studyReadRepository;
            this.studyWriteRepository = studyWriteRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.validatorService = validatorService;
        }


        async Task<StudyModel> IStudyService.AddAsync(StudyModel model, CancellationToken cancellationToken)
        {
            model.Id = Guid.NewGuid();

            await validatorService.ValidateAsync(model, cancellationToken);

            var item = mapper.Map<Study>(model);
            studyWriteRepository.Add(item);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<StudyModel>(item);
        }

        async Task IStudyService.DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var targetStudy = await studyReadRepository.GetByIdAsync(id, cancellationToken);

            if (targetStudy == null)
            {
                throw new TimeTableEntityNotFoundException<Study>(id);
            }

            if (targetStudy.DeletedAt.HasValue)
            {
                throw new TimeTableInvalidOperationException($"Занятие с идентификатором {id} уже удалено");
            }

            studyWriteRepository.Delete(targetStudy);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        async Task<StudyModel> IStudyService.EditAsync(StudyModel source, CancellationToken cancellationToken)
        {
            await validatorService.ValidateAsync(source, cancellationToken);

            var targetStudy = await studyReadRepository.GetByIdAsync(source.Id, cancellationToken);

            if (targetStudy == null)
            {
                throw new TimeTableEntityNotFoundException<Study>(source.Id);
            }

            targetStudy = mapper.Map<Study>(source);
            studyWriteRepository.Update(targetStudy);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<StudyModel>(targetStudy);
        }

        async Task<IEnumerable<StudyModel>> IStudyService.GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await studyReadRepository.GetAllAsync(cancellationToken);

            return result.Select(x => mapper.Map<StudyModel>(x));
        }

        async Task<StudyModel?> IStudyService.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await studyReadRepository.GetByIdAsync(id, cancellationToken);

            if (item == null)
            {
                throw new TimeTableEntityNotFoundException<Study>(id);
            }

            return mapper.Map<StudyModel>(item);
        }
    }
}