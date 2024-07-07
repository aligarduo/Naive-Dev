using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace NaiveDev.Infrastructure.Attributes
{
    /// <summary>
    /// 版本路由属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class VersionRouteAttribute(VersionAttribute version) : RouteAttribute($"/api/{version}/[controller]"), IApiDescriptionGroupNameProvider
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; } = version.ToString();
    }
}