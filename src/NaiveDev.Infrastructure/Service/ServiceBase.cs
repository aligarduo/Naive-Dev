using NaiveDev.Infrastructure.Auth;

namespace NaiveDev.Infrastructure.Service
{
    /// <summary>
    /// 服务实现基类
    /// </summary>
    public abstract class ServiceBase
    {
        /// <summary>
        /// 当前请求的客户端信息
        /// </summary>
        public static UserAgent UserAgent => HttpAccessor.UserAgent;

        /// <summary>
        /// 当前请求的用户信息
        /// </summary>
        public static Accessor Accessor => HttpAccessor.Accessor;
    }
}
