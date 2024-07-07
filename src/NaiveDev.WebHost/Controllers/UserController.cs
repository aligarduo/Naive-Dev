using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaiveDev.Application.Dtos;
using NaiveDev.Application.Queries;
using NaiveDev.Infrastructure.Attributes;
using NaiveDev.Infrastructure.Commons;

namespace NaiveDev.WebHost.Controllers
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [VersionRoute(VersionAttribute.v1)]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// 当前登录的用户信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("current")]
        public async Task<ResponseBody<GetCurrentUserInfoDto>> UserInfoAsync()
        {
            return await _mediator.Send(new GetCurrentUserInfoQuery());
        }
    }
}
