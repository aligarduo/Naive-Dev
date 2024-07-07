using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using NaiveDev.Domain.Enums;
using NaiveDev.Domain.ValueObjects;
using NaiveDev.Shared.Domain;
using NaiveDev.Shared.Tools;

namespace NaiveDev.Domain.Entities
{
    /// <summary>
    /// 用户领域实体
    /// </summary>
    public class User : EntityBase
    {
        /// <summary>
        /// <see href="https://learn.microsoft.com/zh-cn/ef/core/modeling/constructors"/>
        /// </summary>
        protected User() { }

        /// <summary>
        /// 私有构造函数，用于创建新的用户实例
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="nickName">昵称</param>
        /// <param name="password">密码</param>
        /// <param name="email">邮箱</param>
        /// <param name="mobile">手机号</param>
        private User(string account, string nickName, string password, EmailAddress email, MobileNumber mobile) : base()
        {
            Account = account;
            NickName = nickName;
            Email = email;
            Mobile = mobile;
            Status = AccountStatus.Active;

            SetPassword(password);
        }

        /// <summary>
        /// 账号
        /// </summary>
        public string? Account { get; private set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string? NickName { get; private set; }

        /// <summary>
        /// 密码哈希
        /// </summary>
        public string? PasswordHash { get; private set; }

        /// <summary>
        /// 密码盐
        /// </summary>
        public string? PasswordSalt { get; private set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public EmailAddress? Email { get; private set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public MobileNumber? Mobile { get; private set; }

        /// <summary>
        /// 账号状态
        /// </summary>
        public AccountStatus? Status { get; private set; }

        /// <summary>
        /// 生成一个密码盐
        /// </summary>
        private static string GenerateSalt()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] saltValue = new byte[16];
            rng.GetBytes(saltValue);

            return Convert.ToBase64String(saltValue);
        }

        /// <summary>
        /// 将密码哈希
        /// </summary>
        private static string HashPassword(string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("密码不能为空、空或空格", nameof(password));
            }

            // 将盐值从字符串转换为字节数组
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            // 使用盐值哈希密码
            byte[] hashedBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(hashedBytes);
        }

        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="nickName">昵称</param>
        /// <param name="password">密码</param>
        /// <param name="emailAddress">电子邮箱</param>
        /// <param name="phoneNumber">电话号码</param>
        /// <returns>一个新的<see cref="User"/>实例</returns>
        public static User Create(string nickName, string password, string? emailAddress = null, string? phoneNumber = null)
        {
            EmailAddress _emailAddress = string.IsNullOrWhiteSpace(emailAddress) ? new EmailAddress(string.Empty) : new EmailAddress(emailAddress);
            MobileNumber _phoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? new MobileNumber(string.Empty) : new MobileNumber(phoneNumber);

            if (string.IsNullOrWhiteSpace(_emailAddress.Address) && string.IsNullOrWhiteSpace(_phoneNumber.Number))
            {
                throw new Exception("Email address or phone number must be included");
            }

            return new User(ShortGuidIdHelper.NextId(), nickName, password, _emailAddress, _phoneNumber);
        }

        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="password">>密码</param>
        public void SetPassword(string password)
        {
            PasswordSalt = GenerateSalt();

            PasswordHash = HashPassword(password, PasswordSalt);
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>验证结果</returns>
        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(PasswordSalt))
            {
                return false;
            }

            string hashedAttempt = HashPassword(password, PasswordSalt);

            return hashedAttempt == PasswordHash;
        }

        /// <summary>
        /// 更新昵称
        /// </summary>
        /// <param name="nickName">昵称</param>
        public void ChangeName(string nickName)
        {
            NickName = nickName;
        }
    }
}
