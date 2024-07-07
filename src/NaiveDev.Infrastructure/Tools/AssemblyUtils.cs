using System.Reflection;

namespace NaiveDev.Infrastructure.Tools
{
    /// <summary>
    /// 程序集工具类
    /// </summary>
    public class AssemblyUtils
    {
        /// <summary>
        /// 从当前执行程序集中获取指定名称的嵌入资源内容
        /// </summary>
        /// <param name="name">要查找的嵌入资源的部分名称或完整名称</param>
        /// <returns>返回嵌入资源的字符串内容</returns>
        /// <exception cref="InvalidOperationException">如果找不到指定的嵌入资源或资源流为null，则抛出此异常</exception>
        public static string GetManifestResource(string name)
        {
            // 获取当前执行程序集的引用
            Assembly assembly = Assembly.GetExecutingAssembly();

            // 获取所有嵌入资源的名称
            string[] resourceNames = assembly.GetManifestResourceNames();

            // 遍历所有资源名称，找到与指定名称匹配的嵌入资源
            string? resourceName = resourceNames.Where(q => q.Contains(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            // 如果找不到匹配的嵌入资源名称
            if (string.IsNullOrEmpty(resourceName))
            {
                // 抛出异常，指示未找到指定的嵌入资源
                throw new InvalidOperationException($"Embedded resource {name} not found.");
            }

            // 尝试获取嵌入资源的流
            using Stream? stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException("The embedded resource could not be found.");

            // 使用StreamReader读取流的内容
            using StreamReader reader = new(stream);

            // 读取并返回嵌入资源的完整内容
            return reader.ReadToEnd();
        }
    }
}
