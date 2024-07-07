namespace NaiveDev.Infrastructure.Enums
{
    /// <summary>
    /// 缓存的过期类型
    /// </summary>
    /// <remarks>
    /// 包括绝对过期和相对过期两种类型
    /// <list type="bullet">
    /// <item>
    /// <term>绝对过期</term>
    /// <description>缓存项在创建后经过指定时间段即过期</description>
    /// </item>
    /// <item>
    /// <term>相对过期</term>
    /// <description>缓存项在最后一次访问后经过指定时间段未再次被访问即过期，若持续被访问则过期时间自动延长</description>
    /// </item>
    /// </list>
    /// </remarks>
    public enum CacheExpire
    {
        /// <summary>
        /// 绝对过期
        /// </summary>
        Absolute,

        /// <summary>
        /// 相对过期
        /// </summary>
        Relative
    }
}
