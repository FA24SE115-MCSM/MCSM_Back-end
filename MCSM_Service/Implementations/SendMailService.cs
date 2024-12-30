using MailKit.Security;
using MCSM_Service.Interfaces;
using MCSM_Utility.Settings;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MCSM_Service.Implementations
{
    public class SendMailService : ISendMailService
    {
        private readonly string logoSendMail = "https://firebasestorage.googleapis.com/v0/b/mcsm-fa24se115.appspot.com/o/images%2Fe0142447-aabc-48d7-82f9-0b0fdb043489?alt=media";
        private readonly string _sendUrl;
        private readonly string _nameApp;
        private readonly string _emailAddress;
        private readonly bool _useSSL;
        private readonly string _host;
        private readonly int _port;
        private readonly bool _useStartTls;
        private readonly string _username;
        private readonly string _password;
        //private readonly static CancellationToken ct = new CancellationToken();

        public SendMailService(IOptions<AppSetting> appSettings)
        {
            _sendUrl = appSettings.Value.MailKit.SendUrl;
            _nameApp = appSettings.Value.MailKit.NameApp;
            _emailAddress = appSettings.Value.MailKit.EMailAddress;
            _useSSL = appSettings.Value.MailKit.UseSSL;
            _host = appSettings.Value.MailKit.Host;
            _port = appSettings.Value.MailKit.Port;
            _useStartTls = appSettings.Value.MailKit.UseStartTls;
            _username = appSettings.Value.MailKit.UserName;
            _password = appSettings.Value.MailKit.Password;
        }

        public async Task SendVerificationEmail(string userEmail, string token)
        {
            var verificationLink = $"{_sendUrl}/{token}";

            try
            {
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress(_nameApp, _emailAddress));
                mail.Sender = new MailboxAddress(_nameApp, _emailAddress);
                mail.To.Add(MailboxAddress.Parse(userEmail));

                var body = new BodyBuilder();
                mail.Subject = "Email Verification";


                body.HtmlBody = $@"
                    <html>
                        <head>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    background-color: #929292;
                                    padding: 20px;
                                    margin: 0;
                                }}
                                .container {{
                                    max-width: 600px;
                                    margin: auto;
                                    background: white;
                                    padding: 20px;
                                    border: .5px solid;
                                    border-radius: 8px;
                                    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                                }}
                                h1 {{
                                    color: black; 
                                }}
                                p {{
                                    font-size: 16px;
                                    line-height: 1.5;
                                    color: #555;
                                }}
                                a {{
                                    display: block;
                                    width: fit-content;
                                    margin: 20px auto;
                                    padding: 10px 15px;
                                    background-color: #28a745; 
                                    color: black; 
                                    text-decoration: none;
                                    border-radius: 5px;
                                    font-weight: bold;
                                }}
                                a:hover {{
                                    background-color: #218838; 
                                }}
                                .header {{
                                    text-align: center;
                                    background-color: #007BFF; 
                                    color: white;
                                    padding: 10px 0;
                                    border-radius: 8px 8px 0 0;
                                }}
                                .logo {{
                                    width: 100px; 
                                    height: 100px; 
                                    border-radius: 50%; 
                                    display: block; 
                                    margin: 10px auto; 
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='container'>
                                <div class='header'>
                                    <h1>Email Verification</h1>
                                </div>
                                <img class='logo' src='{logoSendMail}' alt='Logo' /> 
                                <p>Hi there!</p>
                                <p>Please verify your email address by clicking the button below:</p>
                                <a href='{verificationLink}'>Verify Email</a>
                                <p>If you did not create an account, you can safely ignore this email.</p>
                                <p>Thank you!</p>
                                <p>Best regards,<br>{_nameApp}</p>
                            </div>
                        </body>
                    </html>";

                mail.Body = body.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                if (_useSSL)
                {
                    await smtp.ConnectAsync(_host, _port, SecureSocketOptions.SslOnConnect);
                }
                else if (_useStartTls)
                {
                    await smtp.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
                }

                await smtp.AuthenticateAsync(_username, _password);
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending verification email", ex);
            }
        }

        public async Task SendVerificationEmail(string userEmail, string token, string password)
        {
            var verificationLink = $"{_sendUrl}/{token}";
            var email = userEmail;
            try
            {
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress(_nameApp, _emailAddress));
                mail.Sender = new MailboxAddress(_nameApp, _emailAddress);
                mail.To.Add(MailboxAddress.Parse(userEmail));

                var body = new BodyBuilder();
                mail.Subject = "Email Verification";


                body.HtmlBody = $@"
                    <html>
                        <head>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    background-color: #929292;
                                    padding: 20px;
                                    margin: 0;
                                }}
                                .container {{
                                    max-width: 600px;
                                    margin: auto;
                                    background: white;
                                    padding: 20px;
                                    border: .5px solid;
                                    border-radius: 8px;
                                    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                                }}
                                h1 {{
                                    color: black; 
                                }}
                                p {{
                                    font-size: 16px;
                                    line-height: 1.5;
                                    color: #555;
                                }}
                                .button {{
                                    display: block;
                                    width: fit-content;
                                    margin: 20px auto;
                                    padding: 10px 15px;
                                    background-color: #28a745; 
                                    color: black; 
                                    text-decoration: none;
                                    border-radius: 5px;
                                    font-weight: bold;
                                }}
                                .button:hover {{
                                    background-color: #218838; 
                                }}
                                .header {{
                                    text-align: center;
                                    background-color: #007BFF; 
                                    color: white;
                                    padding: 10px 0;
                                    border-radius: 8px 8px 0 0;
                                }}
                                .logo {{
                                    width: 100px; 
                                    height: 100px; 
                                    border-radius: 50%; 
                                    display: block; 
                                    margin: 10px auto; 
                                }}
                                a {{
                                    text-decoration: none;
                                    color: inherit;
                                    pointer-events: none;
                                }}
                                .account-box {{
                                    font-size: 15px;
                                    font-weight: bold;
                                    color: #333;
                                    background-color: #e9ecef;
                                    padding: 10px;
                                    border-radius: 5px;
                                    text-align: left;
                                    margin-top: 15px;
                                    list-style: none;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='container'>
                                <div class='header'>
                                    <h1>Email Verification</h1>
                                </div>
                                <img class='logo' src='{logoSendMail}' alt='Logo' /> 
                                <p>Hi there! Thank you for registering for the retreat at <strong>{_nameApp}</strong></p>
                                <p>Here are your account details:</p>
                                    <ul class='account-box'>
                                        <li><strong>Email:</strong> <a>{email}<a/></li>
                                        <li><strong>Password:</strong> {password}</li>
                                    </ul>
                                <p>Please verify your email address to complete your account setup by clicking the button below:</p>
                                <a href='{verificationLink}' class ='button'>Verify Email</a>
                                <p>If you did not create an account, you can safely ignore this email.</p>
                                <p>Thank you!</p>
                                <p>Best regards,<br>{_nameApp}</p>
                            </div>
                        </body>
                    </html>";

                mail.Body = body.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                if (_useSSL)
                {
                    await smtp.ConnectAsync(_host, _port, SecureSocketOptions.SslOnConnect);
                }
                else if (_useStartTls)
                {
                    await smtp.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
                }

                await smtp.AuthenticateAsync(_username, _password);
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending verification email", ex);
            }
        }


        public async Task SendNewPasswordEmail(string userEmail, string fullName, string newPassword)
        {
            try
            {
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress(_nameApp, _emailAddress));
                mail.Sender = new MailboxAddress(_nameApp, _emailAddress);
                mail.To.Add(MailboxAddress.Parse(userEmail));

                var body = new BodyBuilder();
                mail.Subject = "Your Password Reset Request";


                body.HtmlBody = $@"
                    <html>
                        <head>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    background-color: #f4f4f4;
                                    padding: 20px;
                                    margin: 0;
                                }}
                                .container {{
                                    max-width: 600px;
                                    margin: auto;
                                    background: white;
                                    padding: 20px;
                                    border: .5px solid;
                                    border-radius: 8px;
                                    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                                }}
                                h1 {{
                                    color: black; 
                                }}
                                p {{
                                    font-size: 16px;
                                    line-height: 1.5;
                                    color: #555;
                                }}
                                .password-box {{
                                    font-size: 20px;
                                    font-weight: bold;
                                    color: #333;
                                    background-color: #e9ecef;
                                    padding: 10px;
                                    border-radius: 5px;
                                    text-align: center;
                                    margin-top: 15px;
                                }}
                                .header {{
                                    text-align: center;
                                    background-color: #007BFF; 
                                    color: white;
                                    padding: 10px 0;
                                    border-radius: 8px 8px 0 0;
                                }}
                                .logo {{
                                    width: 100px; 
                                    height: 100px; 
                                    border-radius: 50%; 
                                    display: block; 
                                    margin: 10px auto; 
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='container'>
                                <div class='header'>
                                    <h1>Password Reset</h1>
                                </div>
                                <img class='logo' src='{logoSendMail}' alt='Logo' /> 
                                <p>Hello, <strong>{fullName}</strong></p>
                                <p>Your password has been successfully reset. Here is your new password:</p>
                                <div class='password-box'>{newPassword}</div>
                                <p>Please use this new password to log in, and consider changing it after logging in for added security.</p>
                                <p>If you did not request a password reset, please contact us immediately.</p>
                                <p>Best regards,<br>{_nameApp}</p>
                            </div>
                        </body>
                    </html>";

                mail.Body = body.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                if (_useSSL)
                {
                    await smtp.ConnectAsync(_host, _port, SecureSocketOptions.SslOnConnect);
                }
                else if (_useStartTls)
                {
                    await smtp.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
                }

                await smtp.AuthenticateAsync(_username, _password);
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending reset password email", ex);
            }
        }


        public async Task SendRetreatCancellationEmail(string userEmail, string userName, string refundDetails)
        {
            try
            {
                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress(_nameApp, _emailAddress));
                mail.Sender = new MailboxAddress(_nameApp, _emailAddress);
                mail.To.Add(MailboxAddress.Parse(userEmail));

                mail.Subject = "Retreat Cancellation Notification";

                var body = new BodyBuilder
                {
                    HtmlBody = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Retreat Cancellation</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f8f9fa;
                        padding: 20px;
                        margin: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: auto;
                        background: white;
                        padding: 20px;
                        border: 0.5px solid #ddd;
                        border-radius: 8px;
                        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                    }}
                    h1 {{
                        color: #dc3545;
                        text-align: center;
                    }}
                    p {{
                        font-size: 16px;
                        line-height: 1.5;
                        color: #555;
                    }}
                    .header {{
                        text-align: center;
                        background-color: #dc3545;
                        color: white;
                        padding: 10px 0;
                        border-radius: 8px 8px 0 0;
                    }}
                    .logo {{
                        width: 100px;
                        height: 100px;
                        border-radius: 50%;
                        display: block;
                        margin: 10px auto;
                    }}
                    .account-box {{
                        font-size: 15px;
                        font-weight: bold;
                        color: #333;
                        background-color: #e9ecef;
                        padding: 10px;
                        border-radius: 5px;
                        text-align: left;
                        margin-top: 15px;
                        list-style: none;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Retreat Cancellation</h1>
                    </div>
                    <img class='logo' src='{logoSendMail}' alt='Logo'>
                    <p>Dear {userName},</p>
                    <p>We regret to inform you that due to insufficient registrations, we are unable to proceed with the upcoming retreat at <strong>{_nameApp}</strong>. We sincerely apologize for any inconvenience this may have caused.</p>
                    <p>Please rest assured that your payment will be refunded as follows:</p>
                    <p><strong>{refundDetails}$</strong></p>
                    <p>If you have any concerns or questions, feel free to contact us at our support email.</p>
                    <p>Thank you for your understanding and patience.</p>
                    <p>Best regards,<br>{_nameApp} Team</p>
                </div>
            </body>
            </html>"
                };

                mail.Body = body.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                if (_useSSL)
                {
                    await smtp.ConnectAsync(_host, _port, SecureSocketOptions.SslOnConnect);
                }
                else if (_useStartTls)
                {
                    await smtp.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
                }

                await smtp.AuthenticateAsync(_username, _password);
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending retreat cancellation email", ex);
            }
        }

        public async Task SendPaymentSuccessEmail(string email, string fullName, string retreatName, DateOnly startDate, DateOnly endDate, string paymentAmount)
        {
            try
            {
                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress(_nameApp, _emailAddress));
                mail.Sender = new MailboxAddress(_nameApp, _emailAddress);
                mail.To.Add(MailboxAddress.Parse(email));

                var body = new BodyBuilder();
                mail.Subject = "Payment Successful";

                body.HtmlBody = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Payment Successful</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f7f7f7;
                        padding: 20px;
                        margin: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: auto;
                        background: white;
                        padding: 20px;
                        border: 0.5px solid #ddd;
                        border-radius: 8px;
                        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                    }}
                    h1 {{
                        color: black; 
                        text-align: center;
                    }}
                    p {{
                        font-size: 16px;
                        line-height: 1.5;
                        color: #555;
                    }}
                    .info-box {{
                        font-size: 15px;
                        font-weight: bold;
                        color: #333;
                        background-color: #e9ecef;
                        padding: 10px;
                        border-radius: 5px;
                        text-align: left;
                        margin-top: 15px;
                        list-style: none;
                    }}
                    .header {{
                        text-align: center;
                        background-color: #007BFF;
                        color: white;
                        padding: 10px 0;
                        border-radius: 8px 8px 0 0;
                    }}
                    .logo {{
                        width: 100px;
                        height: 100px;
                        border-radius: 50%;
                        display: block;
                        margin: 10px auto;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Payment Successful</h1>
                    </div>
                    <img class='logo' src='{logoSendMail}' alt='Logo'>
                    <p>Dear {fullName},</p>
                    <p>We are truly honored to welcome you to our Plum Village community. Your payment for the upcoming retreat has been successfully processed. Below are the details of your registration:</p>
                    <ul class='info-box'>
                        <li><strong>Retreat Name:</strong> {retreatName}</li>
                        <li><strong>Start Date:</strong> {startDate:dd/MM/yyyy}</li>
                        <li><strong>End Date:</strong> {endDate:dd/MM/yyyy}</li>
                        <li><strong>Payment Amount:</strong> {paymentAmount}</li>
                    </ul>
                    <p>This retreat is an opportunity to pause, breathe, and reconnect with the beauty of life. Please take this time to nourish your mind, body, and soul in the peaceful environment of Plum Village.</p>
                    <p>If you have any questions or need further assistance, please don’t hesitate to reach out to us.</p>
                    <p>With warmth and gratitude,<br>The Plum Village Team</p>
                </div>
            </body>
            </html>";

                mail.Body = body.ToMessageBody();
                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                if (_useSSL)
                {
                    await smtp.ConnectAsync(_host, _port, SecureSocketOptions.SslOnConnect);
                }
                else if (_useStartTls)
                {
                    await smtp.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
                }

                await smtp.AuthenticateAsync(_username, _password);
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending retreat cancellation email", ex);
            }
        }
    }
}
