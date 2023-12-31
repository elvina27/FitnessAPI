﻿namespace Fitness.API.Models.CreateRequest
{
    /// <summary>
    /// Модель запроса создания зала
    /// </summary>
    public class CreateGymRequest
    {
        /// <summary>
        /// Название зала
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Вместимость зала
        /// </summary>
        public short Capacity { get; set; }
    }
}
