namespace MCSM_Data.Models.Requests.Post
{
    public class CreateRoomModel
    {
        public Guid RoomTypeId { get; set; }

        public string Name { get; set; } = null!;

        public int Capacity { get; set; }

    }
}
