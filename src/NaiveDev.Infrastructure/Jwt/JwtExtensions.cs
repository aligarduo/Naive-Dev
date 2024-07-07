using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace NaiveDev.Infrastructure.Jwt
{
    /// <summary>
    /// 包含与JWT相关的扩展
    /// </summary>
    public static class JwtExtensions
    {
        /// <summary>
        /// 为<see cref="IServiceCollection"/>添加Jwt鉴权
        /// </summary>
        /// <param name="services">依赖注入的服务集合</param>
        /// <returns>返回配置后的<see cref="IServiceCollection"/>实例，以便链式调用其他配置方法</returns>
        public static IServiceCollection AddJwt(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = nameof(AuthenticationHandler);
                options.DefaultForbidScheme = nameof(AuthenticationHandler);
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtOption.GetDefaultTokenValidationParameters();
            }).AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>(nameof(AuthenticationHandler), options => { });

            return services;
        }
    }
}