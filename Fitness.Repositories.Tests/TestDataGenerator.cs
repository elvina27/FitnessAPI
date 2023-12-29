using System.Net.Sockets;
using System;
using Fitness.Context.Contracts.Models;
using Fitness.Context.Contracts.Enums;

namespace Fitness.Repositories.Tests
{
    internal static class TestDataGenerator
    {
        static internal Club Club(Action<Club>? settings = null)
        {
            var result = new Club
            {
                Title = $"{Guid.NewGuid():N}",
                Metro = $"{Guid.NewGuid():N}",
                Address = $"{Guid.NewGuid():N}",
                Email = $"{Guid.NewGuid():N}"
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static internal Coach Coach(Action<Coach>? settings = null)
        {
            var result = new Coach
            {
                Surname = $"{Guid.NewGuid():N}",
                Name = $"{Guid.NewGuid():N}",
                Patronymic = $"{Guid.NewGuid():N}",
                Email = $"{Guid.NewGuid():N}",
                Age = 31
                
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static internal Document Document(Action<Document>? settings = null) //уточнить верность!
        {
            var result = new Document
            {
                Number = $"{Guid.NewGuid():N}",
                Series = $"{Guid.NewGuid():N}",
                IssuedBy = $"{Guid.NewGuid():N}",
                DocumentType = Context.Contracts.Enums.DocumentTypes.Pasport,
                ///добавила хз правильно ли
                CoachId = Guid.NewGuid()
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static internal Gym Gym(Action<Gym>? settings = null)
        {
            var result = new Gym
            {
                Title = $"{Guid.NewGuid():N}",
                Capacity = 30
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static internal Study Study(Action<Study>? settings = null)
        {
            var result = new Study
            {
                Title = $"{Guid.NewGuid():N}",
                Description = $"{Guid.NewGuid():N}",
                Duration = 55,
                Category = Context.Contracts.Enums.Category.Cardio
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static internal TimeTableItem TimeTableItem(Action<TimeTableItem>? settings = null)
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
    }
}