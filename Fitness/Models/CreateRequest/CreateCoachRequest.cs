namespace Fitness.API.Models.CreateRequest
{
    /// <summary>
    /// Тренер
    /// </summary>
    public class CreateCoachRequest
    {
        /// <summary>
        /// Фамилия 
        /// </summary>
        public string Surname { get; set; } = string.Empty;

        /// <summary>
        /// Имя 
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Отчество 
        /// </summary>
        public string Patronymic { get; set; } = string.Empty;

        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Возраст
        /// </summary>
        public short Age { get; set; } 
    }
}

