using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NaiveDev.Shared.Domain;

namespace NaiveDev.Infrastructure.DataBase
{
    /// <summary>
    /// 泛型仓储基类，实现了<see cref="IRepositoryBase{T}"/>接口
    /// </summary>
    /// <remarks>
    /// 用于操作数据库中的指定实体类型（继承自<see cref="EntityBase"/>）
    /// </remarks>
    /// <typeparam name="T">要操作的实体类型，必须继承自<see cref="EntityBase"/></typeparam>
    public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        protected DbContext Context { get; }

        /// <summary>
        /// 表示给定实体类型的集合的DbSet
        /// </summary>
        protected DbSet<T> DbSet { get; }

        /// <summary> 
        /// 初始化<see cref="RepositoryBase{T}"/>类的实例
        /// </summary>
        /// <param name="context">数据库上下文实例</param>
        public RepositoryBase(DbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        /// <summary>
        /// 获取表示给定实体类型的集合的DbSet，用于查询数据库中对应的数据，并启用实体更改追踪 
        /// </summary>
        /// <returns>表示给定实体类型的集合的DbSet，数据将进行更改追踪</returns>
        public IQueryable<T> TrackingQuery()
        {
            return DbSet;
        }

        /// <summary>
        /// 获取表示给定实体类型的集合的DbSet，用于查询数据库中对应的数据，但不会启用实体更改追踪
        /// </summary>
        /// <remarks>
        /// 这通常用于只读操作，以提高性能
        /// </remarks>
        /// <returns>表示给定实体类型的集合的DbSet，数据不会进行更改追踪</returns>
        public IQueryable<T> NoTrackingQuery()
        {
            return DbSet.AsNoTracking();
        }

        /// <summary>
        /// 将给定的实体添加到数据库上下文中，以便在后续的SaveChanges或SaveChangesAsync调用中插入到数据库中
        /// </summary>
        /// <param name="entity">要添加的实体</param>
        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        /// <summary>
        /// 异步地将给定的实体添加到数据库上下文中，以便在后续的SaveChanges或SaveChangesAsync调用中插入到数据库中
        /// </summary>
        /// <param name="entity">要添加的实体</param>
        /// <returns>表示异步操作的Task</returns>
        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        /// <summary>
        /// 异步地将给定实体的数组添加到数据库上下文中，以便在后续的SaveChanges或SaveChangesAsync调用中插入到数据库中
        /// </summary>
        /// <param name="entities">要添加的实体数组</param>
        /// <returns>表示异步操作的Task</returns>
        public async Task AddRangeAsync(params T[] entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// 异步地将给定实体的集合添加到数据库上下文中，以便在后续的SaveChanges或SaveChangesAsync调用中插入到数据库中 
        /// </summary>
        /// <param name="entities">要添加的实体集合</param>
        /// <returns>表示异步操作的Task</returns>
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// 将数据库上下文中的所有挂起的更改保存到数据库中
        /// </summary>
        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        /// <summary>
        /// 异步地将数据库上下文中的所有挂起的更改保存到数据库中
        /// </summary>
        /// <returns>表示异步操作的Task</returns>
        public Task SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// 将给定的实体标记为从数据库中删除，以便在后续的SaveChanges或SaveChangesAsync调用中执行删除操作
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        /// <summary>
        /// 异步地将给定的实体标记为从数据库中删除，以便在后续的SaveChanges或SaveChangesAsync调用中执行删除操作
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns>表示异步操作的Task</returns>
        public Task DeleteAsync(T entity)
        {
            DbSet.Remove(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步地将给定的实体数组标记为从数据库中删除，以便在后续的SaveChanges或SaveChangesAsync调用中执行删除操作
        /// </summary>
        /// <param name="entities">要删除的实体数组</param>
        /// <returns>表示异步操作的Task</returns>
        public Task DeleteAsync(params T[] entities)
        {
            DbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步地将给定的实体集合标记为从数据库中删除，以便在后续的SaveChanges或SaveChangesAsync调用中执行删除操作
        /// </summary>
        /// <param name="entities">要删除的实体集合</param>
        /// <returns>表示异步操作的Task</returns>
        public Task DeleteAsync(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步地开启一个新的数据库事务，并允许设置事务的隔离级别
        /// </summary>
        /// <param name="isolation">指定事务的隔离级别默认为<see cref="IsolationLevel.ReadCommitted"/></param>
        /// <returns>一个表示新开启的数据库事务的异步操作</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            return Context.Database.CurrentTransaction ?? await Context.Database.BeginTransactionAsync(isolation);
        }
    }
}
