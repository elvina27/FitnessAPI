using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Common.Entity.InterfaceDB
{
    /// <summary>
    /// Контекст для работы с записями в БД
    /// </summary>
    public interface IDbWriterContext
    {
        /// <inheritdoc cref="IDbWriter"/>
        IDbWriter Writer { get; }

        /// <inheritdoc cref="IUnitOfWork"/>
        IUnitOfWork UnitOfWork { get; }

        /// <inheritdoc cref="IDateTimeProvider"/>
        IDateTimeProvider DateTimeProvider { get; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        /// <remarks>В реальной системе с авторизацией тут будет IIdentity</remarks>
        string UserName { get; }
    }
}
