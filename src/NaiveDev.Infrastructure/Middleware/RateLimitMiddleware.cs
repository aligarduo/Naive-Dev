using Microsoft.AspNetCore.Http;
using NaiveDev.Infrastructure.Commons;
using NaiveDev.Infrastructure.Helpers.Bucket;

namespace NaiveDev.Infrastructure.Middleware
{
    /// <summary>
    /// 限制请求速率中间件
    /// </summary>
    public class RateLimitMiddleware(RequestDelegate next)
    {
        /// <summary>
        /// 下一个请求处理委托
        /// </summary>
        private readonly RequestDelegate _next = next;

        /// <summary>
        /// 令牌桶
        /// </summary>
        private readonly TokenBucketHelper tokenBucket = new(1000, 50, TimeSpan.FromSeconds(1));

        /// <summary>
        /// 当请求到达时，会尝试从令牌桶中消耗一个令牌
        /// </summary>
        /// <param name="context">HTTP上下文，包含请求和响应信息</param>
        /// <returns>一个表示异步操作的任务</returns>
        public async Task Invoke(HttpContext context)
        {
            // 尝试消耗一个令牌
            if (!tokenBucket.TryConsume())
            {
                // 如果没有可用的令牌
                await ResponseContext.ImmediateReturn(context, ResponseCode.TooManyRequests);
                return;
            }

            await _next(context);
        }
    }
}
