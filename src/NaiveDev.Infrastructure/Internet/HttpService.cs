using System.Text;
using NaiveDev.Infrastructure.Commons;
using NaiveDev.Infrastructure.Service;

namespace NaiveDev.Infrastructure.Internet
{
    /// <summary>
    /// 与外部HTTP服务交互实现类
    /// </summary>
    /// <remarks>
    /// 使用<see cref="IHttpClientFactory"/>创建<see cref="HttpClient"/>实例，以实现<see cref="HttpClient"/>的复用和管理
    /// </remarks>
    public class HttpService(IHttpClientFactory httpClientFactory) : ServiceBase, IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        /// <summary>
        /// 发送GET请求到指定的URL，并返回泛型类型的响应
        /// </summary>
        /// <typeparam name="T">响应内容的泛型类型</typeparam>
        /// <param name="url">请求的URL地址</param>
        /// <returns>包含响应状态和内容的<see cref="ResponseInternal{T}"/>对象</returns>
        public async Task<ResponseInternal<T>> GetAsync<T>(string url)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                using var response = await httpClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                return ResponseInternal<T>.Succeed((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return ResponseInternal<T>.Fail(400, ex.Message);
            }
        }

        /// <summary>
        /// 发送POST请求到指定的URL，并附带请求数据，返回泛型类型的响应
        /// </summary>
        /// <typeparam name="T">响应内容的泛型类型</typeparam>
        /// <param name="url">请求的URL地址</param>
        /// <param name="data">要发送的请求数据，通常以JSON格式字符串表示</param>
        /// <returns>包含响应状态和内容的<see cref="ResponseInternal{T}"/>对象</returns>
        public async Task<ResponseInternal<T>> PostAsync<T>(string url, string data)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync(url, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            return ResponseInternal<T>.Succeed((int)response.StatusCode, responseContent);
        }
    }
}
