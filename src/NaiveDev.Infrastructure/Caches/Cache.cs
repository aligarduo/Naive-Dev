using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using NaiveDev.Infrastructure.Enums;
using NaiveDev.Shared.Tools;

namespace NaiveDev.Infrastructure.Caches
{
    /// <summary>
    /// 缓存服务实现
    /// </summary>
    public class Cache(IDistributedCache cache) : ICache
    {
        private readonly IDistributedCache _cache = cache;

        /// <summary>
        /// 构建缓存Key
        /// </summary>
        /// <param name="key">用于构建缓存键的输入字符串</param>
        /// <returns>构建好的缓存键字符串</returns>
        protected static string BuildKey(string key)
        {
            return key;
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        public void SetCache(string key, object value)
        {
            string cacheKey = BuildKey(key);
            _cache.SetString(cacheKey, value.ToJson());
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
        /// <returns>一个表示异步操作的任务</returns>
        public async Task SetCacheAsync(string key, object value, CancellationToken cancellationToken = default)
        {
            string cacheKey = BuildKey(key);
            await _cache.SetStringAsync(cacheKey, value.ToJson(), cancellationToken);
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="timeout">过期时间间隔</param>
        public void SetCache(string key, object value, TimeSpan timeout)
        {
            string cacheKey = BuildKey(key);
            _cache.SetString(cacheKey, value.ToJson(), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.Now.Add(timeout))
            });
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="timeout">过期时间间隔</param>
        /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
        /// <returns>一个表示异步操作的任务</returns>
        public async Task SetCacheAsync(string key, object value, TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            string cacheKey = BuildKey(key);
            await _cache.SetStringAsync(cacheKey, value.ToJson(), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.Now.Add(timeout))
            }, cancellationToken);
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="timeout">过期时间间隔</param>
        /// <param name="expireType">过期类型</param>
        public void SetCache(string key, object value, TimeSpan timeout, CacheExpire expireType)
        {
            string cacheKey = BuildKey(key);

            DistributedCacheEntryOptions distributedCacheEntryOptions = (expireType == CacheExpire.Absolute) ?
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.Add(timeout))
                }
                :
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = timeout
                };

            _cache.SetString(cacheKey, value.ToJson(), distributedCacheEntryOptions);
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="timeout">过期时间间隔</param>
        /// <param name="expireType">过期类型</param>
        /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
        /// <returns>一个表示异步操作的任务</returns>
        public async Task SetCacheAsync(string key, object value, TimeSpan timeout, CacheExpire expireType, CancellationToken cancellationToken = default)
        {
            string cacheKey = BuildKey(key);

            DistributedCacheEntryOptions distributedCacheEntryOptions = (expireType == CacheExpire.Absolute) ?
               new DistributedCacheEntryOptions
               {
                   AbsoluteExpiration = new DateTimeOffset(DateTime.Now.Add(timeout))
               }
               :
               new DistributedCacheEntryOptions
               {
                   AbsoluteExpirationRelativeToNow = timeout
               };

            await _cache.SetStringAsync(cacheKey, value.ToJson(), distributedCacheEntryOptions, cancellationToken);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        public string GetCache(string key)
        {
            if (key == null)
            {
                return string.Empty;
            }

            string cacheKey = BuildKey(key);
            return _cache.GetString(cacheKey) ?? string.Empty;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
        /// <returns>一个表示异步操作的任务</returns>
        public async Task<string> GetCacheAsync(string key, CancellationToken cancellationToken = default)
        {
            if (key == null)
            {
                return string.Empty;
            }

            string cacheKey = BuildKey(key);
            return await _cache.GetStringAsync(cacheKey, cancellationToken) ?? string.Empty;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        public T? GetCache<T>(string key) where T : class
        {
            string cacheKey = BuildKey(key);
            var cache = GetCache(cacheKey);
            if (cache != null)
            {
                return cache.ToObject<T>();
            }

            return default;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
        /// <returns>一个表示异步操作的任务</returns>
        public async Task<T?> GetCacheAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            string cacheKey = BuildKey(key);
            var cache = await GetCacheAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrEmpty(cache))
            {
                return cache.ToObject<T>();
            }

            return default;
        }

        /// <summary>
        /// 是否存在缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        public bool IsAny(string key)
        {
            string cacheKey = BuildKey(key);
            var cache = GetCache(cacheKey);
            if (!string.IsNullOrEmpty(cache))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否存在缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
        /// <returns>一个表示异步操作的任务</returns>
        public async Task<bool> IsAnyAsync(string key, CancellationToken cancellationToken = default)
        {
            string cacheKey = BuildKey(key);
            var cache = await GetCacheAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrEmpty(cache))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        public void RemoveCache(string key)
        {
            string cacheKey = BuildKey(key);
            _cache.Remove(cacheKey);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
        /// <returns>一个表示异步操作的任务</returns>
        public async Task RemoveCacheAsync(string key, CancellationToken cancellationToken = default)
        {
            string cacheKey = BuildKey(key);
            await _cache.RemoveAsync(cacheKey, cancellationToken);
        }

        /// <summary>
        /// 刷新缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        public void RefreshCache(string key)
        {
            string cacheKey = BuildKey(key);
            _cache.Refresh(cacheKey);
        }

        /// <summary>
        /// 刷新缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="cancellationToken">取消令牌，用于取消异步操作</param>
        /// <returns>一个表示异步操作的任务</returns>
        public async Task RefreshCacheAsync(string key, CancellationToken cancellationToken = default)
        {
            string cacheKey = BuildKey(key);
            await _cache.RefreshAsync(cacheKey, cancellationToken);
        }

        /// <summary>
        /// 根据指定的缓存键获取缓存的值，如果缓存不存在则使用提供的函数计算结果并设置到缓存中
        /// </summary>
        /// <typeparam name="T">缓存值的类型</typeparam>
        /// <param name="key">缓存键，用于标识缓存中的项</param>
        /// <param name="func">用于计算缓存值的函数，当缓存项不存在时调用</param>
        /// <param name="timeout">缓存项的过期时间间隔</param>
        /// <param name="cancellationToken">取消令牌，用于取消正在进行的异步操作</param>
        /// <returns>返回一个表示异步操作的任务，该任务在完成时包含缓存项的值或计算得到的值</returns>
        public async Task<T> GetOrSetCache<T>(string key, Func<T> func, TimeSpan timeout, CancellationToken cancellationToken = default) where T : class
        {
            var cache = await GetCacheAsync<T>(key, cancellationToken);
            if (cache != null)
            {
                return cache;
            }

            var result = func() ?? throw new InvalidOperationException();
            await SetCacheAsync(key, result, timeout, cancellationToken);

            return result;
        }
    }
}