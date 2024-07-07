using System.ComponentModel.DataAnnotations;
using MediatR;
using NaiveDev.Application.Dtos;
using NaiveDev.Infrastructure.Commons;

namespace NaiveDev.Application.Commands
{
    /// <summary>
    /// 登录命令
    /// </summary>
    public class SignInCommand : IRequest<ResponseBody<SignInResponseDto>>
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "账号不可为空")]
        [MinLength(1, ErrorMessage = "输入的账号至少需要1个字符")]
        [MaxLength(20, ErrorMessage = "输入的账号超出长度限制")]
        public required string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(6, ErrorMessage = "密码长度不能少于6个字符")]
        [MaxLength(24, ErrorMessage = "密码长度不能超过24个字符")]
        public required string Password { get; set; }
    }
}
