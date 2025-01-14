﻿namespace MCSM_Service.Interfaces
{
    public interface ISendMailService
    {
        Task SendVerificationEmail(string userEmail, string token);
        Task SendVerificationEmail(string userEmail, string token, string password);
        Task SendNewPasswordEmail(string userEmail, string fullName, string newPassword);
        Task SendRetreatCancellationEmail(string userEmail, string userName, string refundDetails);
        Task SendPaymentSuccessEmail(string email, string fullName, string retreatName, DateOnly startDate, DateOnly endDate, string paymentAmount);
    }
}
