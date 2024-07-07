using System.ComponentModel;

namespace NaiveDev.Infrastructure.Commons
{
    /// <summary>
    /// 响应状态码
    /// </summary>
    public enum ResponseCode
    {
        /// <summary>
        /// 很抱歉，我们的服务器遇到了一个错误，暂时无法处理您的请求。我们正在尽快修复，请稍后再试。
        /// </summary>
        [Description("很抱歉，我们的服务器遇到了一个错误，暂时无法处理您的请求。我们正在尽快修复，请稍后再试。")]
        InternalServerError = 1,

        /// <summary>
        /// 请求缺少必要的参数。请检查您的请求并确保所有必需的字段都已填写。
        /// </summary>
        [Description("请求缺少必要的参数。请检查您的请求并确保所有必需的字段都已填写。")]
        BadRequest = 1001,

        /// <summary>
        /// 对不起，您的认证凭据无效或已过期。
        /// </summary>
        [Description("对不起，您的认证凭据无效或已过期。")]
        Unauthorized = 1002,

        /// <summary>
        /// 很抱歉，您没有权限访问此资源。
        /// </summary>
        [Description("很抱歉，您没有权限访问此资源。")]
        Forbidden = 1003,

        /// <summary>
        /// 表示请求的资源在服务器上未找到。
        /// </summary>
        [Description("请求的资源在服务器上未找到。")]
        NotFound = 1004,

        /// <summary>
        /// 请求的资源与当前资源状态发生冲突，无法完成请求。这通常是由于违反了唯一性约束或其他资源冲突。
        /// </summary>
        [Description("请求的资源与当前资源状态发生冲突，无法完成请求。这通常是由于违反了唯一性约束或其他资源冲突。")]
        Conflict = 1005,

        /// <summary>
        /// 请求的格式正确，但是由于包含语义错误，服务器无法处理该请求。这通常用于验证失败、类型不匹配或参数无效等情况。
        /// </summary>
        [Description("请求的格式正确，但是服务器无法处理，因为请求中包含无效的数据或参数。")]
        UnprocessableEntity = 1006,

        /// <summary>
        /// 您的请求频率有点高，请稍微放慢一些。
        /// </summary>
        [Description("您的请求频率有点高，请稍微放慢一些。")]
        TooManyRequests = 1007,
    }
}
