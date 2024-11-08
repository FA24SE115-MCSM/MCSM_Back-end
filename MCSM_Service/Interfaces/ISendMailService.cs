namespace MCSM_Service.Interfaces
{
    public interface ISendMailService
    {
        Task SendVerificationEmail(string userEmail, string token);
        Task SendVerificationEmail(string userEmail, string token, string password);
        Task SendNewPasswordEmail(string userEmail, string fullName, string newPassword);
        
    }
}
