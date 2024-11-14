using MCSM_Data.Entities;

namespace MCSM_Data.Models.Views
{
    public class RetreatMonkViewModel
    {
        public Guid Id { get; set; }

        public virtual AccountViewModel Monk { get; set; } = null!;

        public Guid RetreatId { get; set; }        
    }
}
