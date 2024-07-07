using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NaiveDev.Infrastructure.Service;
using NaiveDev.Infrastructure.Settings;

namespace NaiveDev.Infrastructure.Internet
{
    /// <summary>
    /// 电子邮件服务实现类
    /// </summary>
    public class EmailService(IOptions<EmailSettings> email, ILogger<EmailService> logger) : ServiceBase, IEmailService
    {
        private readonly EmailSettings _email = email.Value;
        private readonly ILogger<EmailService> _logger = logger;

        /// <summary>
        /// 异步发送电子邮件
        /// </summary>
        /// <param name="address">收件人的电子邮件地址</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
        /// <returns>一个表示异步操作的任务</returns>
        public async Task SendEmailAsync(string address, string subject, string body, CancellationToken cancellationToken = default)
        {
            using SmtpClient mailclient = new(_email.Host, _email.Port);
            mailclient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mailclient.Credentials = new NetworkCredential(_email.Username, _email.Password);

            MailMessage message = new()
            {
                From = new MailAddress(_email.Username),
                SubjectEncoding = Encoding.UTF8,
                Subject = subject,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Priority = MailPriority.High
            };

            message.To.Add(new MailAddress(address, _email.DisplayName, Encoding.UTF8));

            try
            {
                await mailclient.SendMailAsync(message, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送电子邮件时发生错误");
            }
        }
    }
}
