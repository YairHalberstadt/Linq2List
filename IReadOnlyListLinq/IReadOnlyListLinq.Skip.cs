using System;
using System.Collections.Generic;

namespace IReadOnlyListLinq
{
	public static partial class IReadOnlyListLinq
	{
		public static IReadOnlyList<TSource> Skip<TSource>(this IReadOnlyList<TSource> source, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (count <= 0)
			{
				if (source is Iterator<TSource>)
				{
					return source;
				}

				count = 0;
			}

			return new SkipIterator<TSource>(source, count);
		}

		private sealed class SkipIterator<TSource> : Iterator<TSource>
		{
			private readonly IReadOnlyList<TSource> _source;
			private readonly int _skipCount;
			private int _count = -1;

			public SkipIterator(IReadOnlyList<TSource> source, int count)
			{
				_source = source;
				_skipCount = count;
			}

			public sealed override TSource this[int index] => index >= 0 ? _source[index + _skipCount] : throw new ArgumentOutOfRangeException(nameof(index));

			public sealed override int Count
			{
				get
				{
					var count = _source.Count - _skipCount;
					return count > 0 ? count : 0;
				}
			}

			public sealed override Iterator<TSource> Clone()
			{
				return new SkipIterator<TSource>(_source, _skipCount);
			}

			public sealed override bool MoveNext()
			{
				if (_count == -1)
					_count = Count;
				if (++index >= _count)
				{
					current = default;
					return false;
				}
				current = _source[index + _skipCount];
				return true;
			}
		}
	}
}