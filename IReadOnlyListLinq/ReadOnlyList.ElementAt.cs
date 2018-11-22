using System;
using System.Collections.Generic;

namespace ListLinq
{
    public static partial class ReadOnlyList
    {
		public static TSource ElementAt<TSource>(this IReadOnlyList<TSource> source, int index)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			return source[index];
		}

		/// <summary>
		/// Assumes the <see cref="source"/> is 0 based, and has Count items.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static TSource ElementAtOrDefault<TSource>(this IReadOnlyList<TSource> source, int index)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (index < 0 || index >= source.Count)
				return default;
			return source[index];
		}
	}
}
