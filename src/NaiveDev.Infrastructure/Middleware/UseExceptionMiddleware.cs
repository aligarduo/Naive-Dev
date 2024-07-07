using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NaiveDev.Infrastructure.Commons;

namespace NaiveDev.Infrastructure.Middleware
{
    /// <summary>
    /// 异常处理中间件配置扩展类
    /// </summary>
    /// <remarks>
    /// 为应用程序提供了配置异常处理中间件的扩展方法，用于捕获并处理未处理的异常
    /// </remarks>
    public static class UseExceptionMiddleware
    {
        /// <summary>
        /// 为应用程序添加异常处理中间件
        /// </summary>
        /// <param name="app">应用程序构建器实例</param>
        /// <param name="environment">提供有关Web主机环境和应用程序当前运行模式的信息</param>
        /// <returns>配置有异常处理中间件的应用程序构建器实例</returns>
        public static IApplicationBuilder UseException(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                Exception? exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (exception != null)
                {
                    if (environment.IsDevelopment())
                    {
                        await ResponseContext.ImmediateReturn(context, ResponseCode.InternalServerError, exception.Message);
                    }
                    else
                    {
                        await ResponseContext.ImmediateReturn(context, ResponseCode.InternalServerError);
                    }
                }
            }));

            return app;
        }
    }
}
