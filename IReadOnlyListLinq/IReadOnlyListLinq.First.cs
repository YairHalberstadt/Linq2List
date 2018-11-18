﻿using System;
using System.Collections.Generic;

namespace IReadOnlyListLinq
{
    public static partial class IReadOnlyListLinq
    {
		public static TSource First<TSource>(this IReadOnlyList<TSource> source)
		{
			TSource first = source.TryGetFirst(out bool found);
			if (!found)
			{
				throw new InvalidOperationException($"{nameof(source)} contains no Elements");
			}

			return first;
		}

		public static TSource First<TSource>(this IReadOnlyList<TSource> source, Func<TSource, bool> predicate)
		{
			TSource first = source.TryGetFirst(predicate, out bool found);
			if (!found)
			{
				throw new InvalidOperationException($"{nameof(source)} does not contain any element which matches {nameof(predicate)}");
			}

			return first;
		}

		public static TSource FirstOrDefault<TSource>(this IReadOnlyList<TSource> source) =>
			source.TryGetFirst(out bool _);

		public static TSource FirstOrDefault<TSource>(this IReadOnlyList<TSource> source, Func<TSource, bool> predicate) =>
			source.TryGetFirst(predicate, out bool _);

		private static TSource TryGetFirst<TSource>(this IReadOnlyList<TSource> source, out bool found)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source.Count > 0)
			{
				found = true;
				return source[0];
			}
			found = false;
			return default;
		}

		private static TSource TryGetFirst<TSource>(this IReadOnlyList<TSource> source, Func<TSource, bool> predicate, out bool found)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			for (int i = 0; i < source.Count; i++)
			{
				TSource element = source[i];
				if (predicate(element))
				{
					found = true;
					return element;
				}
			}

			found = false;
			return default;
		}
	}
}