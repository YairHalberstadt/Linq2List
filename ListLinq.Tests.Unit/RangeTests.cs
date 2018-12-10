using System;
using System.Linq;
using Xunit;

namespace Linq2List.Tests.Unit
{
	public class RangeTests : ListLinqTests
	{
		[Fact]
		public void Range_ProduceCorrectSequence()
		{
			var rangeSequence = ReadOnlyList.Range(1, 100);
			int expected = 0;
			foreach (var val in rangeSequence)
			{
				expected++;
				Assert.Equal(expected, val);
			}

			Assert.Equal(100, expected);
		}

		[Fact]
		public void Range_ToArray_ProduceCorrectResult()
		{
			var array = ReadOnlyList.Range(1, 100).ToArray();
			Assert.Equal(array.Length, 100);
			for (var i = 0; i < array.Length; i++)
				Assert.Equal(i + 1, array[i]);
		}

		[Fact]
		public void Range_ToList_ProduceCorrectResult()
		{
			var list = ReadOnlyList.Range(1, 100).ToList();
			Assert.Equal(list.Count, 100);
			for (var i = 0; i < list.Count; i++)
				Assert.Equal(i + 1, list[i]);
		}

		[Fact]
		public void Range_ZeroCountLeadToEmptySequence()
		{
			var array = ReadOnlyList.Range(1, 0).ToArray();
			var array2 = ReadOnlyList.Range(int.MinValue, 0).ToArray();
			var array3 = ReadOnlyList.Range(int.MaxValue, 0).ToArray();
			Assert.Equal(array.Length, 0);
			Assert.Equal(array2.Length, 0);
			Assert.Equal(array3.Length, 0);
		}

		[Fact]
		public void Range_ThrowExceptionOnNegativeCount()
		{
			AssertExtensions.Throws<ArgumentOutOfRangeException>("count", () => ReadOnlyList.Range(1, -1));
			AssertExtensions.Throws<ArgumentOutOfRangeException>("count", () => ReadOnlyList.Range(1, int.MinValue));
		}

		[Fact]
		public void Range_ThrowExceptionOnOverflow()
		{
			AssertExtensions.Throws<ArgumentOutOfRangeException>("count", () => ReadOnlyList.Range(1000, int.MaxValue));
			AssertExtensions.Throws<ArgumentOutOfRangeException>("count", () => ReadOnlyList.Range(int.MaxValue, 1000));
			AssertExtensions.Throws<ArgumentOutOfRangeException>("count", () => ReadOnlyList.Range(int.MaxValue - 10, 20));
		}

		[Fact]
		public void Range_NotEnumerateAfterEnd()
		{
			using (var rangeEnum = ReadOnlyList.Range(1, 1).GetEnumerator())
			{
				Assert.True(rangeEnum.MoveNext());
				Assert.False(rangeEnum.MoveNext());
				Assert.False(rangeEnum.MoveNext());
			}
		}

		[Fact]
		public void Range_ReadOnlyListAndEnumeratorAreSame()
		{
			var rangeReadOnlyList = ReadOnlyList.Range(1, 1);
			using (var rangeEnumerator = rangeReadOnlyList.GetEnumerator())
			{
				Assert.Same(rangeReadOnlyList, rangeEnumerator);
			}
		}

		[Fact]
		public void Range_GetEnumeratorReturnUniqueInstances()
		{
			var rangeReadOnlyList = ReadOnlyList.Range(1, 1);
			using (var enum1 = rangeReadOnlyList.GetEnumerator())
			using (var enum2 = rangeReadOnlyList.GetEnumerator())
			{
				Assert.NotSame(enum1, enum2);
			}
		}

		[Fact]
		public void Range_ToInt32MaxValue()
		{
			int from = int.MaxValue - 3;
			int count = 4;
			var rangeReadOnlyList = ReadOnlyList.Range(from, count);

			Assert.Equal(count, rangeReadOnlyList.Count());

			int[] expected = { int.MaxValue - 3, int.MaxValue - 2, int.MaxValue - 1, int.MaxValue };
			Assert.Equal(expected, rangeReadOnlyList);
		}

