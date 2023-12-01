namespace Fitness.API.Models.Response
{
    /// <summary>
    /// Клуб
    /// </summary>
    public class ClubResponse
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название клуба
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Станция метро 
        /// </summary>
        public string? Metro { get; set; }

        /// <summary>
        /// Адрес 
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }
}
