using System.ComponentModel;

namespace NaiveDev.Infrastructure.Enums
{
    /// <summary>
    /// Jwt类型
    /// </summary>
    /// <remarks>
    /// 包括访问令牌和刷新令牌两种类型
    /// <list type="bullet">
    /// <item>
    /// <term>访问令牌</term>
    /// <description>表示用户用于访问资源的令牌</description>
    /// </item>
    /// <item>
    /// <term>刷新令牌</term>
    /// <description>用于获取新的访问令牌的长寿命令牌，通常在访问令牌过期后使用</description>
    /// </item>
    /// </list>
    /// </remarks>
    public enum JwtType
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        [Description("access_token")]
        AccessToken,

        /// <summary>
        /// 刷新令牌
        /// </summary>
        [Description("refresh_token")]
        RefreshToken
    }
}
