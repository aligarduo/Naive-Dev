using System.Text.RegularExpressions;
using NaiveDev.Infrastructure.Auth;

namespace NaiveDev.Infrastructure.Tools
{
    /// <summary>
    /// 用户代理工具类
    /// </summary>
    public partial class UserAgentUtils
    {
        [GeneratedRegex(@"(?<DeviceType>Android|BlackBerry|Ericsson|HTC|IEMobile|iPhone|iPad|iPod|LG|Macintosh|Meego|Motorola|Nokia|Opera\sMini|Opera\sMobi|Palm|Panasonic|Philips|PlayBook|PortalMMM|Samsung|Sharp|Sony|SonyEricsson|SonyMobile|SPV|Symbian|SymbianOS|Tablet\sPC|webOS|Windows\sCE|Windows\sNT|Windows\sPhone|ZTE)", RegexOptions.IgnoreCase, "zh-CN")]
        private static partial Regex UserAgentDeviceTypeRegex();

        private static readonly Regex deviceTypeRegex = UserAgentDeviceTypeRegex();

        /// <summary>
        /// 根据用户代理字符串获取设备类型
        /// </summary>
        /// <param name="userAgent">用户代理字符串</param>
        /// <returns>包含设备信息的UserAgent对象如果用户代理为空或无效，则返回包含默认值的UserAgent对象</returns>
        public static UserAgent GetDeviceType(string? userAgent)
        {
            UserAgent userAgentDto = new();

            if (string.IsNullOrWhiteSpace(userAgent))
            {
                return userAgentDto;
            }

            Match match = deviceTypeRegex.Match(userAgent);
            if (match.Success && match.Groups["DeviceType"].Success)
            {
                userAgentDto.Device.Brand = match.Groups["DeviceType"].Value;
            }

            return userAgentDto;
        }
    }
}
