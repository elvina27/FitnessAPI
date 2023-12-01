using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Contracts.Exceptions
{
    public class TimeTableEntityNotFoundException<TEntity> : TimeTableNotFoundException
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="TimeTableEntityNotFoundException{TEntity}"/>
        /// </summary>
        public TimeTableEntityNotFoundException(Guid id)
            : base($"Сущность {typeof(TEntity)} c id = {id} не найдена.")
        {
        }
    }
}
