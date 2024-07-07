using NaiveDev.Infrastructure.Enums;

namespace NaiveDev.Infrastructure.Settings
{
    /// <summary>
    /// 缓存设置
    /// </summary>
    public class CacheSettings
    {
        /// <summary>
        /// 缓存设置
        /// </summary>
        public CacheSettings()
        {
            CacheType = CacheType.Memory;
            RedisEndpoint = string.Empty;
        }

        /// <summary>
        /// Memory OR Redis
        /// </summary>
        public CacheType CacheType { get; set; }

        /// <summary>
        /// Redis节点地址
        /// </summary>
        public string RedisEndpoint { get; set; }
    }
}
