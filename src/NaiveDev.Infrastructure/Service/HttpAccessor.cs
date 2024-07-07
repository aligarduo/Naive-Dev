using NaiveDev.Infrastructure.Auth;

namespace NaiveDev.Infrastructure.Service
{
    /// <summary>
    /// HTTP访问器
    /// </summary>
    /// <remarks>
    /// 用于在异步上下文中存储和获取请求相关的信息
    /// </remarks>
    public class HttpAccessor
    {
        /// <summary>
        /// 用于存储<see cref="AccessorHolder"/>实例的异步本地变量
        /// </summary>
        private readonly static AsyncLocal<AccessorHolder> _accessorAsyncLocal = new();

        /// <summary>
        /// 用于存储<see cref="UserAgentHolder"/>实例的异步本地变量
        /// </summary>
        private readonly static AsyncLocal<UserAgentHolder> _userAgentAsyncLocal = new();

        /// <summary>
        /// 辅助类，用于存储<see cref="Accessor"/>实例
        /// </summary>
        /// <remarks>
        /// 通过这个类可以管理<see cref="Accessor"/>的访问和存储，在异步上下文中使用
        /// </remarks>
        private class AccessorHolder
        {
            public Accessor? Accessor;
        }

        /// <summary>
        /// 辅助类，用于存储<see cref="UserAgent"/>实例
        /// </summary>
        /// <remarks>
        /// 通过这个类可以管理<see cref="UserAgent"/>的访问和存储，在异步上下文中使用
        /// </remarks>
        private class UserAgentHolder
        {
            public UserAgent? UserAgent { get; set; }
        }

        /// <summary>
        /// 获取或设置当前请求用户的信息
        /// </summary>
        public static Accessor Accessor
        {
            get
            {
                if (_accessorAsyncLocal.Value?.Accessor == null)
                {
                    return new Accessor() { Account = string.Empty, Name = string.Empty };
                }

                return _accessorAsyncLocal.Value.Accessor;
            }
            private set
            {
                if (_accessorAsyncLocal.Value != null)
                {
                    _accessorAsyncLocal.Value.Accessor = null;
                }

                _accessorAsyncLocal.Value = new AccessorHolder { Accessor = value };
            }
        }

        /// <summary>
        /// 获取或设置当前的客户端标识
        /// </summary>
        public static UserAgent UserAgent
        {
            get
            {
                if (_userAgentAsyncLocal.Value?.UserAgent == null)
                {
                    return new UserAgent() { };
                }

                return _userAgentAsyncLocal.Value.UserAgent;
            }
            private set
            {
                if (_userAgentAsyncLocal.Value != null)
                {
                    _userAgentAsyncLocal.Value.UserAgent = null;
                }

                _userAgentAsyncLocal.Value = new UserAgentHolder { UserAgent = value };
            }
        }

        /// <summary>
        /// 设置<see cref="Accessor"/>属性
        /// </summary>
        /// <param name="accessor">要设置的<see cref="Accessor"/>实例</param>
        public static void SetAccessor(Accessor accessor)
        {
            if (accessor != null)
            {
                Accessor = accessor;
            }
        }

        /// <summary>
        /// 设置<see cref="UserAgent"/>属性
        /// </summary>
        /// <param name="userAgent">要设置的<see cref="UserAgent"/>实例</param>
        public static void SetUserAgent(UserAgent userAgent)
        {
            if (userAgent != null)
            {
                UserAgent = userAgent;
            }
        }
    }
}
