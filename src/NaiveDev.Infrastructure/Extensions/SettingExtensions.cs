using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NaiveDev.Infrastructure.Settings;

namespace NaiveDev.Infrastructure.Extensions
{
    /// <summary>
    /// 包含与自定义设置相关的扩展
    /// </summary>
    public static class SettingExtensions
    {
        /// <summary>
        /// 为<see cref="IServiceCollection"/>添加自定义设置
        /// </summary>
        /// <param name="services">依赖注入的服务集合</param>
        /// <param name="configuration">应用程序的配置根对象</param>
        /// <returns>返回配置后的<see cref="IServiceCollection"/>实例，以便链式调用其他配置方法</returns>
        public static IServiceCollection AddSetting(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OpenApiSettings>(configuration.GetSection("OpenApiStrings"));
            services.Configure<CorsPolicySettings>(configuration.GetSection("CorsPolicyStrings"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            return services;
        }
    }
}
