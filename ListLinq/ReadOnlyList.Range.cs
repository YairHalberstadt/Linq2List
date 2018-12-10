using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Linq2List
{
	public static partial class ReadOnlyList
	{
		public static IReadOnlyList<int> Range(int start, int count)
		{
			long max = ((long)start) + count - 1;
			if (count < 0 || max > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			if (count == 0)
			{
				return Empty<int>();
			}

			return new RangeIterator(start, count);
		}

		private class RangeIterator : Iterator<int>
		{
			private readonly int _start;
			private readonly int _count;

			public RangeIterator(int start, int count)
			{
				Debug.Assert(count > 0);
				_start = start;
				_count = count;
			}

			public override int this[int index] => index >= 0 && index < _count ? _start + index : throw new ArgumentOutOfRangeException(nameof(index));

			public override int Count => _count;
			public override Iterator<int> Clone()
			{
				return new RangeIterator(_start, _count);
			}

			public override bool MoveNext()
			{
				if (index == -1)
				{
					current = _start;
					index++;
					return true;
				}
				if (++index >= _count)
				{
					current = default;
					return false;
				}

				current++;
				return true;
			}
		}
	}
}