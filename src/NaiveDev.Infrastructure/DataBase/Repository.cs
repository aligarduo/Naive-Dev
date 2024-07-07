using NaiveDev.Shared.Domain;

namespace NaiveDev.Infrastructure.DataBase
{
    /// <summary>
    /// 业务仓储实现类，它基于<see cref="RepositoryBase{T}"/>，并实现了<see cref="IRepository{T}"/>接口
    /// </summary>
    /// <remarks>
    /// 此类提供了对特定业务实体类型的基本CRUD操作，并可能包含其他业务逻辑
    /// </remarks>
    /// <typeparam name="T">要操作的实体类型，必须继承自<see cref="EntityBase"/></typeparam>
    public class Repository<T> : RepositoryBase<T>, IRepository<T> where T : EntityBase
    {
        /// <summary> 
        /// 初始化业务仓储类的新实例，使用提供的数据库上下文
        /// </summary>
        /// <param name="context">用于数据库操作的数据库上下文实例</param>
        public Repository(DbContext1 context) : base(context)
        {

        }
    }
}
