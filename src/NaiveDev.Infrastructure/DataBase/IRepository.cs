using NaiveDev.Shared.Domain;

namespace NaiveDev.Infrastructure.DataBase
{
    /// <summary>
    /// 定义了特定业务逻辑所需的仓储接口，继承自通用的<see cref="IRepositoryBase{T}"/>接口
    /// </summary>
    /// <typeparam name="T">表示仓储中要操作的实体类型，必须继承自<see cref="EntityBase"/></typeparam>
    public interface IRepository<T> : IRepositoryBase<T> where T : EntityBase
    {

    }
}
