using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ListLinq.Tests.Unit
{
	public class RepeatTests : ListLinqTests
	{
		[Fact]
		public void Repeat_ProduceCorrectSequence()
		{
			var repeatSequence = ReadOnlyList.Repeat(1, 100);
			int count = 0;
			foreach (var val in repeatSequence)
			{
				count++;
				Assert.Equal(1, val);
			}

			Assert.Equal(100, count);
		}

		[Fact]
		public void Repeat_ToArray_ProduceCorrectResult()
		{
			var array = ReadOnlyList.Repeat(1, 100).ToArray();
			Assert.Equal(array.Length, 100);
			for (var i = 0; i < array.Length; i++)
				Assert.Equal(1, array[i]);
		}

		[Fact]
		public void Repeat_ToList_ProduceCorrectResult()
		{
			var list = ReadOnlyList.Repeat(1, 100).ToList();
			Assert.Equal(list.Count, 100);
			for (var i = 0; i < list.Count; i++)
				Assert.Equal(1, list[i]);
		}

		[Fact]
		public void Repeat_ProduceSameObject()
		{
			object objectInstance = new object();
			var array = ReadOnlyList.Repeat(objectInstance, 100).ToArray();
			Assert.Equal(array.Length, 100);
			for (var i = 0; i < array.Length; i++)
				Assert.Same(objectInstance, array[i]);
		}

		[Fact]
		public void Repeat_WorkWithNullElement()
		{
			object objectInstance = null;
			var array = ReadOnlyList.Repeat(objectInstance, 100).ToArray();
			Assert.Equal(array.Length, 100);
			for (var i = 0; i < array.Length; i++)
				Assert.Null(array[i]);
		}


		[Fact]
		public void Repeat_ZeroCountLeadToEmptySequence()
		{
			var array = ReadOnlyList.Repeat(1, 0).ToArray();
			Assert.Equal(array.Length, 0);
		}

		[Fact]
		public void Repeat_ThrowExceptionOnNegativeCount()
		{
			AssertExtensions.Throws<ArgumentOutOfRangeException>("count", () => ReadOnlyList.Repeat(1, -1));
		}


		[Fact]
		public void Repeat_NotEnumerateAfterEnd()
		{
			using (var repeatEnum = ReadOnlyList.Repeat(1, 1).GetEnumerator())
			{
				Assert.True(repeatEnum.MoveNext());
				Assert.False(repeatEnum.MoveNext());
				Assert.False(repeatEnum.MoveNext());
			}
		}

		[Fact]
		public void Repeat_ReadOnlyListAndEnumeratorAreSame()
		{
			var repeatReadOnlyList = ReadOnlyList.Repeat(1, 1);
			using (var repeatEnumerator = repeatReadOnlyList.GetEnumerator())
			{
				Assert.Same(repeatReadOnlyList, repeatEnumerator);
			}
		}

		[Fact]
		public void Repeat_GetEnumeratorReturnUniqueInstances()
		{
			var repeatReadOnlyList = ReadOnlyList.Repeat(1, 1);
			using (var enum1 = repeatReadOnlyList.GetEnumerator())
			using (var enum2 = repeatReadOnlyList.GetEnumerator())
			{
				Assert.NotSame(enum1, enum2);
			}
		}

		[Fact]
		public void SameResultsRepeatCallsIntQuery()
		{
			Assert.Equal(ReadOnlyList.Repeat(-3, 0), ReadOnlyList.Repeat(-3, 0));
		}

		[Fact]
		public void SameResultsRepeatCallsStringQuery()
		{
			Assert.Equal(ReadOnlyList.Repeat("SSS", 99), ReadOnlyList.Repeat("SSS", 99));
		}

		[Fact]
		public void CountOneSingleResult()
		{
			int[] expected = { -15 };

			Assert.Equal(expected, ReadOnlyList.Repeat(-15, 1));
		}

		[Fact]
		public void RepeatArbitraryCorrectResults()
		{
			int[] expected = { 12, 12, 12, 12, 12, 12, 12, 12 };

			Assert.Equal(expected, ReadOnlyList.Repeat(12, 8));
		}

		[Fact]
		public void RepeatNull()
		{
			int?[] expected = { null, null, null, null };

			Assert.Equal(expected, ReadOnlyList.Repeat((int?)null, 4));
		}

		[Fact]
		public void Take()
		{
			Assert.Equal(ReadOnlyList.Repeat(12, 8), ReadOnlyList.Repeat(12, 12).Take(8));
		}

		[Fact]
		public void TakeExcessive()
		{
			Assert.Equal(ReadOnlyList.Repeat("", 4), ReadOnlyList.Repeat("", 4).Take(22));
		}

		[Fact]
		public void Skip()
		{
			Assert.Equal(ReadOnlyList.Repeat(12, 8), ReadOnlyList.Repeat(12, 12).Skip(4));
		}

		[Fact]
		public void SkipExcessive()
		{
			Assert.Empty(ReadOnlyList.Repeat(12, 8).Skip(22));
		}

		[Fact]
		public void TakeCanOnlyBeOne()
		{
			Assert.Equal(new[] { 1 }, ReadOnlyList.Repeat(1, 10).Take(1));
			Assert.Equal(new[] { 1 }, ReadOnlyList.Repeat(1, 10).Skip(1).Take(1));
			Assert.Equal(new[] { 1 }, ReadOnlyList.Repeat(1, 10).Take(3).Skip(2));
			Assert.Equal(new[] { 1 }, ReadOnlyList.Repeat(1, 10).Take(3).Take(1));
		}

		[Fact]
		public void SkipNone()
		{
			Assert.Equal(ReadOnlyList.Repeat(12, 8), ReadOnlyList.Repeat(12, 8).Skip(0));
		}

		[Fact]
		public void First()
		{
			Assert.Equal("Test", ReadOnlyList.Repeat("Test", 42).First());
		}

		[Fact]
		public void FirstOrDefault()
		{
			Assert.Equal("Test", ReadOnlyList.Repeat("Test", 42).FirstOrDefault());
		}

		[Fact]
		public void Last()
		{
			Assert.Equal("Test", ReadOnlyList.Repeat("Test", 42).Last());
		}

		[Fact]
		public void LastOrDefault()
		{
			Assert.Equal("Test", ReadOnlyList.Repeat("Test", 42).LastOrDefault());
		}

		[Fact]
		public void ElementAt()
		{
			Assert.Equal("Test", ReadOnlyList.Repeat("Test", 42).ElementAt(13));
		}

		[Fact]
		public void ElementAtOrDefault()
		{
			Assert.Equal("Test", ReadOnlyList.Repeat("Test", 42).ElementAtOrDefault(13));
		}

		[Fact]
		public void ElementAtExcessive()
		{
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => ReadOnlyList.Repeat(3, 3).ElementAt(100));
		}

		[Fact]
		public void ElementAtOrDefaultExcessive()
		{
			Assert.Equal(0, ReadOnlyList.Repeat(3, 3).ElementAtOrDefault(100));
		}

		[Fact]
		public void Count()
		{
			Assert.Equal(42, ReadOnlyList.Repeat("Test", 42).Count());
		}

		[Fact]
		public void RunIteratorTests()
		{
			var runner = new IteratorTests();
			runner.RunTests(ReadOnlyList.Repeat<string>(null, 0));
			runner.RunTests(ReadOnlyList.Repeat<string>(null, 30));
			runner.RunTests(ReadOnlyList.Repeat("Hi", 0));
			runner.RunTests(ReadOnlyList.Repeat("Hi", 30));
			runner.RunTests(ReadOnlyList.Repeat(42, 0));
			runner.RunTests(ReadOnlyList.Repeat(42, 30));
		}
	}
}