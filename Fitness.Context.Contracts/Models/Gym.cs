namespace Fitness.Context.Contracts.Models
{
    /// <summary>
    /// Зал
    /// </summary>
    public class Gym : BaseAuditEntity
    {
        /// <summary>
        /// Название зала
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Вместимость зала
        /// </summary>
        public short Capacity { get; set; }

        public ICollection<TimeTableItem> TimeTableItems { get; set; }
    }
}
