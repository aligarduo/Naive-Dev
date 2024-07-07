using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using NaiveDev.Infrastructure.Auth;
using NaiveDev.Infrastructure.Caches;
using NaiveDev.Infrastructure.Commons;
using NaiveDev.Infrastructure.Enums;
using NaiveDev.Infrastructure.Service;
using NaiveDev.Infrastructure.Tools;
using NaiveDev.Shared.Tools;

namespace NaiveDev.Infrastructure.Middleware
{
    /// <summary>
    /// 缓存当前请求的用户信息中间件
    /// </summary>
    public class AccessorMiddleware(RequestDelegate next, ICache cache)
    {
        /// <summary>
        /// 下一个请求处理委托
        /// </summary>
        private readonly RequestDelegate _next = next;

        /// <summary>
        /// 缓存服务的实例
        /// </summary>
        private readonly ICache _cache = cache;

        /// <summary>
        /// 当请求到达时，会尝试从令牌中获取信息，并将信息附加到请求上下文中
        /// </summary>
        /// <param name="context">HTTP上下文，包含请求和响应信息</param>
        /// <returns>一个表示异步操作的任务</returns>
        public async Task Invoke(HttpContext context)
        {
            string? userAgent = context.Request.Headers.UserAgent;
            UserAgent _userAgent = UserAgentUtils.GetDeviceType(userAgent);
            HttpAccessor.SetUserAgent(_userAgent);

            Endpoint? endpoint = context.GetEndpoint();
            AuthorizeAttribute? authAttribute = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();
            if (endpoint != null && authAttribute != null)
            {
                // 提取令牌中的声明信息
                List<Claim> claimList = context.User.Claims.ToList();

                // 尝试从声明列表中获取字段的值
                string? type = claimList?.Where(q => q.Type == "type").FirstOrDefault()?.Value;
                string? client = claimList?.Where(q => q.Type == "client").FirstOrDefault()?.Value;
                string? account = claimList?.Where(q => q.Type == "account").FirstOrDefault()?.Value;
                string? csrf = claimList?.Where(q => q.Type == "csrf").FirstOrDefault()?.Value;

                if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(client) || string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(csrf))
                {
                    await ResponseContext.ImmediateReturn(context, ResponseCode.Unauthorized);
                    return;
                }

                if (type != JwtType.AccessToken.GetDescription())
                {
                    await ResponseContext.ImmediateReturn(context, ResponseCode.Unauthorized);
                    return;
                }

                string accessKey = CacheKeyUtils.TokenFormat(JwtType.AccessToken, client, account);
                string userString = await _cache.GetCacheAsync(accessKey);
                if (string.IsNullOrWhiteSpace(userString))
                {
                    await ResponseContext.ImmediateReturn(context, ResponseCode.Unauthorized);
                    return;
                }

                string activeKey = CacheKeyUtils.ActiveFormat(_userAgent.Device.Brand, account);
                string activeString = await _cache.GetCacheAsync(activeKey);
                if (activeString != csrf)
                {
                    await ResponseContext.ImmediateReturn(context, ResponseCode.Unauthorized);
                    return;
                }

                Accessor? accessor = userString.ToObject<Accessor>();
                if (accessor != null)
                {
                    HttpAccessor.SetAccessor(accessor);
                }
            }

            await _next(context);
        }
    }
}
