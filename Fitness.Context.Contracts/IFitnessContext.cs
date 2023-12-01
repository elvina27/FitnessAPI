using Fitness.Context.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Context.Contracts
{
    /// <summary>
    /// Контекст работы с сущностями
    /// </summary>
    public interface IFitnessContext
    {
        /// <summary>Список <inheritdoc cref="Club"/></summary>
        DbSet<Club> Clubs { get; }

        /// <summary>Список <inheritdoc cref="Coach"/></summary>
        DbSet<Coach> Coaches { get; }

        /// <summary>Список <inheritdoc cref="Gym"/></summary>
        DbSet<Gym> Gyms { get; }

        /// <summary>Список <inheritdoc cref="Document"/></summary>
        DbSet<Document> Documents { get; }

        /// <summary>Список <inheritdoc cref="Study"/></summary>
        DbSet<Study> Studyes { get; }

        /// <summary>Список <inheritdoc cref="TimeTableItem"/></summary>
        DbSet<TimeTableItem> TimeTableItems { get; }
    }
}