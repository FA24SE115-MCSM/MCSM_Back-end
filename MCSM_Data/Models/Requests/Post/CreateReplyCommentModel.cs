namespace MCSM_Data.Models.Requests.Post
{
    public class CreateReplyCommentModel
    {
        public Guid CommentId { get; set; }

        public string? Content { get; set; }
    }
}
