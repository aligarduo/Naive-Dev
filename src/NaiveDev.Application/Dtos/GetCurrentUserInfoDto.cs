using NaiveDev.Infrastructure.Attributes;

namespace NaiveDev.Application.Dtos
{
    /// <summary>
    /// 当前登录的用户信息
    /// </summary>
    public class GetCurrentUserInfoDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string? Account { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [DataMask(MaskMethod.Name)]
        public string? NickName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DataMask(MaskMethod.Mail)]
        public string? Email { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [DataMask(MaskMethod.Mobile)]
        public string? Mobile { get; set; }
    }
}
