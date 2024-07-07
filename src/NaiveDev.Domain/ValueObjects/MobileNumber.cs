using System.Text.RegularExpressions;
using NaiveDev.Shared.Domain;

namespace NaiveDev.Domain.ValueObjects
{
    /// <summary>
    /// 手机号值对象
    /// </summary>
    public class MobileNumber : ValueObjectBase
    {
        /// <summary>
        /// 存储手机号的字符串表示
        /// </summary>
        public string Number { get; }

        /// <summary>
        /// 用于验证手机号的正则表达式匹配以1开头，第二位为3-9的数字，后续跟9位数字的字符串
        /// </summary>
        private static readonly Regex PhoneRegex = new(@"^1[3-9]\d{9}$");

        /// <summary>
        /// 初始化手机号值对象
        /// </summary>
        /// <param name="number">要验证的手机号码</param>
        /// <exception cref="ArgumentException">当提供的手机号码无效时抛出</exception>
        public MobileNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                // 如果手机号码为空或只包含空白字符，则使用空字符串作为手机号
                Number = string.Empty;
            }
            else if (!IsValidPhone(number))
            {
                // 如果手机号码不符合格式要求，则抛出异常
                throw new ArgumentException("Invalid mobile number.", nameof(number));
            }
            else
            {
                // 如果手机号码有效，则设置手机号属性
                Number = number;
            }
        }

        /// <summary>
        /// 验证给定的字符串是否符合手机号码的格式
        /// </summary>
        /// <param name="mobile">要验证的手机号码字符串</param>
        /// <returns>如果字符串符合手机号码格式，则返回true；否则返回false</returns>
        private static bool IsValidPhone(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                return false;
            }

            // 使用正则表达式验证手机号码
            return PhoneRegex.IsMatch(mobile);
        }

        /// <summary>
        /// 判断当前实例是否与指定的对象相等
        /// </summary>
        /// <param name="obj">要与当前实例进行比较的对象</param>
        /// <returns>如果两个对象相等，则返回true；否则返回false</returns>
        public override bool Equals(object? obj)
        {
            // 检查是否为MobileNumber类型，并且手机号相等（注意：手机号码不区分大小写，但这里仍然保持原样以明确意图）
            if (obj is MobileNumber other)
            {
                return Number.Equals(other.Number, StringComparison.Ordinal); // 手机号码通常不区分大小写，所以这里使用Ordinal比较
            }

            return false;
        }

        /// <summary>
        /// 获取当前实例的哈希码
        /// </summary>
        /// <returns>表示当前实例的哈希码</returns>
        public override int GetHashCode()
        {
            // 使用手机号字符串计算哈希码（不区分大小写，但手机号码通常不区分大小写，所以这里保持原样）
            return StringComparer.Ordinal.GetHashCode(Number);
        }

        /// <summary>
        /// 返回表示当前实例的字符串
        /// </summary>
        /// <returns>表示当前实例的字符串，即手机号码</returns>
        public override string ToString()
        {
            return Number;
        }
    }
}
