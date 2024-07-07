using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaiveDev.Application.Commands;
using NaiveDev.Application.Dtos;
using NaiveDev.Infrastructure.Attributes;
using NaiveDev.Infrastructure.Commons;

namespace NaiveDev.WebHost.Controllers
{
    /// <summary>
    /// 授权认证
    /// </summary>
    [VersionRoute(VersionAttribute.v1)]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// 发送邮箱验证码
        /// </summary>
        /// <param name="command">发送邮箱验证码命令</param>
        /// <returns></returns>
        [HttpPost("email/verify")]
        public async Task<ResponseBody> EmailVerifyAsync([FromBody] EmailVerifyCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="command">注册命令</param>
        /// <returns></returns>
        [HttpPost("signup")]
        public async Task<ResponseBody> SignUpAsync([FromBody] SignUpCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="command">登录命令</param>
        /// <returns></returns>
        [HttpPost("signin")]
        public async Task<ResponseBody<SignInResponseDto>> SignInAsync([FromBody] SignInCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// 续约
        /// </summary>
        /// <param name="command">续约命令</param>
        /// <returns></returns>
        [HttpPost("renewal")]
        public async Task<ResponseBody<RenewalResponseDto>> RenewalAsync([FromBody] RenewalCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("signout")]
        public async Task<ResponseBody> SignOutAsync()
        {
            return await _mediator.Send(new SignOutCommand());
        }
    }
}
