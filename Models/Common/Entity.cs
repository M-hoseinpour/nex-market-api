namespace market.Models.Common
{
    public abstract class EntityBase : IEntity, IWithUuid, IWithSoftDelete, IWithTimestamps
    {
        public DateTime CreateMoment { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateMoment { get; set; }
        public DateTime? DeleteMoment { get; set; }
        public bool IsDeleted { get; set; }
        public Guid Uuid { get; set; } = Guid.NewGuid();

    }

    public abstract class Entity<TKey> : EntityBase
    {
        public TKey Id { get; set; }
    }

    public abstract class Entity : Entity<int>
    {
    }
}