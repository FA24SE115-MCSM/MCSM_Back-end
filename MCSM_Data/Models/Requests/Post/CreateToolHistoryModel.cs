namespace MCSM_Data.Models.Requests.Post
{
    public class CreateToolHistoryModel
    {

        public Guid RetreatId { get; set; }

        public Guid ToolId { get; set; }

        public int NumOfTool { get; set; }
    }
}
