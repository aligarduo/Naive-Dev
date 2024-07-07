using System.ComponentModel.DataAnnotations;
using MediatR;
using NaiveDev.Infrastructure.Commons;

namespace NaiveDev.Application.Commands
{
    /// <summary>
    /// 注册命令
    /// </summary>
    public class SignUpCommand : IRequest<ResponseBody>
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "请选择一个您喜欢的昵称")]
        [MinLength(4, ErrorMessage = "昵称至少需要4个字符")]
        [MaxLength(16, ErrorMessage = "昵称不能超过16个字符")]
        public required string NickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(6, ErrorMessage = "密码长度不能少于6个字符")]
        [MaxLength(24, ErrorMessage = "密码长度不能超过24个字符")]
        public required string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "请输入您的邮箱地址")]
        [EmailAddress(ErrorMessage = "请输入一个有效的邮箱地址")]
        public required string Email { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "请输入您的邮箱验证码")]
        [MinLength(1, ErrorMessage = "验证码长度不能少于1个字符")]
        [MaxLength(4, ErrorMessage = "验证码长度不能超过4个字符")]
        public required string Code { get; set; }
    }
}
