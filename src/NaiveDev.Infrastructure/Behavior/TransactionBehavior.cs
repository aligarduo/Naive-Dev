using System.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NaiveDev.Infrastructure.Commons;
using NaiveDev.Infrastructure.DataBase;

namespace NaiveDev.Infrastructure.Behavior
{
    /// <summary>
    /// 事务行为，用于确保在数据库操作中包裹请求的处理在一个事务内执行。如果当前数据库上下文已存在活动事务，则不会开始新的事务。
    /// </summary>
    /// <typeparam name="TRequest">请求的类型</typeparam>
    /// <typeparam name="TResponse">响应的类型</typeparam>
    public class TransactionBehavior<TRequest, TResponse>(DbContext1 context1) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : ResponseBody
    {
        /// <summary>
        /// 执行请求的处理，并在事务中包裹该处理
        /// </summary>
        /// <param name="request">请求实例</param>
        /// <param name="next">请求处理委托，表示后续的处理逻辑</param>
        /// <param name="cancellationToken">取消操作的令牌</param>
        /// <returns>返回请求处理的结果</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // 如果已存在活动事务，则直接执行后续处理
            if (context1.HasActiveTransaction)
            {
                return await next();
            }

            // 创建一个执行策略，用于在发生瞬态故障时重试
            IExecutionStrategy strategy = context1.Database.CreateExecutionStrategy();

            TResponse response = await strategy.ExecuteAsync(async () =>
            {
                // 开始一个新的事务
                using var transaction = await context1.BeginTransactionAsync(IsolationLevel.ReadCommitted);

                // 执行后续请求处理
                TResponse response = await next();

                // 如果没有异常，则提交事务
                if (response.Code == 0)
                {
                    await context1.CommitTransactionAsync(transaction, cancellationToken);
                }

                return response;
            });

            return response;
        }
    }
}
