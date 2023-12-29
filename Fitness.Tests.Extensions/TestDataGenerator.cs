using Fitness.Context.Contracts.Models;
using Fitness.Services.Contracts.Enums;
using Fitness.Services.Contracts.Models;
using Fitness.Services.Contracts.ModelsRequest;

namespace Fitness.Tests.Extensions
{
    public static class TestDataGenerator
    {
        private static Random random = new Random();
        static public Club Club(Action<Club>? settings = null)
        {
            var result = new Club
            {
                Title = $"{Guid.NewGuid():N}",
                Metro = $"{Guid.NewGuid():N}",
                Address = $"{Guid.NewGuid():N}",
                Email = $"{Guid.NewGuid():N}@mail.com"
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static public Coach Coach(Action<Coach>? settings = null)
        {
            var result = new Coach
            {
                Surname = $"{Guid.NewGuid():N}",
                Name = $"{Guid.NewGuid():N}",
                Patronymic = $"{Guid.NewGuid():N}",
                Email = $"{Guid.NewGuid():N}",
                Age = 35

            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static public Document Document(Action<Document>? settings = null) //уточнить верность!
        {
            var result = new Document
            {
                Number = $"{Guid.NewGuid():N}",
                Series = $"{Guid.NewGuid():N}",
                IssuedBy = $"{Guid.NewGuid():N}",
                DocumentType = Context.Contracts.Enums.DocumentTypes.Diplom,
                //попытка не пытка
                //CoachId = Guid.NewGuid()
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static public Gym Gym(Action<Gym>? settings = null)
        {
            var result = new Gym
            {
                Title = $"{Guid.NewGuid():N}",
                Capacity = 35
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static public Study Study(Action<Study>? settings = null)
        {
            var result = new Study
            {
                Title = $"{Guid.NewGuid():N}",
                Description = $"{Guid.NewGuid():N}",
                Duration = 50,
                Category = Context.Contracts.Enums.Category.Power
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static public TimeTableItem TimeTableItem(Action<TimeTableItem>? settings = null)
        {
            var result = new TimeTableItem
            {
                StartTime = DateTimeOffset.Now,
                Warning = $"{Guid.NewGuid():N}"
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static public ClubModel ClubModel(Action<ClubModel>? settings = null)
        {
            var result = new ClubModel
            {
                Id = Guid.NewGuid(),
                Title = $"{Guid.NewGuid():N}",
                Metro = $"{Guid.NewGuid():N}",
                Address = $"{Guid.NewGuid():N}",
                Email = $"{Guid.NewGuid():N}@mail.ru"
            };

            settings?.Invoke(result);
            return result;
        }

        static public CoachModel CoachModel(Action<CoachModel>? settings = null)
        {
            var result = new CoachModel
            {
                Id = Guid.NewGuid(),
                Surname = $"{Guid.NewGuid():N}",
                Name = $"{Guid.NewGuid():N}",
                Patronymic = $"{Guid.NewGuid():N}",
                Email = $"{Guid.NewGuid():N}",
                Age = 35

            };

            settings?.Invoke(result);
            return result;
        }

        static public DocumentRequestModel DocumentRequestModel(Action<DocumentRequestModel>? settings = null) //уточнить верность!
        {
            var result = new DocumentRequestModel
            {
                Id = Guid.NewGuid(),
                Number = $"{Guid.NewGuid():N}",
                Series = $"{Guid.NewGuid():N}",
                IssuedBy = $"{Guid.NewGuid():N}",
                DocumentType = DocumentTypesModel.None,
                //дай бог
                //CoachId = Guid.NewGuid()
            };

            settings?.Invoke(result);
            return result;
        }

        static public GymModel GymModel(Action<GymModel>? settings = null)
        {
            var result = new GymModel
            {
                Id = Guid.NewGuid(),
                Title = $"{Guid.NewGuid():N}",
                Capacity = 18
            };

            settings?.Invoke(result);
            return result;
        }

        static public StudyModel StudyModel(Action<StudyModel>? settings = null)
        {
            var result = new StudyModel
            {
                Id = Guid.NewGuid(),
                Title = $"{Guid.NewGuid():N}",
                Description = $"{Guid.NewGuid():N}",
                Duration = 50,
                Category = CategoryModel.Endurance
            };

            settings?.Invoke(result);
            return result;
        }

        static public TimeTableItemRequestModel TimeTableItemRequestModel(Action<TimeTableItemRequestModel>? settings = null)
        {
            var result = new TimeTableItemRequestModel
            {
                Id = Guid.NewGuid(),
                StartTime = DateTimeOffset.Now,
                Warning = $"{Guid.NewGuid():N}"
            };

            settings?.Invoke(result);
            return result;
        }
    }
}