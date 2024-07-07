using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NaiveDev.Infrastructure.Settings;

namespace NaiveDev.Infrastructure.Middleware
{
    /// <summary>
    /// CORS策略中间件配置扩展类
    /// </summary>
    /// <remarks>
    /// 为应用程序提供了使用自定义CORS策略的配置扩展方法
    /// </remarks>
    public static class CorsPolicyMiddleware
    {
        /// <summary>
        /// 为应用程序添加CORS策略中间件
        /// 该方法从应用程序的服务容器中检索CORS策略设置，并使用指定的策略名来配置CORS中间件
        /// 如果服务容器中不存在CORS策略设置或策略名为null，则不配置CORS中间件
        /// </summary>
        /// <param name="app">应用程序构建器实例</param>
        /// <returns>配置有CORS策略的应用程序构建器实例</returns>
        public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
        {
            CorsPolicySettings? corsSettings = app.ApplicationServices.GetService<IOptions<CorsPolicySettings>>()?.Value;

            if (corsSettings != null)
            {
                app.UseCors(corsSettings.PolicyName);
            }

            return app;
        }
    }
}
