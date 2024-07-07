using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NaiveDev.Application.Dtos;
using NaiveDev.Application.Queries;
using NaiveDev.Domain.Entities;
using NaiveDev.Domain.Enums;
using NaiveDev.Infrastructure.Commons;
using NaiveDev.Infrastructure.DataBase;
using NaiveDev.Infrastructure.Service;

namespace NaiveDev.Application.QueryHandlers
{
    /// <summary>
    /// 用户查询处理器
    /// </summary>
    public class UserQueryHandlers(IMapper mapper, IRepository<User> user) : ServiceBase,
        IRequestHandler<GetCurrentUserInfoQuery, ResponseBody<GetCurrentUserInfoDto>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IRepository<User> _user = user;

        /// <summary>
        /// 当前登录的用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseBody<GetCurrentUserInfoDto>> Handle(GetCurrentUserInfoQuery request, CancellationToken cancellationToken)
        {
            User? user = await _user.NoTrackingQuery().FirstOrDefaultAsync(q => q.Id == Accessor.Id && q.Status == AccountStatus.Active, cancellationToken);
            if (user == null)
            {
                return ResponseBody<GetCurrentUserInfoDto>.Fail(ResponseCode.Forbidden);
            }

            var userDto = _mapper.Map<GetCurrentUserInfoDto>(user);

            return ResponseBody<GetCurrentUserInfoDto>.Succeed(userDto);
        }
    }
}
