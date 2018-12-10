using System;
using System.Collections.Generic;

namespace ListLinq
{
    public static partial class ReadOnlyList
    {
        public static IReadOnlyList<TResult> Zip<TFirst, TSecond, TResult>(this IReadOnlyList<TFirst> first, IReadOnlyList<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return new ZipIterator<TFirst, TSecond, TResult>(first, second, resultSelector);
        }

        private sealed class ZipIterator<TFirst, TSecond, TResult> :  Iterator<TResult>
        {
            private readonly IReadOnlyList<TFirst> _first;
            private readonly IReadOnlyList<TSecond> _second;
            private readonly Func<TFirst, TSecond, TResult> _resultSelector;

            public ZipIterator(IReadOnlyList<TFirst> first, IReadOnlyList<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
            {
                _first = first;
                _second = second;
                _resultSelector = resultSelector;
            }

            public sealed override TResult this[int index] => _resultSelector(_first[index], _second[index]);

            public sealed override int Count => Math.Min(_first.Count, _second.Count);

            private int _count = -1;

            public sealed override Iterator<TResult> Clone()
            {
                return new ZipIterator<TFirst, TSecond, TResult>(_first, _second, _resultSelector);
            }

            public sealed override bool MoveNext()
            {
                if (_count == -1)
                    _count = Count;
                if(++index >= _count)
                {
                    current = default;
                    return false;
                }
                current = _resultSelector(_first[index], _second[index]);
                return true;
            }
        }

		public static IReadOnlyList<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IReadOnlyList<TFirst> first, IReadOnlyList<TSecond> second)
		{
			if (first == null)
			{
				throw new ArgumentNullException(nameof(first));
			}

			if (second == null)
			{
				throw new ArgumentNullException(nameof(second));
			}

			return new ZipIterator<TFirst, TSecond>(first, second);
		}

		private sealed class ZipIterator<TFirst, TSecond> : Iterator<(TFirst, TSecond)>
		{
			private readonly IReadOnlyList<TFirst> _first;
			private readonly IReadOnlyList<TSecond> _second;

			public ZipIterator(IReadOnlyList<TFirst> first, IReadOnlyList<TSecond> second)
			{
				_first = first;
				_second = second;
			}

			public sealed override (TFirst, TSecond) this[int index] => (_first[index], _second[index]);

			public sealed override int Count => Math.Min(_first.Count, _second.Count);

			private int _count = -1;

			public sealed override Iterator<(TFirst, TSecond)> Clone()
			{
				return new ZipIterator<TFirst, TSecond>(_first, _second);
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
				current = (_first[index], _second[index]);
				return true;
			}
		}
	}
}
