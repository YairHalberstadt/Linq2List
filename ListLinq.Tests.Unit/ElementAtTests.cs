using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ListLinq.Tests.Unit
{
    public class ElementAtTests : ListLinqTests
    {
		[Fact]
		public void SameResultsRepeatCallsIntQuery()
		{
			var q = (from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
					where x > int.MinValue
					select x).ToList();

			Assert.Equal(q.ElementAt(3), q.ElementAt(3));
		}

		[Fact]
		public void SameResultsRepeatCallsStringQuery()
		{
			var q = (from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", string.Empty }
					where !string.IsNullOrEmpty(x)
					select x).ToList();

			Assert.Equal(q.ElementAt(4), q.ElementAt(4));
		}

		public static IEnumerable<object[]> TestData()
		{
			yield return new object[] { ReadOnlyList.Range(9, 1), 0, 9 };
			yield return new object[] { ReadOnlyList.Range(9, 10), 9, 18 };
			yield return new object[] { ReadOnlyList.Range(-4, 10), 3, -1 };

			yield return new object[] { new int[] { -4 }, 0, -4 };
			yield return new object[] { new int[] { 9, 8, 0, -5, 10 }, 4, 10 };
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public void ElementAt(IReadOnlyList<int> source, int index, int expected)
		{
			Assert.Equal(expected, source.ElementAt(index));
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public void ElementAtRunOnce(IReadOnlyList<int> source, int index, int expected)
		{
			Assert.Equal(expected, source.RunOnce().ElementAt(index));
		}

		[Theory]
		[MemberData(nameof(TestData))]
		public void ElementAtEqualsIndexer(IReadOnlyList<int> source, int _, int __)
		{
			for (int i = 0; i < source.Count; i++)
			{
				Assert.Equal(source[i], source.ElementAt(i));
			}
		}

		[Fact]
		public void InvalidIndex_ThrowsArgumentOutOfRangeException()
		{
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => new int?[] { 9, 8 }.ElementAt(-1));
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => new int[] { 1, 2, 3, 4 }.ElementAt(4));
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => new int[0].ElementAt(0));

			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => ReadOnlyList.Range(-4, 5).ElementAt(-1));
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => ReadOnlyList.Range(5, 5).ElementAt(5));
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => ReadOnlyList.Range(0, 0).ElementAt(0));

		}

		[Fact]
		public void NullableArray_ValidIndex_ReturnsCorrectObject()
		{
			int?[] source = { 9, 8, null, -5, 10 };

			Assert.Null(source.ElementAt(2));
			Assert.Equal(-5, source.ElementAt(3));
		}

		[Fact]
		public void NullSource_ThrowsArgumentNullException()
		{
			AssertExtensions.Throws<ArgumentNullException>("source", () => ((IReadOnlyList<int>)null).ElementAt(2));
		}
	}
}