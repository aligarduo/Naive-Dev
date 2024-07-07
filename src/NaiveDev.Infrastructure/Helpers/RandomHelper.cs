using System.Text;
using NaiveDev.Shared.Tools;

namespace NaiveDev.Infrastructure.Helpers
{
    /// <summary>
    /// 随机数辅助类，提供生成随机数的功能
    /// </summary>
    public class RandomHelper
    {
        private readonly Random _random;

        /// <summary>
        /// 初始化<see cref="RandomHelper"/>实例
        /// </summary>
        public RandomHelper()
        {
            _random = new Random();
        }

        /// <summary> 
        /// 生成一个指定长度的随机数字字符串
        /// </summary>
        /// <param name="length">生成的随机数字字符串的长度，默认为6</param>
        /// <returns>返回生成的随机数字字符串</returns>
        public int RandomGen(int length = 6)
        {
            const string valid = "0123456789";
            StringBuilder res = new();

            while (res.Length < length)
            {
                res.Append(valid[_random.Next(valid.Length)]);
            }

            return res.ToInt();
        }
    }
}
