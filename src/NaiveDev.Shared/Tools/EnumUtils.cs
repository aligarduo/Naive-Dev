using System.ComponentModel;
using System.Reflection;

namespace NaiveDev.Shared.Tools
{
    /// <summary>
    /// 枚举工具类
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// 获取枚举值的描述
        /// </summary>
        /// <remarks>
        /// 如果枚举值上定义了<see cref="DescriptionAttribute"/>特性，则返回该特性的描述；否则返回枚举值的名称
        /// </remarks>
        /// <param name="value">要获取描述的枚举值</param>
        /// <returns>枚举值的描述，如果未找到则返回枚举值的名称</returns>
        /// <exception cref="ArgumentNullException">如果<paramref name="value"/>为null</exception>
        public static string GetDescription(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "Enum value cannot be null.");
            }

            FieldInfo? field = value.GetType().GetField(value.ToString());

            if (field == null)
            {
                return value.ToString();
            }

            return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is not DescriptionAttribute attribute ? value.ToString() : attribute.Description;
        }
    }
}
