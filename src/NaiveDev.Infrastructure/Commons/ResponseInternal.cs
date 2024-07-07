namespace NaiveDev.Infrastructure.Commons
{
    /// <summary>
    /// 内部响应类，用于封装API或操作返回的结果信息
    /// </summary>
    /// <typeparam name="T">返回的数据类型</typeparam>
    public class ResponseInternal<T>
    {
        /// <summary>
        /// 表示操作是否成功，基于状态码是否为200进行判断
        /// </summary>
        public virtual bool IsSucceed => Code == 200;

        /// <summary>
        /// 响应的状态码，用于标识操作的结果状态
        /// </summary>
        public virtual int Code { get; init; }

        /// <summary>
        /// 操作返回的数据，如果操作成功，则包含有效的数据；否则，可能为空
        /// </summary>
        public virtual T? Data { get; init; }

        /// <summary>
        /// 如果操作失败，则包含错误信息的描述；否则，可能为空
        /// </summary>
        public virtual string? Error { get; init; }

        /// <summary>
        /// 创建一个表示成功的响应对象
        /// </summary>
        /// <param name="code">成功的状态码，通常为200</param>
        /// <param name="data">成功返回的数据对象</param>
        /// <returns>一个封装了成功信息和数据的<see cref="ResponseInternal{T}"/>对象</returns>
        public static ResponseInternal<T> Succeed(int code, object data) => new()
        {
            Code = code,
            Data = (T)data
        };

        /// <summary>
        /// 创建一个表示失败的响应对象
        /// </summary>
        /// <param name="code">失败的状态码</param>
        /// <param name="error">描述失败原因的错误信息</param>
        /// <returns>一个封装了失败信息和错误消息的<see cref="ResponseInternal{T}"/>对象</returns>
        public static ResponseInternal<T> Fail(int code, string error) => new()
        {
            Code = code,
            Error = error
        };
    }
}