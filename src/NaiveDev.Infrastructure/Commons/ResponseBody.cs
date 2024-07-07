using NaiveDev.Shared.Tools;

namespace NaiveDev.Infrastructure.Commons
{
    /// <summary>
    /// 响应正文
    /// </summary>
    public class ResponseBody
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public virtual int Code { get; init; }

        /// <summary>
        /// 状态信息
        /// </summary>
        public virtual required string Message { get; init; }

        /// <summary>
        /// 创建一个表示成功的响应体对象
        /// </summary>
        /// <returns>
        /// 返回一个<see cref="ResponseBody"/>对象，其中：
        /// <list type="bullet">
        /// <item><see cref="Code"/>属性设置为0，表示成功</item>
        /// <item><see cref="Message"/>属性设置为"success"，表示成功的消息</item>
        /// </list>
        /// </returns>
        public static ResponseBody Succeed() => new()
        {
            Code = 0,
            Message = "success"
        };

        /// <summary>
        /// 创建一个表示成功的响应体对象，并指定状态信息
        /// </summary>
        /// <param name="message">要包含在成功响应中的状态信息</param>
        /// <returns>
        /// 返回一个<see cref="ResponseBody"/>对象，其中：
        /// <list type="bullet">
        /// <item><see cref="Code"/>属性设置为0，表示成功</item>
        /// <item><see cref="Message"/>属性设置为传入的<paramref name="message"/>参数，用于表示成功的具体状态信息</item>
        /// </list>
        /// </returns>
        public static ResponseBody Succeed(string message) => new()
        {
            Code = 0,
            Message = message
        };

        /// <summary>
        /// 创建一个表示失败的响应体对象，并指定响应代码和对应的描述信息
        /// </summary>
        /// <param name="responseCode">表示失败状态的响应代码</param>
        /// <returns>
        /// 返回一个<see cref="ResponseBody"/>对象，其中：
        /// <list type="bullet">
        /// <item><see cref="Code"/>属性设置为<paramref name="responseCode"/>的整数值，表示失败状态</item>
        /// <item><see cref="Message"/>属性通过调用<see cref="EnumUtils.GetDescription"/>扩展方法来获取<paramref name="responseCode"/>对应的描述信息，用于表示失败的具体原因</item>
        /// </list>
        /// </returns>
        public static ResponseBody Fail(ResponseCode responseCode) => new()
        {
            Code = (int)responseCode,
            Message = responseCode.GetDescription()
        };

