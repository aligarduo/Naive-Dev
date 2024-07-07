namespace NaiveDev.Shared.Tools
{
    /// <summary>
    /// 生成简短GUID的帮助类
    /// </summary>
    public class ShortGuidIdHelper
    {
        /// <summary>
        /// 包含所有可用字符的数组，用于生成简短的ID
        /// 包括数字（0-9）、小写字母（a-z）和大写字母（A-Z）  
        /// </summary>
        private static readonly char[] Characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        /// <summary>
        /// 生成一个简短的ID
        /// </summary>
        /// <remarks>
        /// ID的长度可以通过参数指定，默认为8个字符
        /// </remarks>
        /// <param name="length">生成的简短ID的长度，默认为8</param>
        /// <returns>生成的简短ID字符串</returns>
        private static string ShortIdGen(int length = 8)
        {
            string guidString = Guid.NewGuid().ToString("N");
            List<char> buffer = [];

            for (int i = 0; i < length; i++)
            {
                int val = Convert.ToInt32(guidString.Substring(i * 4, 4), 16);
                buffer.Add(Characters[val % 62]);
            }

            return new string(buffer.ToArray());
        }

        /// <summary>
        /// 生成一个简短的ID
        /// </summary>
        /// <remarks>
        /// 默认长度为8个字符
        /// </remarks>
        /// <returns>生成的简短ID字符串</returns>
        public static string NextId()
        {
            return ShortIdGen(8);
        }
    }
}
