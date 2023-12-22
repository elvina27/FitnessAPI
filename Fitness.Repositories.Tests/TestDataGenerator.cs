using System.Net.Sockets;
using System;
using Fitness.Context.Contracts.Models;

namespace Fitness.Repositories.Tests
{
    internal static class TestDataGenerator
    {
        static internal Club Cinema(Action<Club>? settings = null)
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

        static internal Coach Film(Action<Coach>? settings = null)
        {
            var result = new Coach
            {
                Surname = $"{Guid.NewGuid():N}",
                Name = $"{Guid.NewGuid():N}",
                Patronymic = $"{Guid.NewGuid():N}",
                Email = $"{Guid.NewGuid():N}",
                Age = 18
                
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static internal Document Hall(Action<Document>? settings = null)
        {
            var result = new Document
            {
                Number = 1,
                NumberOfSeats = 20
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static internal Gym Client(Action<Gym>? settings = null)
        {
            var result = new Gym
            {
                FirstName = $"{Guid.NewGuid():N}",
                LastName = $"{Guid.NewGuid():N}",
                Patronymic = $"{Guid.NewGuid():N}",
                Age = 18,
                Email = $"{Guid.NewGuid():N}"
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static internal Study Staff(Action<Study>? settings = null)
        {
            var result = new Study
            {
                FirstName = $"{Guid.NewGuid():N}",
                LastName = $"{Guid.NewGuid():N}",
                Patronymic = $"{Guid.NewGuid():N}",
                Age = 18,
                Post = Context.Contracts.Enums.Post.Manager
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        static internal TimeTableItem Ticket(Action<TimeTableItem>? settings = null)
        {
            var result = new TimeTableItem
            {
                Date = DateTimeOffset.Now,
                Place = 1,
                Row = 1,
                Price = 100
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }
    }
}