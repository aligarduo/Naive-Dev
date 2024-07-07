using Newtonsoft.Json;

namespace NaiveDev.Shared.Tools
{
    /// <summary>
    /// 转换工具类
    /// </summary>
    public static class ConvertUtils
    {
        /// <summary>
        /// 将给定的对象转换为整数(int)
        /// </summary>
        /// <remarks>
        /// 如果对象为空(null)或(DBNull.Value)，则返回0；如果对象不是整数且无法转换为整数，则返回0
        /// </remarks>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的整数，如果转换失败则返回0</returns>
        public static int ToInt(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return 0;
            }

            return int.TryParse(obj.ToString(), out int result) ? result : 0;
        }

        /// <summary>
        /// 将给定的对象转换为十进制数(decimal)
        /// </summary>
        /// <remarks>
        /// 如果对象为空(null)或(DBNull.Value)，则返回0；如果对象不是十进制数且无法转换为十进制数，则返回0
        /// </remarks>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的十进制数，如果转换失败则返回0</returns>
        public static decimal ToDecimal(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return 0;
            }

            return decimal.TryParse(obj.ToString(), out decimal result) ? result : 0;
        }

        /// <summary>
        /// 将给定的对象转换为DateTime类型
        /// </summary>
        /// <remarks>
        /// 如果对象为空(null)或(DBNull.Value)，或者无法转换为DateTime类型，则返回DateTime.MinValue
        /// </remarks>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的DateTime值，如果转换失败则返回DateTime.MinValue</returns>
        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return DateTime.MinValue;
            }

            return DateTime.TryParse(obj.ToString(), out DateTime result) ? result : DateTime.MinValue;
        }

        /// <summary>
        /// 将给定的对象转换为布尔值(bool)
        /// </summary>
        /// <remarks>
        /// 如果对象为(null)或(DBNull.Value)，则返回false；如果对象不是布尔值且无法转换为布尔值，则返回false
        /// </remarks>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的布尔值，如果转换失败则返回false</returns>
        public static bool ToBool(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return false;
            }

            return bool.TryParse(obj.ToString(), out bool result) && result;
        }

        /// <summary>
        /// 将给定的对象转换为指定的枚举类型(T)
        /// </summary>
        /// <remarks>
        /// 如果对象为(null)或(DBNull.Value)，则返回该枚举类型的默认值；
        /// 如果对象不是该枚举类型的有效字符串表示，或者无法转换为该枚举类型，则返回该枚举类型的默认值
        /// </remarks>
        /// <typeparam name="T">要转换成的枚举类型</typeparam>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的枚举值，如果转换失败则返回该枚举类型的默认值</returns>
        public static T ToEnum<T>(this object obj) where T : struct, Enum
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default;
            }

            if (Enum.TryParse(obj.ToString(), out T result))
            {
                return result;
            }

            return default;
        }

        /// <summary>
        /// 将给定的对象序列化为JSON格式的字符串
        /// </summary>
        /// <remarks>
        /// 如果对象为null或DBNull.Value，则返回空字符串
        /// </remarks>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的JSON字符串，如果对象为null或DBNull.Value则返回空字符串</returns>
        public static string ToJson(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return string.Empty;
            }

            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Serialization failed: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// 将JSON字符串反序列化为指定类型的对象
        /// </summary>
        /// <remarks>
        /// 如果JSON字符串为空、仅包含空白字符或无法反序列化，则返回该类型的默认值(对于引用类型，通常是null)
        /// </remarks>
        /// <typeparam name="T">要反序列化的目标类型，必须是一个引用类型</typeparam>
        /// <param name="json">包含JSON数据的字符串</param>
        /// <returns>反序列化后的对象实例；如果输入为空、仅包含空白字符或无法反序列化，则返回该类型的默认值</returns>
        public static T? ToObject<T>(this string json) where T : class
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"Invalid JSON string: {ex.Message}");
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Unable to deserialize JSON to type {typeof(T).Name}: {ex.Message}");
            }

            return default;
        }

        /// <summary>
        /// 将给定对象转换为指定类型T的实例
        /// </summary>
        /// <remarks>
        /// 如果对象为空、是DBNull.Value或者转换为JSON字符串后为空，则返回T类型的默认值(对于引用类型，通常是null)
        /// </remarks>
        /// <typeparam name="T">要转换到的目标类型，必须是一个类(引用类型)</typeparam>
        /// <param name="obj">要转换的源对象</param>
        /// <returns>转换后的T类型实例；如果转换失败或源对象无效，则返回T类型的默认值</returns>
        public static T? ToObject<T>(this object obj) where T : class
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default;
            }

            string json = obj.ToJson();
            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"Invalid JSON string: {ex.Message}");
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Unable to deserialize JSON to type {typeof(T).Name}: {ex.Message}");
            }

            return default;
        }
    }
}
