using System;
using System.Collections.Generic;

namespace Linq2List
{
	public static partial class ReadOnlyList
	{
		public static IReadOnlyList<TResult> Repeat<TResult>(TResult element, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			if (count == 0)
			{
				return Empty<TResult>();
			}

			return new RepeatIterator<TResult>(element, count);
		}

		private sealed class RepeatIterator<TResult> : Iterator<TResult>
		{
			private readonly TResult _element;
			private readonly int _count;

			public RepeatIterator(TResult element, int count)
			{
				_element = element;
				_count = count;
			}

			public override TResult this[int index] => index >= 0 && index < _count ? _element : throw new ArgumentOutOfRangeException(nameof(index));

			public override int Count => _count;
			public override Iterator<TResult> Clone()
			{
				return new RepeatIterator<TResult>(_element, _count);
			}

			public override bool MoveNext()
			{
				if (index == -1)
				{
					current = _element;
				}
				if (++index >= _count)
				{
					current = default;
					return false;
				}
				return true;
			}
		}
	}
}