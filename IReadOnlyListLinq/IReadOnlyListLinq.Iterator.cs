using System.Collections;
using System.Collections.Generic;

namespace IReadOnlyListLinq
{
    public static partial class IReadOnlyListLinq
    {
        private abstract class Iterator<T> : IReadOnlyList<T>, IEnumerator<T>
        {
            protected bool _enumeratorTaken;
            protected T _current;
            protected int _index = -1;

            public abstract T this[int index] { get; }
            public abstract int Count { get; }
            public T Current => _current;
            object IEnumerator.Current => Current;

            public abstract Iterator<T> Clone();
            public IEnumerator<T> GetEnumerator()
            {
                var enumerator = _enumeratorTaken ? Clone() : this;
                enumerator._enumeratorTaken = true;
                return enumerator;
            }

            public abstract bool MoveNext();

            public virtual void Dispose()
            {
                _current = default;
                _index = -1;
            }

            public void Reset()
            {
                _current = default;
                _index = -1;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
