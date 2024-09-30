using MailKit.Security;
using MCSM_Service.Interfaces;
using MCSM_Utility.Settings;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MCSM_Service.Implementations
{
    public class SendMailService : ISendMailService
    {
        private readonly string _nameApp;
        private readonly string _emailAddress;
        private readonly bool _useSSL;
        private readonly string _host;
        private readonly int _port;
        private readonly bool _useStartTls;
        private readonly string _username;
        private readonly string _password;
        private readonly static CancellationToken ct = new CancellationToken();

        public SendMailService(IOptions<AppSetting> appSettings)
        {
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
            var verificationLink = $"https://localhost:7235/api/accounts/verification/{token}";

            try
            {
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress(_nameApp, _emailAddress));
                mail.Sender = new MailboxAddress(_nameApp, _emailAddress);
                mail.To.Add(MailboxAddress.Parse(userEmail));

                var body = new BodyBuilder();
                mail.Subject = "Email Verification";

                // Mẫu HTML cho email với hình ảnh và màu sắc
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
                        <img class='logo' src='https://firebasestorage.googleapis.com/v0/b/mcsm-fa24se115.appspot.com/o/images%2F78c91fd1-0933-4f8e-83de-6d437f718018?alt=media&token=bc190fe2-2e28-43ac-955d-4c10398fab9c' alt='Logo' /> <!-- Thay đổi đường dẫn đến logo của bạn -->
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



    }
}
