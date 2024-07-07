using System.Text.RegularExpressions;
using NaiveDev.Shared.Domain;

namespace NaiveDev.Domain.ValueObjects
{
    /// <summary>
    /// 邮箱值对象
    /// </summary>
    public class EmailAddress : ValueObjectBase
    {
        /// <summary>
        /// 邮箱地址的字符串表示
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// 静态正则表达式，用于验证邮箱地址的格式
        /// </summary>
        private static readonly Regex EmailRegex = new(@"^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(com|cn|net)$");

        /// <summary>
        /// 初始化邮箱值对象
        /// </summary>
        /// <param name="address">要验证的邮箱地址</param>
        /// <exception cref="ArgumentException">当提供的邮箱地址无效时抛出</exception>
        public EmailAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                // 如果地址为空或只包含空白字符，则使用空字符串作为地址
                Address = string.Empty;
            }
            else if (!IsValidEmail(address))
            {
                // 如果地址不符合邮箱格式，则抛出异常
                throw new ArgumentException("Invalid email address.", nameof(address));
            }
            else
            {
                // 如果地址有效，则设置地址属性
                Address = address;
            }
        }

        /// <summary>
        /// 验证给定的字符串是否符合邮箱地址的格式
        /// </summary>
        /// <param name="email">要验证的邮箱地址字符串</param>
        /// <returns>如果字符串符合邮箱地址格式，则返回true；否则返回false</returns>
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            // 使用正则表达式验证邮箱地址
            return EmailRegex.IsMatch(email);
        }

        /// <summary>
        /// 判断当前实例是否与指定的对象相等
        /// </summary>
        /// <param name="obj">要与当前实例进行比较的对象</param>
        /// <returns>如果两个对象相等，则返回true；否则返回false</returns>
        public override bool Equals(object? obj)
        {
            // 检查是否为EmailAddress类型，并且地址相等（不区分大小写）
            if (obj is EmailAddress other)
            {
                return Address.Equals(other.Address, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// 获取当前实例的哈希码
        /// </summary>
        /// <returns>表示当前实例的哈希码</returns>
        public override int GetHashCode()
        {
            // 使用地址字符串（不区分大小写）计算哈希码
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Address);
        }

        /// <summary>
        /// 返回表示当前实例的字符串
        /// </summary>
        /// <returns>表示当前实例的字符串，即邮箱地址</returns>
        public override string ToString()
        {
            return Address;
        }
    }
}
