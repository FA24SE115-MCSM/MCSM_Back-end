namespace MCSM_Service.Interfaces
{
    public interface ISendMailService
    {
        Task SendVerificationEmail(string userEmail, string token);
        Task SendNewPasswordEmail(string userEmail, string fullName, string newPassword);
    }
}
