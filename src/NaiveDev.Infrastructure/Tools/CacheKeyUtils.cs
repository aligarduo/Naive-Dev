using System.Text;
using NaiveDev.Infrastructure.Enums;
using NaiveDev.Shared.Tools;

namespace NaiveDev.Infrastructure.Tools
{
    /// <summary>
    /// 缓存键工具类，用于生成和格式化缓存键的字符串表示
    /// </summary>
    public class CacheKeyUtils
    {
        /// <summary>
        /// 格式化缓存键并生成字符串表示
        /// </summary>
        /// <param name="cacheKey">待格式化的缓存键对象</param>
        /// <param name="param">用于构造缓存键的额外参数</param>
        /// <returns>返回格式化后的缓存键字符串</returns>
        public static string Format(CacheKey cacheKey, params string[] param)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(cacheKey.GetDescription());

            foreach (var key in param)
            {
                sb.Append(':');
                sb.Append(key);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 格式化Token缓存键并生成字符串表示
        /// </summary>
        /// <param name="jwtType">令牌类型</param>
        /// <param name="client">客户端</param>
        /// <param name="account">账号</param>>
        /// <returns>返回格式化后的Token缓存键字符串</returns>
        public static string TokenFormat(JwtType jwtType, string client, string account)
        {
            return $"{client}:{account}:{jwtType.GetDescription()}";
        }

        /// <summary>
        /// 格式化Active缓存键并生成字符串表示
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="account">账号</param>
        /// <returns>返回格式化后的Token缓存键字符串</returns>
        public static string ActiveFormat(string client, string account)
        {
            return $"{client}:{account}:active";
        }
    }
}
