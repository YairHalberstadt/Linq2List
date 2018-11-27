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
			private readonly IReadOnlyList<TSource>[] _sources;

			public ConcatIterator(IReadOnlyList<TSource> first, IReadOnlyList<TSource> second)
			{

				if (first is ConcatIterator<TSource> concatenatedFirst)
				{
					if (second is ConcatIterator<TSource> concatenatedSecond)
					{
						_sources = new IReadOnlyList<TSource>[concatenatedFirst._sources.Length +
															concatenatedSecond._sources.Length];
						Array.Copy(concatenatedFirst._sources, 0, _sources, 0, concatenatedFirst._sources.Length);
						Array.Copy(concatenatedSecond._sources, 0, _sources, concatenatedFirst._sources.Length, concatenatedSecond._sources.Length);
					}
					else
					{
						_sources = new IReadOnlyList<TSource>[concatenatedFirst._sources.Length + 1];
						Array.Copy(concatenatedFirst._sources, 0, _sources, 0, concatenatedFirst._sources.Length);
						_sources[_sources.Length - 1] = second;
					}
				}
				else
				{
					if (second is ConcatIterator<TSource> concatenatedSecond)
					{
						_sources = new IReadOnlyList<TSource>[concatenatedSecond._sources.Length + 1];
						_sources[0] = first;
						Array.Copy(concatenatedSecond._sources, 0, _sources, 1, concatenatedSecond._sources.Length);
					}
					else
					{
						_sources = new []
						{
							first,
							second,
						};
					}
				}
			}

			public ConcatIterator(IReadOnlyList<TSource>[] sources)
			{
				_sources = sources;
			}

			public sealed override TSource this[int index]
			{
				get
				{
					var countSum = 0;
					foreach (var source in _sources)
					{
						var newCountSum = checked(source.Count + countSum);
						if (index < newCountSum)
							return source[index - countSum];
						countSum = newCountSum;
					}

					throw new ArgumentOutOfRangeException(nameof(index));
				}
			}

			public sealed override int Count
			{
				get
				{
					var countSum = 0;
					foreach (var source in _sources)
					{
						checked
						{
							countSum += source.Count;
						}
					}
					return countSum;
				}
			}

			private int _count = -1;

			public sealed override Iterator<TSource> Clone()
			{
				return new ConcatIterator<TSource>(_sources);
			}

			private int currentSourceIndex;
			public sealed override bool MoveNext()
			{
				if(currentSourceIndex >= _sources.Length)
				{
					current = default;
					return false;
				}

				if (_count == -1)
					_count = _sources[currentSourceIndex].Count;

				if (++index >= _count)
				{
					currentSourceIndex++;
					_count = -1;
					index = -1;
					return MoveNext();
				}

				current = _sources[currentSourceIndex][index];
				return true;
			}
		}
	}
}
