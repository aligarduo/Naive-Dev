namespace NaiveDev.Infrastructure.Settings
{
    /// <summary>
    /// 邮箱SMTP服务器设置
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// 邮箱SMTP服务器设置
        /// </summary>
        public EmailSettings()
        {
            Host = string.Empty;
            Port = 587;
            Username = string.Empty;
            Password = string.Empty;
            DisplayName = string.Empty;
        }

        /// <summary>
        /// 获取或设置SMTP服务器的主机地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 获取或设置SMTP服务器的端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 获取或设置用于SMTP身份验证的用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置用于SMTP身份验证的密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 相关联的显示名称
        /// </summary>
        public string DisplayName { get; set; }
    }
}
