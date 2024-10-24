using MCSM_Data.Entities;

namespace MCSM_Data.Models.Views
{
    public class RetreatMonkViewModel
    {
        public Guid Id { get; set; }

        public Guid MonkId { get; set; }

        public string MonkFirstName { get; set; } = null!;

        public string MonkLastName { get; set; } = null!;

        public Guid RetreatId { get; set; }        
    }
}
