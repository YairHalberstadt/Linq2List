using System;
using System.Collections.Generic;

namespace ListLinq
{
	public static partial class ReadOnlyList
	{
		public static IReadOnlyList<TSource> Take<TSource>(this IReadOnlyList<TSource> source, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			return count <= 0 ? (IReadOnlyList<TSource>)Array.Empty<TSource>() : new TakeIterator<TSource>(source, count);
		}

		private sealed class TakeIterator<TSource> : Iterator<TSource>
		{
			private readonly IReadOnlyList<TSource> _source;
			private readonly int _takeCount;
			private int _count = -1;

			public TakeIterator(IReadOnlyList<TSource> source, int count)
			{
				_source = source;
				_takeCount = count;
			}

			public sealed override TSource this[int index] => index < _takeCount ? _source[index] : throw new ArgumentOutOfRangeException(nameof(index));

			public sealed override int Count => Math.Min(_takeCount, _source.Count);

			public sealed override Iterator<TSource> Clone()
			{
				return new TakeIterator<TSource>(_source, _takeCount);
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
				current = _source[index];
				return true;
			}
		}

	}
}