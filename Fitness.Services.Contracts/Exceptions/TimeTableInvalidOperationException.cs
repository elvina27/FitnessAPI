using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Contracts.Exceptions
{
    public class TimeTableInvalidOperationException : TimeTableException
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="TimeTableInvalidOperationException"/>
        /// с указанием сообщения об ошибке
        /// </summary>
        public TimeTableInvalidOperationException(string message)
            : base(message)
        {

        }
    }
}
