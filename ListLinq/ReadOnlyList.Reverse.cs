using System;
using System.Collections.Generic;

namespace Linq2List
{
    public static partial class ReadOnlyList
    {
		public static IReadOnlyList<TSource> Reverse<TSource>(this IReadOnlyList<TSource> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			return new ReverseIterator<TSource>(source);
		}

		/// <summary>
		/// An iterator that yields the items of an <see cref="IEnumerable{TSource}"/> in reverse.
		/// </summary>
		/// <typeparam name="TSource">The type of the source enumerable.</typeparam>
		private sealed class ReverseIterator<TSource> : Iterator<TSource>
		{
			private readonly IReadOnlyList<TSource> _source;

			public ReverseIterator(IReadOnlyList<TSource> source)
			{
				_source = source;
			}


			public sealed override bool MoveNext()
			{
				if (index == -1)
					index = Count;
				if (--index < 0)
				{
					current = default;
					return false;
				}
				current = _source[index];
				return true;
			}

			public sealed override TSource this[int index] => _source[Count - index - 1];

			public sealed override int Count => _source.Count;
			public sealed override Iterator<TSource> Clone()
			{
				return new ReverseIterator<TSource>(_source);
			}
		}
	}
}
