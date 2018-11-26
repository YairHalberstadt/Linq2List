using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ListLinq
{
    public static partial class ReadOnlyList
    {
		public static IReadOnlyList<TSource> Concat<TSource>(this IReadOnlyList<TSource> first, IReadOnlyList<TSource> second)
		{
			if (first == null)
			{
				throw new ArgumentNullException(nameof(first));
			}

			if (second == null)
			{
				throw new ArgumentNullException(nameof(second));
			}

			return new ConcatIterator<TSource>(first, second);
		}

		private sealed class ConcatIterator<TSource> : Iterator<TSource>
		{
			private readonly IReadOnlyList<TSource> _first;
			private readonly IReadOnlyList<TSource> _second;

			public ConcatIterator(IReadOnlyList<TSource> first, IReadOnlyList<TSource> second)
			{
				_first = first;
				_second = second;
			}

			public sealed override TSource this[int index]
			{
				get
				{
					var firstCount = _first.Count;
					return index < firstCount ? _first[index] : _second[index - firstCount];
				}
			}

			public sealed override int Count => checked(_first.Count + _second.Count);

			private int _count = -1;

			public sealed override Iterator<TSource> Clone()
			{
				return new ConcatIterator<TSource>(_first, _second);
			}

			private bool onSecond;
			public sealed override bool MoveNext()
			{
				if (_count == -1)
					_count = _first.Count;
				if (++index >= _count)
				{
					if (onSecond)
					{
						current = default;
						return false;
					}

					_count = _second.Count;
					if (_count == 0)
					{
						current = default;
						return false;
					}

					index = 0;
					onSecond = true;
				}
				current = onSecond ? _second[index] : _first[index];
				return true;
			}
		}
	}
}
