namespace NaiveDev.Infrastructure.Auth
{
    /// <summary>
    /// 访问器
    /// </summary>
    /// <remarks>
    /// 记录当前请求的用户信息
    /// </remarks>
    public class Accessor
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Id { get; init; }

        /// <summary>
        /// 账号
        /// </summary>
        public required string Account { get; init; }

        /// <summary>
        /// 用户名
        /// </summary>
        public required string Name { get; init; }
    }
}
