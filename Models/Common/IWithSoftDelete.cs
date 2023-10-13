namespace market.Models.Common
{
    public interface IWithSoftDelete
    {
        public DateTime? DeleteMoment { get; set; }
        public bool IsDeleted { get; set; }
    }
}