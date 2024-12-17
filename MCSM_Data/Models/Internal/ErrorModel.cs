namespace MCSM_Data.Models.Internal
{
    public class ErrorModel
    {
        public string message { get; set; } = null!;
        public List<string>? messages { get; set; } = null;
    }
}
