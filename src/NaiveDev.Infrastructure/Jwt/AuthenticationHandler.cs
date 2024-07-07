using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NaiveDev.Infrastructure.Commons;

namespace NaiveDev.Infrastructure.Jwt
{
    /// <summary>
    /// 认证处理程序的构造函数，用于初始化认证处理程序并注入必要的依赖项
    /// </summary>
    /// <param name="options">认证方案选项的监视器，用于获取和监听认证方案选项的更改</param>
    /// <param name="logger">日志记录器工厂，用于创建日志记录器实例，用于记录认证过程中的日志信息</param>
    /// <param name="encoder">URL编码器，用于对URL中的特殊字符进行编码，以确保其正确性和安全性</param>
    public class AuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        /// <summary>
        /// 重写异步方法，处理身份验证过程
        /// </summary>
        /// <returns>身份验证结果</returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 重写处理身份验证的异步方法
        /// </summary>
        /// <remarks>
        /// 当需要用户进行身份验证时，会调用此方法
        /// </remarks>
        /// <param name="properties">包含有关身份验证挑战的附加信息的属性对象</param> 
        /// <returns>异步任务</returns>  
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await ResponseContext.ImmediateReturn(Response, ResponseCode.Unauthorized);
        }

        /// <summary>
        /// 重写处理禁止访问请求的异步方法
        /// </summary>
        /// <remarks>
        /// 当用户没有权限访问受保护的资源时，会调用此方法
        /// </remarks>
        /// <param name="properties">包含有关认证请求的附加信息的属性对象</param>
        /// <returns>异步任务</returns>
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            await ResponseContext.ImmediateReturn(Response, ResponseCode.Forbidden);
        }
    }
}