        /// <summary>
        /// 创建一个表示失败的响应体对象，并指定响应代码和自定义的状态信息
        /// </summary>
        /// <param name="responseCode">表示失败状态的响应代码枚举值</param>
        /// <param name="message">与失败状态相关联的自定义状态信息</param>
        /// <returns>
        /// 返回一个<see cref="ResponseBody"/>对象，其中：
        /// <list type="bullet">
        /// <item><see cref="Code"/>属性设置为<paramref name="responseCode"/>的整数值，表示失败状态</item>
        /// <item><see cref="Message"/>属性设置为<paramref name="message"/>参数，用于提供与失败状态相关联的自定义状态信息</item>
        /// </list>
        /// </returns>
        public static ResponseBody Fail(ResponseCode responseCode, string message) => new()
        {
            Code = (int)responseCode,
            Message = message
        };
    }

    /// <summary>
    /// 泛型响应正文
    /// </summary>
    /// <typeparam name="T">响应数据的类型</typeparam>
    public class ResponseBody<T> : ResponseBody
    {
        /// <summary>
        /// 根据特定查询条件检索到的单条数据
        /// </summary>
        public virtual T? Data { get; init; }

        /// <summary>
        /// 创建一个表示成功的响应体对象，并包含根据特定查询条件检索到的单条数据
        /// </summary>
        /// <param name="data">根据特定查询条件检索到的单条数据</param>
        /// <returns>
        /// 返回一个<see cref="ResponseBody{T}"/>对象，其中：
        /// <list type="bullet">
        /// <item><see cref="ResponseBody.Code"/>属性设置为0，表示成功状态</item>
        /// <item><see cref="ResponseBody.Message"/>属性设置为"success"，表示操作成功</item>
        /// <item><see cref="ResponseBody{T}.Data"/>属性设置为<paramref name="data"/>参数，包含根据特定查询条件检索到的单条数据</item>
        /// </list>
        /// </returns>
        public static ResponseBody<T> Succeed(T data) => new()
        {
            Code = 0,
            Message = "success",
            Data = data
        };

        /// <summary>
        /// 创建一个表示失败的响应体对象，并指定响应代码和对应的描述信息
        /// </summary>
        /// <param name="responseCode">表示失败状态的响应代码</param>
        /// <returns>
        /// 返回一个<see cref="ResponseBody{T}"/>对象，其中：
        /// <list type="bullet">
        /// <item><see cref="ResponseBody.Code"/>属性设置为<paramref name="responseCode"/>的整数值，表示失败状态</item>
        /// <item><see cref="ResponseBody.Message"/>属性通过调用<see cref="EnumUtils.GetDescription"/>扩展方法来获取<paramref name="responseCode"/>对应的描述信息，用于表示失败的具体原因</item>
        /// </list>
        /// </returns>
        public static new ResponseBody<T> Fail(ResponseCode responseCode) => new()
        {
            Code = (int)responseCode,
            Message = responseCode.GetDescription()
        };

        /// <summary>
        /// 创建一个表示失败的响应体对象，并指定响应代码和自定义的状态信息
        /// </summary>
        /// <param name="responseCode">表示失败状态的响应代码枚举值</param>
        /// <param name="message">与失败状态相关联的自定义状态信息</param>
        /// <returns>
        /// <list type="bullet">
        /// 返回一个<see cref="ResponseBody{T}"/>对象，其中：
        /// <item><see cref="ResponseBody.Code"/>属性设置为<paramref name="responseCode"/>的整数值，表示失败状态</item>
        /// <item><see cref="ResponseBody.Message"/>属性设置为<paramref name="message"/>参数，用于提供与失败状态相关联的自定义状态信息</item>
        /// </list>
        /// </returns>
        public static new ResponseBody<T> Fail(ResponseCode responseCode, string message) => new()
        {
            Code = (int)responseCode,
            Message = message
        };
    }

    /// <summary>
    /// 泛型分页响应正文
    /// </summary>
    /// <typeparam name="T">响应数据类型</typeparam>
    public class ResponseBodyPage<T> : ResponseBody
    {
        /// <summary>
        /// 当前页的数据集合
        /// </summary>
        public virtual IEnumerable<T>? Data { get; init; }

        /// <summary>
        /// 当前页码（从1开始计数）
        /// </summary>
        public virtual int? PageNumber { get; init; }

        /// <summary>
        /// 每页显示的数据条数
        /// </summary>
        public virtual int? PageSize { get; init; }

        /// <summary>
        /// 数据的总条数
        /// </summary>
        public virtual int? TotalCount { get; init; }

        /// <summary>
        /// 数据的总页数
        /// </summary>
        public virtual int? TotalPages { get; init; }

        /// <summary>
        /// 创建一个表示分页成功的响应体对象
        /// </summary>
        /// <param name="data">当前页的数据集合</param>
        /// <param name="pageNumber">当前页码（从1开始计数）</param>
        /// <param name="pageSize">每页显示的数据条数</param>
        /// <param name="totalCount">数据的总条数</param>
        /// <param name="totalPages">数据的总页数</param>
        /// <returns>
        /// 返回一个<see cref="ResponseBodyPage{T}"/>对象，其中：
        /// <list type="bullet">
        /// <item><see cref="ResponseBody.Code"/>属性设置为0，表示成功</item>
        /// <item><see cref="ResponseBody.Message"/>属性设置为"success"，表示成功的消息</item>
        /// <item><see cref="Data"/>属性设置为<paramref name="data"/>，表示当前页的数据集合</item>
        /// <item><see cref="PageNumber"/>属性设置为<paramref name="pageNumber"/>，表示当前页码</item>
        /// <item><see cref="PageSize"/>属性设置为<paramref name="pageSize"/>，表示每页显示的数据条数</item>
        /// <item><see cref="TotalCount"/>属性设置为<paramref name="totalCount"/>，表示数据的总条数</item>
        /// <item><see cref="TotalPages"/>属性设置为<paramref name="totalPages"/>，表示数据的总页数</item>
        /// </list>
        /// </returns>
        public static ResponseBodyPage<T> Succeed(IEnumerable<T> data, int pageNumber, int pageSize, int totalCount, int totalPages) => new()
        {
            Code = 0,
            Message = "success",
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        /// <summary>
        /// 创建一个表示失败的响应体对象，并指定响应代码和对应的描述信息
        /// </summary>
        /// <param name="responseCode">表示失败状态的响应代码</param>
        /// <returns>
        /// 返回一个<see cref="ResponseBody{T}"/>对象，其中：
        /// <list type="bullet">
        /// <item><see cref="ResponseBody.Code"/>属性设置为<paramref name="responseCode"/>的整数值，表示失败状态</item>
        /// <item><see cref="ResponseBody.Message"/>属性通过调用<see cref="EnumUtils.GetDescription"/>扩展方法来获取<paramref name="responseCode"/>对应的描述信息，用于表示失败的具体原因</item>
        /// </list>
        /// </returns>
        public static new ResponseBodyPage<T> Fail(ResponseCode responseCode) => new()
        {
            Code = (int)responseCode,
            Message = responseCode.GetDescription()
        };

        /// <summary>
        /// 创建一个表示失败的响应体对象，并指定响应代码和自定义的状态信息
        /// </summary>
        /// <param name="responseCode">表示失败状态的响应代码枚举值</param>
        /// <param name="message">与失败状态相关联的自定义状态信息</param>
        /// <returns>
        /// <list type="bullet">
        /// 返回一个<see cref="ResponseBody{T}"/>对象，其中：
        /// <item><see cref="ResponseBody.Code"/>属性设置为<paramref name="responseCode"/>的整数值，表示失败状态</item>
        /// <item><see cref="ResponseBody.Message"/>属性设置为<paramref name="message"/>参数，用于提供与失败状态相关联的自定义状态信息</item>
        /// </list>
        /// </returns>
        public static new ResponseBodyPage<T> Fail(ResponseCode responseCode, string message) => new()
        {
            Code = (int)responseCode,
            Message = message
        };
    }
}