		[Fact]
		public void RepeatedCallsSameResults()
		{
			Assert.Equal(ReadOnlyList.Range(-1, 2), ReadOnlyList.Range(-1, 2));
			Assert.Equal(ReadOnlyList.Range(0, 0), ReadOnlyList.Range(0, 0));
		}

		[Fact]
		public void NegativeStart()
		{
			int start = -5;
			int count = 1;
			int[] expected = { -5 };

			Assert.Equal(expected, ReadOnlyList.Range(start, count));
		}

		[Fact]
		public void ArbitraryStart()
		{
			int start = 12;
			int count = 6;
			int[] expected = { 12, 13, 14, 15, 16, 17 };

			Assert.Equal(expected, ReadOnlyList.Range(start, count));
		}

		[Fact]
		public void Take()
		{
			Assert.Equal(ReadOnlyList.Range(0, 10), ReadOnlyList.Range(0, 20).Take(10));
		}

		[Fact]
		public void TakeExcessive()
		{
			Assert.Equal(ReadOnlyList.Range(0, 10), ReadOnlyList.Range(0, 10).Take(int.MaxValue));
		}

		[Fact]
		public void Skip()
		{
			Assert.Equal(ReadOnlyList.Range(10, 10), ReadOnlyList.Range(0, 20).Skip(10));
		}

		[Fact]
		public void SkipExcessive()
		{
			Assert.Empty(ReadOnlyList.Range(10, 10).Skip(20));
		}

		[Fact]
		public void SkipTakeCanOnlyBeOne()
		{
			Assert.Equal(new[] { 1 }, ReadOnlyList.Range(1, 10).Take(1));
			Assert.Equal(new[] { 2 }, ReadOnlyList.Range(1, 10).Skip(1).Take(1));
			Assert.Equal(new[] { 3 }, ReadOnlyList.Range(1, 10).Take(3).Skip(2));
			Assert.Equal(new[] { 1 }, ReadOnlyList.Range(1, 10).Take(3).Take(1));
		}

		[Fact]
		public void ElementAt()
		{
			Assert.Equal(4, ReadOnlyList.Range(0, 10).ElementAt(4));
		}

		[Fact]
		public void ElementAtExcessiveThrows()
		{
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => ReadOnlyList.Range(0, 10).ElementAt(100));
		}

		[Fact]
		public void ElementAtOrDefault()
		{
			Assert.Equal(4, ReadOnlyList.Range(0, 10).ElementAtOrDefault(4));
		}

		[Fact]
		public void ElementAtOrDefaultExcessiveIsDefault()
		{
			Assert.Equal(0, ReadOnlyList.Range(52, 10).ElementAtOrDefault(100));
		}

		[Fact]
		public void First()
		{
			Assert.Equal(57, ReadOnlyList.Range(57, 1000000000).First());
		}

		[Fact]
		public void FirstOrDefault()
		{
			Assert.Equal(-100, ReadOnlyList.Range(-100, int.MaxValue).FirstOrDefault());
		}

		[Fact]
		public void Last()
		{
			Assert.Equal(1000000056, ReadOnlyList.Range(57, 1000000000).Last());
		}

		[Fact]
		public void LastOrDefault()
		{
			Assert.Equal(int.MaxValue - 101, ReadOnlyList.Range(-100, int.MaxValue).LastOrDefault());
		}

		[Fact]
		public void RunIteratorTests()
		{
			var runner = new IteratorTests();
			runner.RunTests(ReadOnlyList.Range(0, 0));
			runner.RunTests(ReadOnlyList.Range(1, 0));
			runner.RunTests(ReadOnlyList.Range(-1, 0));
			runner.RunTests(ReadOnlyList.Range(0, 30));
			runner.RunTests(ReadOnlyList.Range(1, 30));
			runner.RunTests(ReadOnlyList.Range(-1, 30));
		}
	}
}