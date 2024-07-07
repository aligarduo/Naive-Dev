using Microsoft.AspNetCore.Http;

namespace NaiveDev.Infrastructure.Commons
{
    /// <summary>
    /// 响应上下文
    /// </summary>
    public class ResponseContext
    {
        /// <summary>
        /// 即刻返回
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="responseCode">状态码</param>
        /// <returns>一个表示异步操作的任务</returns>
        public static async Task ImmediateReturn(HttpContext context, ResponseCode responseCode)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(ResponseBody.Fail(responseCode));
        }

        /// <summary>
        /// 即刻返回
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="responseCode">状态码</param>
        /// <param name="message">状态信息</param>
        /// <returns>一个表示异步操作的任务</returns>
        public static async Task ImmediateReturn(HttpContext context, ResponseCode responseCode, string message)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(ResponseBody.Fail(responseCode, message));
        }

        /// <summary>
        /// 即刻返回
        /// </summary>
        /// <param name="httpResponse">HTTP响应</param>
        /// <param name="responseCode">状态码</param>
        /// <returns>一个表示异步操作的任务</returns>
        public static async Task ImmediateReturn(HttpResponse httpResponse, ResponseCode responseCode)
        {
            httpResponse.StatusCode = 200;
            httpResponse.ContentType = "application/json";
            await httpResponse.WriteAsJsonAsync(ResponseBody.Fail(responseCode));
        }
    }
}
