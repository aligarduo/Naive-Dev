using Microsoft.Extensions.DependencyInjection;
using NaiveDev.Infrastructure.DataBase;

namespace NaiveDev.Infrastructure.Extensions
{
    /// <summary>
    /// 包含与仓储相关的扩展
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// 为<see cref="IServiceCollection"/>添加仓储支持
        /// </summary>
        /// <param name="services">依赖注入的服务集合</param>
        /// <returns>返回配置后的<see cref="IServiceCollection"/>实例，以便链式调用其他配置方法</returns>
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            // 返回配置后的服务集合，以便链式调用其他配置方法
            return services;
        }
    }
}
