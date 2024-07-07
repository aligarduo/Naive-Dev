using NaiveDev.Shared.Tools;

namespace NaiveDev.Shared.Domain
{
    /// <summary>
    /// 领域实体基类
    /// </summary>
    public abstract class EntityBase
    {
        private SnowflakeIdHelper? _snowflakeId;
        private SnowflakeIdHelper SnowflakeId
        {
            get
            {
                _snowflakeId ??= new SnowflakeIdHelper(1, 1);
                return _snowflakeId;
            }
        }

        /// <summary>
        /// 构造函数，初始化创建于和修改于时间
        /// </summary>
        public EntityBase()
        {
            Id = SnowflakeId.NextId();
            CreatedAt = DateTime.Now;
            ModifyedAt = DateTime.Now;
        }

        /// <summary>
        /// 更改数据后记录更改信息
        /// </summary>
        protected void UpdateRecord()
        {
            ModifyedAt = DateTime.Now;
        }

        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        /// 创建于
        /// </summary>
        public DateTime CreatedAt { get; protected set; }

        /// <summary>
        /// 修改于
        /// </summary>
        public DateTime ModifyedAt { get; protected set; }
    }
}
