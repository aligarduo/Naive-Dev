using System.ComponentModel.DataAnnotations;
using MediatR;
using NaiveDev.Application.Dtos;
using NaiveDev.Infrastructure.Commons;

namespace NaiveDev.Application.Commands
{
    /// <summary>
    /// 续约命令
    /// </summary>
    public class RenewalCommand : IRequest<ResponseBody<RenewalResponseDto>>
    {
        /// <summary>
        /// 续签Token
        /// </summary>
        [Required(ErrorMessage = "RefreshToken参数缺失")]
        public required string RefreshToken { get; set; }
    }
}
