using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using NaiveDev.Infrastructure.Commons;

namespace NaiveDev.Infrastructure.Extensions
{
    /// <summary>
    /// 包含与<see cref="ApiBehaviorOptions"/>相关的扩展
    /// </summary>
    public static class ApiBehaviorOptionExtensions
    {
        /// <summary>
        /// 为<see cref="IServiceCollection"/>添加API行为选项
        /// </summary>
        /// <param name="services">依赖注入的服务集合</param>
        /// <returns>返回配置后的<see cref="IServiceCollection"/>实例，以便链式调用其他配置方法</returns>
        public static IServiceCollection AddApiBehaviorOption(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    List<string> errors = actionContext.ModelState
                        .Where(s => s.Value != null && s.Value.ValidationState == ModelValidationState.Invalid)
                        .SelectMany(s => s.Value!.Errors.ToList())
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return new OkObjectResult(ResponseBody.Fail(ResponseCode.BadRequest, errors.First()));
                };
            });

            return services;
        }
    }
}
