namespace MCSM_Utility.Settings
{
    public class PayPalSetting
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string ReturnUrl { get; set; } = null!;
        public string CancelUrl { get; set; } = null!;


        public string BaseUrl { get; set; } = null!;
    }
}
