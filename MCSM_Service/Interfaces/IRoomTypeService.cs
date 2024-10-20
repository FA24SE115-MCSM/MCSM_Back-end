using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface IRoomTypeService
    {
        Task<List<RoomTypeViewModel>> GetRoomTypes();
    }
}
