namespace MCSM_Data.Models.Views
{
    public class RoomViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public int Capacity { get; set; }

        public string Status { get; set; } = null!;
        public DateTime CreateAt { get; set; }

        //public virtual RetreatSchedule? RetreatSchedule { get; set; }

        public virtual RoomTypeViewModel RoomType { get; set; } = null!;
    }
}
