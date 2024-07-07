using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NaiveDev.Infrastructure.Behavior;

namespace NaiveDev.Infrastructure.Extensions
{
    /// <summary>
    /// 包含与MediatR相关的扩展
    /// </summary>
    public static class MediatRExtensions
    {
        /// <summary>
        /// 为<see cref="IServiceCollection"/>添加MediatR支持
        /// </summary>
        /// <param name="services">依赖注入的服务集合</param>
        /// <returns>返回配置后的<see cref="IServiceCollection"/>实例，以便链式调用其他配置方法</returns>
        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            // 过滤出所有包含非抽象、非接口的IBaseRequest继承类的程序集
            Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetTypes().Any(t => typeof(IBaseRequest).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface))
                .Distinct()
                .ToArray();

            // 如果没有找到任何含有IBaseRequest子类型的程序集
            if (allAssemblies.Length == 0)
                return services;

            // 注册MediatR及其依赖注入支持
            // 这里使用了一个lambda表达式来配置MediatR，使其能够扫描finalAssemblies中的程序集，并自动注册相应的请求处理器和服务
            services.AddMediatR((config) =>
            {
                config.RegisterServicesFromAssemblies(allAssemblies);
            });

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            // 返回配置后的服务集合，以便链式调用其他配置方法
            return services;
        }
    }
}
