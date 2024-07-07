namespace NaiveDev.Infrastructure.Auth
{
    /// <summary>
    /// 用户代理
    /// </summary>
    /// <remarks>
    /// 记录当前请求的客户端信息
    /// </remarks>
    public class UserAgent
    {
        /// <summary>
        /// 从用户代理字符串解析的设备
        /// </summary>
        public Device Device { get; } = new();
    }

    /// <summary>
    /// 从用户代理字符串解析的设备
    /// </summary>
    public class Device
    {
        /// <summary>
        /// 设备的品牌
        /// </summary>
        public string Brand { get; set; } = "unknown";

        /// <summary>
        /// 设备的型号
        /// </summary>
        public string Model { get; set; } = "unknown";
    }
}
