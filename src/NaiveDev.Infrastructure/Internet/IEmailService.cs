using NaiveDev.Infrastructure.Service;

namespace NaiveDev.Infrastructure.Internet
{
    /// <summary>
    /// 电子邮件服务接口，提供发送电子邮件的基本功能
    /// </summary>
    public interface IEmailService : ITransientDependency
    {
        /// <summary>
        /// 异步发送电子邮件
        /// </summary>
        /// <param name="address">收件人的电子邮件地址</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
        /// <returns>一个表示异步操作的任务</returns>
        Task SendEmailAsync(string address, string subject, string body, CancellationToken cancellationToken = default);
    }
}
