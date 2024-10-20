using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface IRoomService
    {
        Task<ListViewModel<RoomViewModel>> GetRooms(RoomFilterModel filter, PaginationRequestModel pagination);
        Task<RoomViewModel> GetRoom(Guid id);
        Task<RoomViewModel> CreateRoom(CreateRoomModel model);
        Task<RoomViewModel> UpdateRoom(Guid id, UpdateRoomModel model);
    }
}
