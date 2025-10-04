namespace Musync.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public int? CreatedById { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public int? UpdatedById { get; set; }
    }
}
