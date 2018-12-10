using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Linq2List.Tests.Unit
{
    public abstract class ListLinqTests
    {
        protected static bool IsEven(int num) => num % 2 == 0;

		protected class InfiniteList<T> : IReadOnlyList<T>, IEnumerator<T>
		{
			public IEnumerator<T> GetEnumerator()
			{
				return this;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this;
			}

			public int Count => int.MaxValue;

			public T this[int index] => default;

			public bool MoveNext() => true;

			public void Reset()
			{
			}

			public T Current => default;

			object IEnumerator.Current => default;

			public void Dispose()
			{
			}
		}

        protected class AnagramEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x == null | y == null) return false;
                int length = x.Length;
                if (length != y.Length) return false;
                using (var en = x.OrderBy(i => i).GetEnumerator())
                {
                    foreach (char c in y.OrderBy(i => i))
                    {
                        en.MoveNext();
                        if (c != en.Current) return false;
                    }
                }
                return true;
            }

            public int GetHashCode(string obj)
            {
                if (obj == null) return 0;
                int hash = obj.Length;
                foreach (char c in obj)
                    hash ^= c;
                return hash;
            }
        }

        protected struct StringWithIntArray
        {
            public string name { get; set; }
            public int?[] total { get; set; }
        }


        protected static List<Func<IReadOnlyList<T>, IReadOnlyList<T>>> IdentityTransforms<T>()
        {
            // All of these transforms should take an enumerable and produce
            // another enumerable with the same contents.
            return new List<Func<IReadOnlyList<T>, IReadOnlyList<T>>>
            {
                e => e,
                e => e.ToArray(),
                e => e.ToList(),
                e => e.Select(i => i),
            };
        }

        protected class ThrowsOnMatchReadOnlyList<T> : IReadOnlyList<T>
        {
            private readonly IReadOnlyList<T> _data;
            private readonly T _thrownOn;

            public ThrowsOnMatchReadOnlyList(IReadOnlyList<T> source, T thrownOn)
            {
                _data = source;
                _thrownOn = thrownOn;
            }

            public T this[int index]
            {
                get
                {
                    var datum = _data[index];
                    if (datum.Equals(_thrownOn)) throw new Exception();
                    return datum;
                }
            }

            public int Count => _data.Count;

            public IEnumerator<T> GetEnumerator()
            {
                foreach (var datum in _data)
                {
                    if (datum.Equals(_thrownOn)) throw new Exception();
                    yield return datum;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
