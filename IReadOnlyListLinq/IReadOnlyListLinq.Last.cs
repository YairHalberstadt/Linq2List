using System;
using System.Collections.Generic;

namespace IReadOnlyListLinq
{
	public static partial class IReadOnlyListLinq
	{
		public static TSource Last<TSource>(this IReadOnlyList<TSource> source)
		{
			TSource last = source.TryGetLast(out bool found);
			if (!found)
			{
				throw new InvalidOperationException($"{nameof(source)} contains no Elements");
			}

			return last;
		}

		public static TSource Last<TSource>(this IReadOnlyList<TSource> source, Func<TSource, bool> predicate)
		{
			TSource last = source.TryGetLast(predicate, out bool found);
			if (!found)
			{
				throw new InvalidOperationException(
					$"{nameof(source)} does not contain any element which matches {nameof(predicate)}");
			}

			return last;
		}

		public static TSource LastOrDefault<TSource>(this IReadOnlyList<TSource> source) =>
			source.TryGetLast(out bool _);

		public static TSource
			LastOrDefault<TSource>(this IReadOnlyList<TSource> source, Func<TSource, bool> predicate) =>
			source.TryGetLast(predicate, out bool _);

		private static TSource TryGetLast<TSource>(this IReadOnlyList<TSource> source, out bool found)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			int count = source.Count;
			if (count > 0)
			{
				found = true;
				return source[count - 1];
			}


			found = false;
			return default;
		}

		private static TSource TryGetLast<TSource>(this IReadOnlyList<TSource> source, Func<TSource, bool> predicate,
			out bool found)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (predicate == null)
			{
				throw new ArgumentNullException(nameof(predicate));
			}

			for (int i = source.Count - 1; i >= 0; --i)
			{
				TSource result = source[i];
				if (predicate(result))
				{
					found = true;
					return result;
				}
			}

			found = false;
			return default;
		}
	}
}