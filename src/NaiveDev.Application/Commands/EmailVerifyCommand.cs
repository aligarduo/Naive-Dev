using System.ComponentModel.DataAnnotations;
using MediatR;
using NaiveDev.Infrastructure.Commons;

namespace NaiveDev.Application.Commands
{
    /// <summary>
    /// 发送邮箱验证码命令
    /// </summary>
    public class EmailVerifyCommand : IRequest<ResponseBody>
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "请输入您的邮箱地址")]
        [EmailAddress(ErrorMessage = "请输入一个有效的邮箱地址")]
        public required string Email { get; set; }
    }
}
