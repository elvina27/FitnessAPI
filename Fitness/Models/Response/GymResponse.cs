namespace Fitness.API.Models.Response
{
    /// <summary>
    /// Зал
    /// </summary>
    public class GymResponse
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

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
