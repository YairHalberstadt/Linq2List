using System;
using System.Collections.Generic;

namespace IReadOnlyListLinq
{
    public static partial class IReadOnlyListLinq
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
            private IReadOnlyList<TFirst> first;
            private IReadOnlyList<TSecond> second;
            private Func<TFirst, TSecond, TResult> resultSelector;

            public ZipIterator(IReadOnlyList<TFirst> first, IReadOnlyList<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
            {
                this.first = first;
                this.second = second;
                this.resultSelector = resultSelector;
            }

            public sealed override TResult this[int index] => resultSelector(first[index], second[index]);

            public sealed override int Count => Math.Min(first.Count, second.Count);

            private int _count = -1;

            public sealed override Iterator<TResult> Clone()
            {
                return new ZipIterator<TFirst, TSecond, TResult>(first, second, resultSelector);
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
                current = resultSelector(first[index], second[index]);
                return true;
            }
        }
	}
}
