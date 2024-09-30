namespace MCSM_Service.Interfaces
{
    public interface ISendMailService
    {
        Task SendVerificationEmail(string userEmail, string token);
    }
}
