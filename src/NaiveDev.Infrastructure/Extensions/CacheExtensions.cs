using CSRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NaiveDev.Infrastructure.Caches;
using NaiveDev.Infrastructure.Enums;
using NaiveDev.Infrastructure.Settings;

namespace NaiveDev.Infrastructure.Extensions
{
    /// <summary>
    /// 包含与缓存相关的扩展
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// 为<see cref="IHostBuilder"/>添加缓存支持
        /// </summary>
        /// <param name="hostBuilder">当前的主机构建器实例</param>
        /// <returns>配置后的<see cref="IHostBuilder"/>实例，以便链式调用其他配置方法</returns>
        /// <exception cref="ArgumentException">当参数错误时抛出</exception>
        public static IHostBuilder AddCache(this IHostBuilder hostBuilder)
        {
            // 配置服务容器，添加缓存相关服务
            hostBuilder.ConfigureServices((buidlerContext, services) =>
            {
                // 将自定义的ICache接口实现Cache注册为单例服务
                services.AddSingleton<ICache, Cache>();

                // 从配置文件中获取Cache配置节的信息，并映射为CacheConfiguration类型对象
                CacheSettings? cacheOption = buidlerContext.Configuration.GetSection("CacheStrings").Get<CacheSettings>();

                // 没有配置信息
                if (cacheOption == null) return;

                // 根据CacheConfiguration中的CacheType属性值，决定使用哪种缓存类型
                switch (cacheOption?.CacheType)
                {
                    // 使用内存缓存
                    case CacheType.Memory:
                        // 添加内存缓存服务到服务容器中
                        services.AddDistributedMemoryCache();
                        break;
                    // 使用Redis缓存
                    case CacheType.Redis:
                        {
                            // 创建CSRedisClient实例，用于连接Redis服务器
                            CSRedisClient csredis = new(cacheOption.RedisEndpoint);

                            // 初始化RedisHelper，用于简化Redis操作
                            RedisHelper.Initialization(csredis);

                            // 将CSRedisClient实例注册为单例服务，方便在应用程序中使用
                            services.AddSingleton(csredis);

                            // 使用CSRedisCache实现IDistributedCache接口，并注册为单例服务
                            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
                        };
                        break;
                    // 如果CacheType无效，则抛出异常
                    default: throw new ArgumentException("Invalid CacheType");
                }
            });

            // 返回配置后的IHostBuilder实例，以便链式调用其他配置方法
            return hostBuilder;
        }
    }
}
