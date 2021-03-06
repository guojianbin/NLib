﻿namespace NLib.Collections.Generic.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Defines extensions methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Filters sequence that are between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IEnumerable<TSource> Between<TSource>(this IEnumerable<TSource> source, TSource min, TSource max)
        {
            return Between(source, min, max, Comparer<TSource>.Default);
        }

        /// <summary>
        /// Filters sequence that are between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="comparer">The <see cref="IComparer{TSource}"/> to compare values.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is null.</exception>
        public static IEnumerable<TSource> Between<TSource>(this IEnumerable<TSource> source, TSource min, TSource max, IComparer<TSource> comparer)
        {
            return Between(source, x => x, min, max, comparer);
        }

        /// <summary>
        /// Filters sequence that are between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is null.</exception>
        public static IEnumerable<TSource> Between<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, TKey min, TKey max)
        {
            return Between(source, keySelector, min, max, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Filters sequence that are between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="comparer">The <see cref="IComparer{TSource}"/> to compare values.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="keySelector"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is null.</exception>
        public static IEnumerable<TSource> Between<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, TKey min, TKey max, IComparer<TKey> comparer)
        {
            Check.Current.ArgumentNullException(source, "source")
                         .ArgumentNullException(keySelector, "keySelector")
                         .ArgumentNullException(comparer, "comparer");

            return source.Where(x => comparer.Compare(keySelector(x), min) >= 0 && comparer.Compare(keySelector(x), max) <= 0);
        }

        /// <summary>
        /// Performs the specified action on each element of the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="action">The <see cref=" Action{T}"/> delegate to perform on each element of the <see cref="IEnumerable{T}"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="collection"/> must not be null.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> must not be null.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "CheckError class do the check")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "CheckError class do the check")]
        public static void ForEach<TSource>(this IEnumerable<TSource> collection, Action<TSource> action)
        {
            Check.Current.ArgumentNullException(collection, "collection")
                         .ArgumentNullException(action, "action");
            
            var list = collection as List<TSource>;
            if (list != null)
            {
                list.ForEach(action);
            }
            else
            {
                foreach (var v in collection)
                {
                    action(v);
                }
            }
        }

        /// <summary>
        /// Paginates the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="page">The zero-based page number.</param>
        /// <param name="pageSize">Size of a page.</param>
        /// <returns>The subset of the collection.</returns>
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> collection, int page, int pageSize)
        {
            Check.Current.ArgumentNullException(collection, "collection");

            var skip = Math.Max(pageSize * page, 0);

            return collection.Skip(skip).Take(pageSize);
        }
    }
}
