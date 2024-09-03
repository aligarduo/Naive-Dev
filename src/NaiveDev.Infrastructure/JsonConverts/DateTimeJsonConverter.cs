using System.Text.Json;
using System.Text.Json.Serialization;

namespace NaiveDev.Infrastructure.JsonConverts
{
    /// <summary>
    /// DateTime类型的JSON转换器，用于自定义DateTime类型在序列化和反序列化过程中的格式
    /// </summary>
    /// <remarks>
    /// 在反序列化时，尝试将JSON字符串转换为DateTime，如果转换失败则返回DateTime的默认值。
    /// 在序列化时，将DateTime对象格式化为"yyyy-MM-dd HH:mm:ss"格式的字符串。
    /// </remarks>
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// 从JSON读取器读取DateTime值
        /// 尝试将JSON字符串转换为DateTime，如果成功则返回该DateTime，否则返回DateTime的默认值
        /// </summary>
        /// <param name="reader">JSON读取器</param>
        /// <param name="typeToConvert">需要转换的类型，此处为DateTime</param>
        /// <param name="options">JSON序列化选项</param>
        /// <returns>转换得到的DateTime值，如果转换失败则返回DateTime的默认值</returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.TryParse(reader.GetString(), out DateTime dateTime) ? dateTime : default;
        }

        /// <summary>
        /// 将DateTime值写入JSON写入器
        /// 将DateTime对象格式化为"yyyy-MM-dd HH:mm:ss"格式的字符串，并写入JSON写入器
        /// </summary>
        /// <param name="writer">JSON写入器</param>
        /// <param name="value">需要写入的DateTime值</param>
        /// <param name="options">JSON序列化选项</param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
