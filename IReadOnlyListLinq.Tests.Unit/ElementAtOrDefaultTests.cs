using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace IReadOnlyListLinq.Tests.Unit
{
    public class ElementAtOrDefaultTests : IReadOnlyListLinqTests
    {
		[Fact]
		public void SameResultsRepeatCallsIntQuery()
		{
			var q = (from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
					where x > int.MinValue
					select x).ToList();

			Assert.Equal(q.ElementAtOrDefault(3), q.ElementAtOrDefault(3));
		}

		[Fact]
		public void SameResultsRepeatCallsStringQuery()
		{
			var q = (from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", string.Empty }
					where !string.IsNullOrEmpty(x)
					select x).ToList();

			Assert.Equal(q.ElementAtOrDefault(4), q.ElementAtOrDefault(4));
		}

		public static IEnumerable<object[]> TestData()
		{
			yield return new object[] { Enumerable.Range(9, 1).ToList(), 0, 9 };
			yield return new object[] { Enumerable.Range(9, 10).ToList(), 9, 18 };
			yield return new object[] { Enumerable.Range(-4, 10).ToList(), 3, -1 };

			yield return new object[] { new int[] { 1, 2, 3, 4 }, 4, 0 };
			yield return new object[] { new int[0], 0, 0 };
			yield return new object[] { new int[] { -4 }, 0, -4 };
			yield return new object[] { new int[] { 9, 8, 0, -5, 10 }, 4, 10 };

			yield return new object[] { Enumerable.Range(-4, 5).ToList(), -1, 0 };
			yield return new object[] { Enumerable.Range(5, 5).ToList(), 5, 0 };
			yield return new object[] { Enumerable.Range(0, 0).ToList(), 0, 0 };
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public void ElementAtOrDefault(IReadOnlyList<int> source, int index, int expected)
		{
			Assert.Equal(expected, source.ElementAtOrDefault(index));
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public void ElementAtOrDefaultRunOnce(IReadOnlyList<int> source, int index, int expected)
		{
			Assert.Equal(expected, source.RunOnce().ElementAtOrDefault(index));
		}

		[Fact]
		public void NullableArray_NegativeIndex_ReturnsNull()
		{
			int?[] source = { 9, 8 };
			Assert.Null(source.ElementAtOrDefault(-1));
		}

		[Fact]
		public void NullableArray_ValidIndex_ReturnsCorrectObjecvt()
		{
			int?[] source = { 9, 8, null, -5, 10 };

			Assert.Null(source.ElementAtOrDefault(2));
			Assert.Equal(-5, source.ElementAtOrDefault(3));
		}

		[Fact]
		public void NullSource_ThrowsArgumentNullException()
		{
			AssertExtensions.Throws<ArgumentNullException>("source", () => ((IReadOnlyList<int>)null).ElementAtOrDefault(2));
		}
	}
}