using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Contracts.Exceptions
{
    public class TimeTableNotFoundException : TimeTableException
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="TimeTableNotFoundException"/> с указанием
        /// сообщения об ошибке
        /// </summary>
        public TimeTableNotFoundException(string message)
            : base(message)
        { }
    }
}
