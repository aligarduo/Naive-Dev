using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NaiveDev.Domain.Entities;

namespace NaiveDev.Infrastructure.DataBase
{
    /// <summary>
    /// 自定义的数据库上下文，继承自<see cref="DbContext"/>，用于配置和管理数据库实体
    /// </summary>
    public class DbContext1(DbContextOptions<DbContext1> options) : DbContext(options)
    {
        /// <summary>
        /// 当前事务
        /// </summary>
        private IDbContextTransaction? CurrentTransaction;

        /// <summary>
        /// 当前DbContext是否开启了事务
        /// </summary>
        public bool HasActiveTransaction => CurrentTransaction != null;

        /// <summary>
        /// 开启事务
        /// </summary>
        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            CurrentTransaction = await Database.BeginTransactionAsync(isolation);

            return CurrentTransaction;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(transaction);

            if (transaction != CurrentTransaction)
            {
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
            }

            try
            {
                await SaveChangesAsync(cancellationToken);
                transaction.Commit();
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                }
            }
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (CurrentTransaction != null)
                {
                    await CurrentTransaction.RollbackAsync(cancellationToken);
                }
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                }
            }
        }

        /// <summary>
        /// 在模型创建时调用的方法，用于配置数据库模型
        /// </summary>
        /// <remarks>
        /// 在此方法中，你可以自定义实体与数据库表之间的映射关系，包括列的数据类型、长度、是否可为空、索引、外键关系等。
        /// 通常，你会重写此方法以应用特定的配置，而不是直接使用默认配置
        /// </remarks>
        /// <param name="modelBuilder">用于构建模型的ModelBuilder对象</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("user", tb => tb.HasComment("user"));

                builder.HasKey(u => u.Id);

                builder.Property(u => u.Id).HasColumnName("id");
                builder.Property(p => p.Account).HasColumnName("account");
                builder.Property(p => p.NickName).HasColumnName("nick_name");
                builder.Property(p => p.PasswordHash).HasColumnName("password_hash");
                builder.Property(p => p.PasswordSalt).HasColumnName("password_salt");
                builder.OwnsOne(u => u.Email, ea =>
                {
                    ea.Property(e => e.Address).HasColumnName("email");
                });
                builder.OwnsOne(u => u.Mobile, pn =>
                {
                    pn.Property(p => p.Number).HasColumnName("mobile");
                });
                builder.Property(p => p.Status).HasColumnName("status");
                builder.Property(p => p.CreatedAt).HasColumnName("created_at");
                builder.Property(p => p.ModifyedAt).HasColumnName("modifyed_at");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
