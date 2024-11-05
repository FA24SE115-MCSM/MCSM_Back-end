namespace MCSM_Utility.Settings
{
    public class AppSetting
    {
        public string SecretKey { get; set; } = null!;
        public string RefreshTokenSecret { get; set; } = null!;

        public string StorageBucket { get; set; } = null!;
        public string ImageFolder { get; set; } = null!;
        public string DocumentFolder { get; set; } = null!;


        public MailKitSetting MailKit { get; set; } = null!;
    }
}
