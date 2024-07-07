namespace NaiveDev.Infrastructure.Settings
{
    /// <summary>
    /// CORS策略设置
    /// </summary>
    public class CorsPolicySettings
    {
        private string? _policyName;
        private HashSet<string>? _origins;
        private HashSet<string>? _headers;
        private HashSet<string>? _methods;

        /// <summary>
        /// CORS策略的名称
        /// </summary>
        public string PolicyName
        {
            get
            {
                return string.IsNullOrWhiteSpace(_policyName) ? "Cors" : _policyName;
            }
            set
            {
                _policyName = value;
            }
        }

        /// <summary>
        /// 允许跨域请求的源列表
        /// </summary>
        public HashSet<string> Origins
        {
            get
            {
                return (_origins is null || _origins.Count == 0) ? [] : _origins;
            }
            set
            {
                _origins = value;
            }
        }

        /// <summary>
        /// 允许跨域请求中包含的HTTP头部列表
        /// </summary>
        public HashSet<string> Headers
        {
            get
            {
                return (_headers is null || _headers.Count == 0) ? [] : _headers;
            }
            set
            {
                _headers = value;
            }
        }

        /// <summary>
        /// 允许跨域请求的HTTP方法列表
        /// </summary>
        public HashSet<string> Methods
        {
            get
            {
                return (_methods is null || _methods.Count == 0) ? [] : _methods;
            }
            set
            {
                _methods = value;
            }
        }
    }
}
