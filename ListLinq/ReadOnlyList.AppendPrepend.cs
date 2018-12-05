using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ListLinq
{
	public static partial class ReadOnlyList
	{
		public static IReadOnlyList<TSource> Append<TSource>(this IReadOnlyList<TSource> source,
			TSource element)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source is AppendPrependIterator<TSource> appendPrependIterator)
				return appendPrependIterator.Append(element);

			return new AppendPrepend1Iterator<TSource>(source, element, true);
		}

		public static IReadOnlyList<TSource> Prepend<TSource>(this IReadOnlyList<TSource> source, TSource element)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (source is AppendPrependIterator<TSource> appendPrependIterator)
				return appendPrependIterator.Prepend(element);

			return new AppendPrepend1Iterator<TSource>(source, element, false);
		}

		private abstract class AppendPrependIterator<TSource> : Iterator<TSource>
		{
			public abstract AppendPrependIterator<TSource> Append(TSource item);

			public abstract AppendPrependIterator<TSource> Prepend(TSource item);
		}

		private sealed class AppendPrepend1Iterator<TSource> : AppendPrependIterator<TSource>
		{
			//These are internal in order to give AppendPrependNIterator access to them
			internal readonly IReadOnlyList<TSource> _source;
			internal TSource _item;
			internal bool _isAppended;

			public AppendPrepend1Iterator(IReadOnlyList<TSource> source, TSource item, bool append)
			{
				_source = source;
				_item = item;
				_isAppended = append;
			}

			public override TSource this[int index]
			{
				get
				{
					if (_isAppended)
					{
						if (index == _source.Count)
							return _item;
						return _source[index];
					}

					if (index == 0)
						return _item;
					return _source[index - 1];
				}
			}

			public override int Count => _source.Count + 1;

			public override Iterator<TSource> Clone()
			{
				return new AppendPrepend1Iterator<TSource>(_source, _item, _isAppended);
			}

			private int _count = -1;

			public override bool MoveNext()
			{
				if (index == -1)
				{
					index = 0;
					if (!_isAppended)
					{
						current = _item;
						return true;
					}
				}
				if (_count == -1)
					_count = _source.Count;
				if (index >= _count)
				{
					if (!_isAppended)
					{
						current = default;
						return false;
					}

					if (index > _count)
					{
						current = default;
						return false;
					}

					index++;
					current = _item;
					return true;
				}

				current = _source[index];
				index++;
				return true;
			}

			public override AppendPrependIterator<TSource> Append(TSource item)
			{
				return new AppendPrependNIterator<TSource>(this, item, true);
			}

			public override AppendPrependIterator<TSource> Prepend(TSource item)
			{
				return new AppendPrependNIterator<TSource>(this, item, false);
			}
		}

		private sealed class AppendPrependNIterator<TSource> : AppendPrependIterator<TSource>
		{
			private readonly IReadOnlyList<TSource> _source;
			private readonly List<TSource> _prepended;
			private readonly List<TSource> _appended;
			private readonly int _prependedCount;
			private readonly int _appendedCount;

			/// <summary>
			/// This method may look like a bit of magic is going on, so I'll explain it here
			/// If we append an item to an <see cref = "AppendPrependNIterator{TSource}"/> we just add an item to the end of the <see cref = "_appended"/> list of the original iterator, and increases its personal <see cref = "_appendedCount"/> by 1.
			/// However, if the <see cref = "AppendPrependNIterator{TSource}"/> already has an item appended to the end of its <see cref = "_appended"/> list, we cannot do that.
			/// Instead we have a choice:
			/// 1. We can copy the <see cref = "_appended"/> list, excluding the extra appended items.
			/// 2. We can treat the iterator like any other list, and set it as the <see cref = "_source"/>
			/// Both have costs.
			/// The first has a cost proportional to the <see cref = "_appendedCount"/>.
			/// The second increases the level of recursion when indexing into the list by one, and so when iterating through the list has a cost proportional to the total length of the existing iterator
			/// The total cost can be minimized by taking the first option when the list is small, but the second when the list is large.
			/// How big is small and large?
			/// A little bit of calculus tells us that in the worst case scenario, the length that a list has to be before we copy it should be proportional to the square root of the total length of the iterator.
			/// What is that proportion?
			/// A little bit of preliminary testing suggests it to be around the 10 - 15 region, leaning towards 15. Hopefully I'll benchmark it more thoroughly in the future. This is the <see cref = "recursionCostMultiplier"/>
			///
			/// Everything above applies to prepending as well of course
			/// </summary>
			/// <param name="source"></param>
			/// <param name="item"></param>
			/// <param name="append"></param>
			private AppendPrependNIterator(AppendPrependNIterator<TSource> source, TSource item, bool append)
			{
				const int recursionCostMultiplier = 15;
				_source = source._source;
				if (append)
				{
					if (source._appended.Count > source._appendedCount && source._appendedCount > Math.Sqrt(source._source.Count) * recursionCostMultiplier)
					{
						_source = source;
						_appended = new List<TSource>(1) { item };
						_prepended = new List<TSource>();
						_appendedCount = 1;
						_prependedCount = 0;
					}
					else
					{
						_appended = source._appended.Count > source._appendedCount
							? source._appended.GetRange(0, source._appendedCount)
							: source._appended;
						_prepended = source._prepended;
						_prependedCount = source._prependedCount;
						_appendedCount = source._appendedCount + 1;
						_appended.Add(item);
					}
				}
				else
				{
					if (source._prepended.Count > source._prependedCount && source._prependedCount > Math.Sqrt(source._source.Count) * recursionCostMultiplier)
					{
						_source = source;
						_appended = new List<TSource>();
						_prepended = new List<TSource>(1) { item };
						_appendedCount = 0;
						_prependedCount = 1;
					}
					else
					{
						_appended = source._appended;
						_prepended = source._prepended.Count > source._prependedCount
							? source._prepended.GetRange(0, source._prependedCount)
							: source._prepended;
						_prependedCount = source._prependedCount + 1;
						_appendedCount = source._appendedCount;
						_prepended.Add(item);
					}
				}
			}

			private AppendPrependNIterator(AppendPrependNIterator<TSource> source)
			{
				_source = source._source;
				_prepended = source._prepended;
				_appended = source._appended;
				_prependedCount = source._prependedCount;
				_appendedCount = source._appendedCount;
			}

			public AppendPrependNIterator(AppendPrepend1Iterator<TSource> source, TSource item, bool append)
			{
				_source = source._source;
				if (append)
				{
					if (source._isAppended)
					{
						_appended = new List<TSource>(2) { source._item, item };
						_prepended = new List<TSource>(0);
						_appendedCount = 2;
						_prependedCount = 0;
					}
					else
					{
						_appended = new List<TSource>(1) { item };
						_prepended = new List<TSource>(1) { source._item };
						_appendedCount = 1;
						_prependedCount = 1;
					}
				}
				else
				{
					if (source._isAppended)
					{
						_appended = new List<TSource>(1) { source._item };
						_prepended = new List<TSource>(1) { item };
						_appendedCount = 1;
						_prependedCount = 1;
					}
					else
					{
						_appended = new List<TSource>(0);
						_prepended = new List<TSource>(2) { source._item, item };
						_appendedCount = 0;
						_prependedCount = 2;
					}
				}
			}

			public sealed override TSource this[int index]
			{
				get
				{
					if (index < 0)
						throw new ArgumentOutOfRangeException(nameof(index));
					if (index < _prependedCount)
						return _prepended[_prependedCount - index - 1];
					var sourceCount = _source.Count;
					if (index < sourceCount + _prependedCount)
						return _source[index - _prependedCount];
					if (index < sourceCount + _prependedCount + _appendedCount)
						return _appended[index - sourceCount - _prependedCount];
					throw new ArgumentOutOfRangeException(nameof(index));
				}
			}

			public sealed override int Count => _prependedCount + _source.Count + _appendedCount;


			public sealed override Iterator<TSource> Clone()
			{
				return new AppendPrependNIterator<TSource>(this);
			}

			private int _count = -1;
			private int _state = 0;

			public sealed override bool MoveNext()
			{
				switch (_state)
				{
					case 0:
						if (++index >= _prependedCount)
						{
							_state = 1;
							index = -1;
							return MoveNext();
						}

						current = _prepended[_prependedCount - index - 1];
						return true;
					case 1:
						if (_count == -1)
							_count = _source.Count;
						if (++index >= _count)
						{
							_state = 2;
							index = -1;
							return MoveNext();
						}

						current = _source[index];
						return true;
					case 2:
						if (++index >= _appendedCount)
						{
							current = default;
							return false;
						}

						current = _appended[index];
						return true;
					default:
						throw Error.InvalidState;
				}
			}

			public sealed override AppendPrependIterator<TSource> Append(TSource item)
			{
				return new AppendPrependNIterator<TSource>(this, item, true);
			}

			public sealed override AppendPrependIterator<TSource> Prepend(TSource item)
			{
				return new AppendPrependNIterator<TSource>(this, item, false);
			}
		}
	}
}