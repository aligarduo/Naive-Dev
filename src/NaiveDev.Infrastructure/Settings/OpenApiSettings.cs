﻿namespace NaiveDev.Infrastructure.Settings
{
    /// <summary>
    /// 公开信息设置
    /// </summary>
    public class OpenApiSettings
    {
        /// <summary>
        /// 应用程序的标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 应用程序的简短描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 公开API的联系信息
        /// </summary>
        public Openapicontact? OpenApiContact { get; set; }
    }

    /// <summary>
    /// 公开API的联系信息
    /// </summary>
    public class Openapicontact
    {
        /// <summary>
        /// 联系人/机构的识别名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 联系人/组织的电子邮件地址
        /// </summary>
        /// <remarks>
        /// 必须是电子邮件地址的格式
        /// </remarks>
        public string? Email { get; set; }
    }
}
