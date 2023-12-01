namespace Fitness.API.Models.Response
{
    /// <summary>
    /// Тренер
    /// </summary>
    public class CoachResponse
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя 
        /// </summary>
        public string Name { get; set; } = string.Empty;

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

