namespace MCSM_Data.Models.Requests.Post
{
    public class CreateRefundModel
    {
        public Guid RetreatRegId { get; set; }
        public string EmailPaypal { get; set; } = null!;
        public string RefundReason { get; set; } = null!;
    }
}
