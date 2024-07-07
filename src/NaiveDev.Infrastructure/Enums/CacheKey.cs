using System.ComponentModel;

namespace NaiveDev.Infrastructure.Enums
{
    /// <summary>
    /// 缓存的键
    /// </summary>
    public enum CacheKey
    {
        /// <summary>
        /// 邮箱验证码
        /// </summary>
        [Description("email_verify_code")]
        EmailVerifyCode,
    }
}
