namespace market.Models.Common
{
    public interface IWithTimestamps
    {
        public DateTime CreateMoment { get; set; }
        public DateTime? UpdateMoment { get; set; }
    }
}