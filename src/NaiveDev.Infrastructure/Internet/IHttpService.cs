using NaiveDev.Infrastructure.Commons;
using NaiveDev.Infrastructure.Service;

namespace NaiveDev.Infrastructure.Internet
{
    /// <summary>
    /// 与外部HTTP服务交互接口，提供发送GET和POST请求的基本功能
    /// </summary>
    public interface IHttpService : ITransientDependency
    {
        /// <summary>
        /// 发送GET请求到指定的URL，并返回泛型类型的响应
        /// </summary>
        /// <typeparam name="T">响应内容的泛型类型</typeparam>
        /// <param name="url">请求的URL地址</param>
        /// <returns>包含响应状态和内容的<see cref="ResponseInternal{T}"/>对象</returns>
        Task<ResponseInternal<T>> GetAsync<T>(string url);

        /// <summary>
        /// 发送POST请求到指定的URL，并附带请求数据，返回泛型类型的响应
        /// </summary>
        /// <typeparam name="T">响应内容的泛型类型</typeparam>
        /// <param name="url">请求的URL地址</param>
        /// <param name="data">要发送的请求数据，通常以JSON格式字符串表示</param>
        /// <returns>包含响应状态和内容的<see cref="ResponseInternal{T}"/>对象</returns>
        Task<ResponseInternal<T>> PostAsync<T>(string url, string data);
    }
}
