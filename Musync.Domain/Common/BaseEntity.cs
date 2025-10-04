using Musync.Identity.Models;

namespace Musync.Domain.Common
{
    public abstract class BaseEntity
    {
        public int ID { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public int? CreatedByID { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public int? UpdatedByID { get; set; }
    }
}
