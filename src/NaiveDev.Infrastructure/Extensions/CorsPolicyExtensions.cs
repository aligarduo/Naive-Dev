using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NaiveDev.Infrastructure.Settings;

namespace NaiveDev.Infrastructure.Extensions
{
    /// <summary>
    /// 包含与CORS（跨源资源共享）策略相关的扩展
    /// </summary>
    public static class CorsPolicyExtensions
    {
        /// <summary>
        /// 为<see cref="IServiceCollection"/>添加CORS策略配置
        /// </summary>
        /// <param name="services">依赖注入的服务集合</param>
        /// <returns>返回配置后的<see cref="IServiceCollection"/>实例，以便链式调用其他配置方法</returns>
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            CorsPolicySettings? corsSettings = services.BuildServiceProvider().GetRequiredService<IOptions<CorsPolicySettings>>().Value;

            services.AddCors(options =>
            {
                options.AddPolicy(corsSettings.PolicyName, builder =>
                {
                    builder.WithOrigins([.. corsSettings.Origins])
                           .WithHeaders([.. corsSettings.Headers])
                           .WithMethods([.. corsSettings.Methods]);
                });
            });

            // 返回配置后的服务集合，以便链式调用其他配置方法
            return services;
        }
    }
}