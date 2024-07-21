using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NaiveDev.Infrastructure.Extensions;

namespace NaiveDev.Infrastructure.Extensions
{
    /// <summary>
    /// EFCore 扩展方法
    /// </summary>
    public static class EFCoreExtensions
    {
        /// <summary>
        /// 根据条件成立再构建 Where 查询
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="expression">表达式</param>
        /// <returns>新的集合对象</returns>
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> sources, bool condition, Func<TSource, bool> expression)
        {
            return condition ? sources.Where(expression) : sources;
        }

        /// <summary>
        /// 根据条件成立再构建 Where 查询，支持索引器
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="expression">表达式</param>
        /// <returns>新的集合对象</returns>
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> sources, bool condition, Func<TSource, int, bool> expression)
        {
            return condition ? sources.Where(expression) : sources;
        }

        /// <summary>
        /// 根据条件成立再构建 Where 查询
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="expression">表达式</param>
        /// <returns>新的集合对象</returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> sources, bool condition, Expression<Func<TSource, bool>> expression)
        {
            return condition ? Queryable.Where(sources, expression) : sources;
        }

        /// <summary>
        /// 根据条件成立再构建 Where 查询，支持索引器
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="expression">表达式</param>
        /// <returns>新的集合对象</returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> sources, bool condition, Expression<Func<TSource, int, bool>> expression)
        {
            return condition ? Queryable.Where(sources, expression) : sources;
        }

        /// <summary>
        /// 与操作合并多个表达式
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="expressions">表达式数组</param>
        /// <returns>新的集合对象</returns>
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> sources, params Expression<Func<TSource, bool>>[] expressions)
        {
            if (expressions == null || expressions.Length == 0)
            {
                return sources;
            }

            if (expressions.Length == 1)
            {
                return Queryable.Where(sources, expressions[0]);
            }

            Expression<Func<TSource, bool>> expression = LinqExtensions.Or<TSource>();
            foreach (Expression<Func<TSource, bool>> _expression in expressions)
            {
                expression = expression.Or(_expression);
            }

            return Queryable.Where(sources, expression);
        }

        /// <summary>
        /// 与操作合并多个表达式，支持索引器
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="expressions">表达式数组</param>
        /// <returns>新的集合对象</returns>
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> sources, params Expression<Func<TSource, int, bool>>[] expressions)
        {
            if (expressions == null || expressions.Length == 0)
            {
                return sources;
            }

            if (expressions.Length == 1)
            {
                return Queryable.Where(sources, expressions[0]);
            }

            Expression<Func<TSource, int, bool>> expression = LinqExtensions.IndexOr<TSource>();
            foreach (Expression<Func<TSource, int, bool>> _expression in expressions)
            {
                expression = expression.Or(_expression);
            }

            return Queryable.Where(sources, expression);
        }

        /// <summary>
        /// 根据条件成立再构建 WhereOr 查询
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="conditionExpressions">条件表达式</param>
        /// <returns>新的集合对象</returns>
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> sources, params (bool condition, Expression<Func<TSource, bool>> expression)[] conditionExpressions)
        {
            List<Expression<Func<TSource, bool>>> expressions = [];

            foreach ((bool condition, Expression<Func<TSource, bool>> expression) in conditionExpressions)
            {
                if (condition) expressions.Add(expression);
            }

            return sources.Where(expressions.ToArray());
        }

        /// <summary>
        /// 根据条件成立再构建 WhereOr 查询，支持索引器
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="conditionExpressions">条件表达式</param>
        /// <returns>新的集合对象</returns>
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> sources, params (bool condition, Expression<Func<TSource, int, bool>> expression)[] conditionExpressions)
        {
            List<Expression<Func<TSource, int, bool>>> expressions = [];

            foreach ((bool condition, Expression<Func<TSource, int, bool>> expression) in conditionExpressions)
            {
                if (condition) expressions.Add(expression);
            }

            return sources.Where(expressions.ToArray());
        }

        /// <summary>
        /// 根据条件成立再构建 Include 查询
        /// </summary>
        /// <typeparam name="TSource">泛型类型</typeparam>
        /// <typeparam name="TProperty">泛型属性类型</typeparam>
        /// <param name="sources">集合对象</param>
        /// <param name="condition">布尔条件</param>
        /// <param name="expression">新的集合对象表达式</param>
        /// <returns>新的导航属性查询</returns>
        public static IIncludableQueryable<TSource, TProperty> IncludeIf<TSource, TProperty>(this IQueryable<TSource> sources, bool condition, Expression<Func<TSource, TProperty>> expression) where TSource : class
        {
            return condition ? sources.Include(expression) : (IIncludableQueryable<TSource, TProperty>)sources;
        }

        /// <summary>
        /// 条件性地执行嵌套 Include 操作
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TPreviousProperty">前一个导航属性的类型</typeparam>
        /// <typeparam name="TProperty">当前导航属性的类型</typeparam>
        /// <param name="source">可包含的查询源</param>
        /// <param name="condition">决定是否执行 Include 的布尔值</param>
        /// <param name="navigationPropertyPath">用于 Include 的导航属性表达式</param>
        /// <returns>新的导航属性查询</returns>
        public static IIncludableQueryable<TEntity, TProperty> ThenIncludeIf<TEntity, TPreviousProperty, TProperty>(this IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source, bool condition, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath) where TEntity : class
        {
            return condition ? source.ThenInclude(navigationPropertyPath) : (IIncludableQueryable<TEntity, TProperty>)source;
        }

        /// <summary>
        /// 执行嵌套 Include 操作
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TPreviousProperty">前一个导航属性的类型</typeparam>
        /// <typeparam name="TProperty">当前导航属性的类型</typeparam>
        /// <param name="source">可包含的查询源</param>
        /// <param name="condition">决定是否执行 Include 的布尔值</param>
        /// <param name="navigationPropertyPath">用于 Include 的导航属性表达式</param>
        /// <returns>新的导航属性查询</returns>
        public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IIncludableQueryable<TEntity, TPreviousProperty> source, bool condition, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath) where TEntity : class
        {
            return condition ? source.ThenInclude(navigationPropertyPath) : (IIncludableQueryable<TEntity, TProperty>)source;
        }
    }
}
