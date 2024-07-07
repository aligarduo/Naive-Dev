namespace NaiveDev.Infrastructure.Enums
{
    /// <summary>
    /// 缓存的类型
    /// </summary>
    /// <remarks>
    /// 包括内存缓存和Redis缓存两种类型
    /// <list type="bullet">
    /// <item>
    /// <term>内存缓存</term>
    /// <description>使用内存缓存，不支持分布式</description>
    /// </item>
    /// <item>
    /// <term>Redis缓存</term>
    /// <description>使用Redis缓存，支持分布式</description>
    /// </item>
    /// </list>
    /// </remarks>
    public enum CacheType
    {
        /// <summary>
        /// 使用内存缓存，不支持分布式 
        /// </summary>
        Memory,

        /// <summary>
        /// 使用Redis缓存，支持分布式
        /// </summary>
        Redis
    }
}
