namespace NaiveDev.Infrastructure.Helpers.Bucket
{
    /// <summary>
    /// 令牌桶
    /// </summary>
    public class TokenBucketHelper : IDisposable
    {
        /// <summary>
        /// 令牌桶的容量
        /// </summary>
        private readonly int _capacity;

        /// <summary>
        /// 每个时间间隔内要添加的令牌数量
        /// </summary>
        private readonly int _tokensToAddPerInterval;

        /// <summary>
        /// 添加令牌的时间间隔
        /// </summary>
        private readonly TimeSpan _interval;

        /// <summary>
        /// 用于同步访问令牌的信号量
        /// </summary>
        private readonly SemaphoreSlim _semaphore;

        /// <summary>
        /// 用于定期添加令牌的计时器
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// 初始化令牌桶
        /// </summary>
        /// <param name="capacity">令牌桶的容量</param>
        /// <param name="tokensToAddPerInterval">每个时间间隔内要添加的令牌数量</param>
        /// <param name="interval">添加令牌的时间间隔</param>
        public TokenBucketHelper(int capacity, int tokensToAddPerInterval, TimeSpan interval)
        {
            _capacity = capacity;
            _tokensToAddPerInterval = tokensToAddPerInterval;
            _interval = interval;

            // 初始化信号量，表示令牌桶的容量
            _semaphore = new SemaphoreSlim(capacity);

            // 启动计时器以定期添加令牌
            _timer = new Timer(state => RefillTokens(), null, _interval, _interval);
        }

        /// <summary>
        /// 定期添加令牌到令牌桶中
        /// </summary>
        private void RefillTokens()
        {
            // 计算应添加的令牌数，不超过容量和要添加的令牌数
            int tokensToAdd = Math.Min(_tokensToAddPerInterval, _capacity - _semaphore.CurrentCount);
            if (tokensToAdd > 0)
            {
                // 释放令牌，允许并发访问
                _semaphore.Release(tokensToAdd);
            }
        }

        /// <summary>
        /// 尝试从桶中获取一个令牌，如果成功则立即返回true，否则返回false（非阻塞）
        /// </summary>
        /// <returns>是否成功获取令牌</returns>
        public bool TryConsume()
        {
            // 尝试等待一个令牌，如果当前没有可用令牌，则立即返回false
            return _semaphore.Wait(0);
        }

        /// <summary>
        /// 从桶中获取一个令牌，如果当前没有可用令牌，则阻塞直到令牌可用
        /// </summary>
        public void Consume()
        {
            // 等待一个令牌，如果当前没有可用令牌，则阻塞
            _semaphore.Wait();
        }

        /// <summary>
        /// 释放令牌桶占用的资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放令牌桶占用的资源（包括托管和非托管资源）
        /// </summary>
        /// <param name="disposing">指示是否释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 停止计时器
                _timer?.Change(Timeout.Infinite, Timeout.Infinite);
                // 释放计时器
                _timer?.Dispose();
                // 释放信号量
                _semaphore?.Dispose();
            }
        }
    }
}
