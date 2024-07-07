using System.Diagnostics;

namespace NaiveDev.Infrastructure.Helpers.Bucket
{
    /// <summary>
    /// LeakyBucket类实现了漏桶算法，用于限制一段时间内可以处理的请求数量
    /// </summary>
    public class LeakyBucketHelper
    {
        /// <summary>
        /// 桶的容量，表示在任意时刻桶内可以容纳的最大水量
        /// </summary>
        private readonly int _capacity;

        /// <summary>
        /// 漏出速率，表示每毫秒从桶中漏出的水量
        /// </summary>
        private readonly int _leakRatePerMillisecond;

        /// <summary>
        /// 当前水量，表示当前桶内的水量
        /// </summary>
        private int _currentWaterLevel;

        /// <summary>
        /// 上次加水时间的时间戳，使用 Stopwatch 的时间戳表示
        /// </summary>
        private long _lastRefillTime;

        /// <summary>
        /// 用于高精度时间测量的 Stopwatch 实例
        /// </summary>
        private readonly Stopwatch _stopwatch = new();

        /// <summary>
        /// 初始化 LeakyBucket 类的新实例
        /// </summary>
        /// <param name="capacity">桶的容量</param>
        /// <param name="leakRatePerSecond">每秒的漏出速率</param>
        public LeakyBucketHelper(int capacity, int leakRatePerSecond)
        {
            _capacity = capacity;
            _leakRatePerMillisecond = leakRatePerSecond / 1000;
            _currentWaterLevel = 0;
            _stopwatch.Start();
            _lastRefillTime = _stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// 尝试获取一个请求，如果桶内有足够的空间则允许请求并增加水量，否则拒绝请求
        /// </summary>
        /// <returns>如果成功获取请求则返回 true，否则返回 false</returns>
        public bool TryAcquire()
        {
            long now = _stopwatch.ElapsedMilliseconds;
            long elapsedMilliseconds = now - _lastRefillTime;

            // 先执行漏水
            DrainWater(elapsedMilliseconds);

            // 尝试加水（即允许请求）
            if (_currentWaterLevel < _capacity)
            {
                _currentWaterLevel++;
                _lastRefillTime = now;
                return true;
            }

            // 桶已满，拒绝请求
            return false;
        }

        /// <summary>
        /// 私有方法，根据经过的时间排水
        /// </summary>
        /// <param name="elapsedMilliseconds">自从上次加水后经过的毫秒数</param>
        private void DrainWater(long elapsedMilliseconds)
        {
            int leakedWater = (int)(_leakRatePerMillisecond * elapsedMilliseconds);
            _currentWaterLevel = Math.Max(0, _currentWaterLevel - leakedWater);
        }
    }
}
