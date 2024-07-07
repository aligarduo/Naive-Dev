using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace NaiveDev.Infrastructure.Extensions
{
    /// <summary>
    /// 包含与AutoMapper相关的扩展
    /// </summary>
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// 为<see cref="IServiceCollection"/>添加AutoMapper支持，查找并注册继承自<see cref="Profile"/>的类作为映射配置
        /// </summary>
        /// <param name="services">依赖注入的服务集合</param>
        /// <returns>返回配置后的<see cref="IServiceCollection"/>实例，以便链式调用其他配置方法</returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            // 查找当前应用程序域中所有程序集中继承自Profile的类
            Type[] allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(Profile)))
                .Distinct()
                .ToArray();

            // 使用找到的Profile类型配置AutoMapper，并注册到服务集合中
            services.AddAutoMapper(allTypes);

            // 返回配置后的服务集合，以便链式调用其他配置方法
            return services;
        }
    }
}
