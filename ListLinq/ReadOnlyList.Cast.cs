using System;
using System.Collections.Generic;

namespace ListLinq
{
    public static partial class ReadOnlyList
    {
		public static IReadOnlyList<TResult> Cast<TSource, TResult>(this IReadOnlyList<TSource> source)
		{
            if (source is IReadOnlyList<TResult> result)
                return result;
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            return new CastIterator<TSource, TResult>(source);
		}

        private sealed class CastIterator<TSource, TResult> : Iterator<TResult>
        {
            private readonly IReadOnlyList<TSource> _source;

            public CastIterator(IReadOnlyList<TSource> source)
            {
                _source = source;
            }

            public sealed override TResult this[int index] => (TResult)(object)_source[index];

            public sealed override int Count => _source.Count;

            public sealed override Iterator<TResult> Clone() =>
                new CastIterator<TSource, TResult>(_source);

            private int _count = -1;
            public sealed override bool MoveNext()
            {
                if (_count == -1)
                    _count = Count;
                if (++index >= _count)
                {
                    current = default;
                    return false;
                }
                current = (TResult)(object)(_source[index]);
                return true;
            }
        }

		/// <summary>
		/// This cannot be used for an IReadOnlyList of type T where T is a struct. Use the overload with two type arguments instead.
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
        public static IReadOnlyList<TResult> Cast<TResult>(this IReadOnlyList<object> source)
        {
            if (source is IReadOnlyList<TResult> result)
                return result;
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            return new CastIterator<TResult>(source);
        }

        private sealed class CastIterator<TResult> : Iterator<TResult>
        {
            private readonly IReadOnlyList<object> _source;

            public CastIterator(IReadOnlyList<object> source)
            {
                _source = source;
            }

            public sealed override TResult this[int index] => (TResult)_source[index];

            public sealed override int Count => _source.Count;

            public sealed override Iterator<TResult> Clone() =>
                new CastIterator<TResult>(_source);

            private int _count = -1;
            public sealed override bool MoveNext()
            {
                if (_count == -1)
                    _count = Count;
                if (++index >= _count)
                {
                    current = default;
                    return false;
                }
                current = (TResult)(_source[index]);
                return true;
            }
        }
    }
}
