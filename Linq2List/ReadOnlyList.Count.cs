using System.Collections.Generic;

namespace Linq2List
{
	public static partial class ReadOnlyList
	{
		/// <summary>
		/// Calls <see cref="IReadOnlyList{TSource}"/>.Count
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static int Count<TSource>(this IReadOnlyList<TSource> source) => source.Count;

		/// <summary>
		/// Calls <see cref="IReadOnlyList{TSource}"/>.Count
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source"></param>
		/// <returns>The returned value is guaranteed to be smaller than int.MaxValue and can be safely cast to an int without information loss</returns>
		public static long LongCount<TSource>(this IReadOnlyList<TSource> source) => source.Count;
	}
}