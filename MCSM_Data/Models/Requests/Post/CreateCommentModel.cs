namespace MCSM_Data.Models.Requests.Post
{
    public class CreateCommentModel
    {
        public Guid PostId { get; set; }

        public string? Content { get; set; } 

    }
}
