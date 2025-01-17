﻿using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NaiveDev.Infrastructure.Service;

namespace NaiveDev.Infrastructure.Extensions
{
    /// <summary>
    /// 包含与Autofac相关的扩展
    /// </summary>
    public static class AutofacExtensions
    {
        /// <summary>
        /// 为<see cref="IHostBuilder"/>添加Autofac支持，将Autofac设置为服务提供程序工厂，并配置Autofac容器以使用<see cref="AutofacRegisterModule"/>模块
        /// </summary>
        /// <param name="hostBuilder">要配置的主机生成器实例</param>
        /// <returns>配置后的<see cref="IHostBuilder"/>实例，以便链式调用其他配置方法</returns>
        public static IHostBuilder AddAutofac(this IHostBuilder hostBuilder)
        {
            // 设置Autofac为服务提供程序工厂
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            // 配置Autofac容器，注册AutofacRegisterModule模块
            hostBuilder.ConfigureContainer<ContainerBuilder>((hostBuilderContext, containerBuilder) =>
            {
                // 注册AutofacRegisterModule，并传递当前执行的程序集
                containerBuilder.RegisterModule(new AutofacRegisterModule());
            });

            // 返回配置后的IHostBuilder实例，以便链式调用其他配置方法
            return hostBuilder;
        }
    }

    /// <summary>
    /// Autofac模块注册类，用于在Autofac容器中注册程序集中的服务
    /// </summary>
    public class AutofacRegisterModule : Autofac.Module
    {
        /// <summary>
        /// 重写Load方法，用于加载和注册服务
        /// </summary>
        /// <param name="builder">Autofac的容器构建器对象</param>
        protected override void Load(ContainerBuilder builder)
        {
            // 扫描所有程序集，找到所有继承自ServiceBase的类型
            Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetTypes().Any(t => t.IsSubclassOf(typeof(ServiceBase))))
                .Distinct()
                .ToArray();

            // 遍历所有含有ServiceBase子类型的程序集
            foreach (var assembly in allAssemblies)
            {
                // 使用Autofac的扩展方法BatchAutowired，批量自动装配该程序集中的组件
                builder.BatchAutowired(assembly);
            }
        }
    }

    /// <summary>
    /// 依赖注入管理器，用于管理和配置依赖注入的容器
    /// </summary>
    public static class IocManager
    {
        /// <summary>
        /// 批量自动注入扩展方法，用于批量注册指定程序集中的类型到依赖注入容器中
        /// </summary>
        /// <param name="builder">Autofac容器构建器对象</param>
        /// <param name="assembly">需要注入的类型所在的程序集</param>
        public static void BatchAutowired(this ContainerBuilder builder, Assembly assembly)
        {
            // 定义瞬时生命周期的接口类型瞬时注入，每次请求都创建一个新的实例
            Type? transientType = typeof(ITransientDependency);

            // 定义单例生命周期的接口类型单例注入，在整个应用程序生命周期中只创建一个实例
            Type? singletonType = typeof(ISingletonDependency);

            // 定义作用域生命周期的接口类型作用域注入，每次请求作用域开始时创建一个新的实例，作用域结束时销毁
            Type? scopeType = typeof(IScopeDependency);

            // 批量注册瞬时生命周期的类型
            // 在指定程序集中查找所有满足条件的类型（是类、非抽象、实现了ITransitDenpendency接口），
            // 并以自身类型和实现的接口类型注册到容器中，生命周期为每次请求时创建一个新实例
            builder.RegisterAssemblyTypes(assembly).Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(transientType))
                .AsSelf() // 注册类型自身
                .AsImplementedInterfaces() // 注册类型实现的所有接口
                .InstancePerDependency(); // 每次请求时创建一个新实例

            // 批量注册单例生命周期的类型
            // 在指定程序集中查找所有满足条件的类型（是类、非抽象、实现了ISingletonDenpendency接口），
            // 并以自身类型和实现的接口类型注册到容器中，生命周期为单例
            builder.RegisterAssemblyTypes(assembly).Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(singletonType))
                .AsSelf() // 注册类型自身
                .AsImplementedInterfaces() // 注册类型实现的所有接口
                .SingleInstance(); // 单例模式，整个应用程序生命周期中只创建一个实例

            // 批量注册作用域生命周期的类型
            // 在指定程序集中查找所有满足条件的类型（是类、非抽象、实现了IScopeDenpendency接口），
            // 并以自身类型和实现的接口类型注册到容器中，生命周期为请求作用域
            builder.RegisterAssemblyTypes(assembly).Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(scopeType))
                .AsSelf() // 注册类型自身
                .AsImplementedInterfaces() // 注册类型实现的所有接口
                .InstancePerLifetimeScope(); // 每次请求作用域开始时创建一个新实例，作用域结束时销毁
        }
    }
}
