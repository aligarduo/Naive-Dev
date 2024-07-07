using MediatR;
using NaiveDev.Application.Dtos;
using NaiveDev.Infrastructure.Commons;

namespace NaiveDev.Application.Queries
{
    /// <summary>
    /// 当前登录的用户信息
    /// </summary>
    public class GetCurrentUserInfoQuery : IRequest<ResponseBody<GetCurrentUserInfoDto>>
    {

    }
}
