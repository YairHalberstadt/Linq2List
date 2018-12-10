// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Linq2List.Tests.Unit
{
	public class SkipTests : ListLinqTests
	{
		[Fact]
		public void SkipSomeIList()
		{
			Assert.Equal(ReadOnlyList.Range(10, 10), ReadOnlyList.Range(0, 20).Skip(10));
		}

		[Fact]
		public void RunOnce()
		{
			Assert.Equal(ReadOnlyList.Range(10, 10), ReadOnlyList.Range(0, 20).RunOnce().Skip(10));
		}

		[Fact]
		public void SkipNoneIList()
		{
			Assert.Equal(ReadOnlyList.Range(0, 20), ReadOnlyList.Range(0, 20).Skip(0));
		}

		[Fact]
		public void SkipExcessiveIList()
		{
			Assert.Equal(ReadOnlyList.Empty<int>(), ReadOnlyList.Range(0, 20).Skip(42));
		}

		[Fact]
		public void SkipAllExactlyIList()
		{
			Assert.False(ReadOnlyList.Range(0, 20).Skip(20).Any());
		}

		[Fact]
		public void SkipThrowsOnNull()
		{
			AssertExtensions.Throws<ArgumentNullException>("source", () => ((IReadOnlyList<DateTime>)null).Skip(3));
		}

		[Fact]
		public void SkipOnEmptyIList()
		{
			Assert.Equal(ReadOnlyList.Empty<int>(), Array.Empty<int>().Skip(0));
			Assert.Equal(ReadOnlyList.Empty<string>(), Array.Empty<string>().Skip(-1));
			Assert.Equal(ReadOnlyList.Empty<double>(), Array.Empty<double>().Skip(1));
		}

		[Fact]
		public void SkipNegativeIList()
		{
			Assert.Equal(ReadOnlyList.Range(0, 20), ReadOnlyList.Range(0, 20).Skip(-42));
		}

		[Fact]
		public void SameResultsRepeatCallsIntQueryIList()
		{
			var q = (from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
					 where x > Int32.MinValue
					 select x).ToList();

			Assert.Equal(q.Skip(0), q.Skip(0));
		}

		[Fact]
		public void SameResultsRepeatCallsStringQueryIList()
		{
			var q = (from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", String.Empty }
					 where !String.IsNullOrEmpty(x)
					 select x).ToList();

			Assert.Equal(q.Skip(0), q.Skip(0));
		}

		[Fact]
		public void SkipOne()
		{
			int?[] source = { 3, 100, 4, null, 10 };
			int?[] expected = { 100, 4, null, 10 };

			Assert.Equal(expected, source.Skip(1));
		}

		[Fact]
		public void SkipAllButOne()
		{
			int?[] source = { 3, 100, null, 4, 10 };
			int?[] expected = { 10 };

			Assert.Equal(expected, source.Skip(source.Length - 1));
		}

		[Fact]
		public void SkipOneMoreThanAll()
		{
			int[] source = { 3, 100, 4, 10 };
			Assert.Empty(source.Skip(source.Length + 1));
		}

		[Fact]
		public void Count()
		{
			Assert.Equal(2, new[] { 1, 2, 3 }.Skip(1).Count);
		}

		[Fact]
		public void FollowWithTake()
		{
			var source = new[] { 5, 6, 7, 8 };
			var expected = new[] { 6, 7 };
			Assert.Equal(expected, source.Skip(1).Take(2));
		}

		[Fact]
		public void FollowWithTakeThenMassiveTake()
		{
			var source = new[] { 5, 6, 7, 8 };
			var expected = new[] { 7 };
			Assert.Equal(expected, source.Skip(2).Take(1).Take(int.MaxValue));
		}

		[Fact]
		public void FollowWithSkip()
		{
			var source = new[] { 1, 2, 3, 4, 5, 6 };
			var expected = new[] { 4, 5, 6 };
			Assert.Equal(expected, source.Skip(1).Skip(2).Skip(-4));
		}

		[Fact]
		public void ElementAt()
		{
			var source = new[] { 1, 2, 3, 4, 5, 6 };
			var remaining = source.Skip(2);
			Assert.Equal(3, remaining.ElementAt(0));
			Assert.Equal(3, remaining[0]);
			Assert.Equal(4, remaining.ElementAt(1));
			Assert.Equal(4, remaining[1]);
			Assert.Equal(6, remaining.ElementAt(3));
			Assert.Equal(6, remaining[3]);
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => remaining.ElementAt(-1));
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => remaining[-1]);
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => remaining.ElementAt(4));
			AssertExtensions.Throws<ArgumentOutOfRangeException>("index", () => remaining[4]);
		}

		[Fact]
		public void ElementAtOrDefault()
		{
			var source = new[] { 1, 2, 3, 4, 5, 6 };
			var remaining = source.Skip(2);
			Assert.Equal(3, remaining.ElementAtOrDefault(0));
			Assert.Equal(4, remaining.ElementAtOrDefault(1));
			Assert.Equal(6, remaining.ElementAtOrDefault(3));
			Assert.Equal(0, remaining.ElementAtOrDefault(-1));
			Assert.Equal(0, remaining.ElementAtOrDefault(4));
		}

		[Fact]
		public void First()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			Assert.Equal(1, source.Skip(0).First());
			Assert.Equal(3, source.Skip(2).First());
			Assert.Equal(5, source.Skip(4).First());
			Assert.Throws<InvalidOperationException>(() => source.Skip(5).First());
		}

		[Fact]
		public void FirstOrDefault()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			Assert.Equal(1, source.Skip(0).FirstOrDefault());
			Assert.Equal(3, source.Skip(2).FirstOrDefault());
			Assert.Equal(5, source.Skip(4).FirstOrDefault());
			Assert.Equal(0, source.Skip(5).FirstOrDefault());
		}

		[Fact]
		public void Last()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			Assert.Equal(5, source.Skip(0).Last());
			Assert.Equal(5, source.Skip(1).Last());
			Assert.Equal(5, source.Skip(4).Last());
			Assert.Throws<InvalidOperationException>(() => source.Skip(5).Last());
		}

		[Fact]
		public void LastOrDefault()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			Assert.Equal(5, source.Skip(0).LastOrDefault());
			Assert.Equal(5, source.Skip(1).LastOrDefault());
			Assert.Equal(5, source.Skip(4).LastOrDefault());
			Assert.Equal(0, source.Skip(5).LastOrDefault());
		}

		[Fact]
		public void ToArray()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			Assert.Equal(new[] { 1, 2, 3, 4, 5 }, source.Skip(0).ToArray());
			Assert.Equal(new[] { 2, 3, 4, 5 }, source.Skip(1).ToArray());
			Assert.Equal(5, source.Skip(4).ToArray().Single());
			Assert.Empty(source.Skip(5).ToArray());
			Assert.Empty(source.Skip(40).ToArray());
		}

		[Fact]
		public void ToList()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			Assert.Equal(new[] { 1, 2, 3, 4, 5 }, source.Skip(0).ToList());
			Assert.Equal(new[] { 2, 3, 4, 5 }, source.Skip(1).ToList());
			Assert.Equal(5, source.Skip(4).ToList().Single());
			Assert.Empty(source.Skip(5).ToList());
			Assert.Empty(source.Skip(40).ToList());
		}

		[Fact]
		public void RepeatEnumerating()
		{
			var source = new[] { 1, 2, 3, 4, 5 };
			var remaining = source.Skip(1);
			Assert.Equal(remaining, remaining);
		}

		[Fact]
		public void LazySkipMoreThan32Bits()
		{
			var range = ReadOnlyList.Range(1, 100);
			var skipped = range.Skip(50).Skip(int.MaxValue); // Could cause an integer overflow.
			Assert.Empty(skipped);
			Assert.Equal(0, skipped.Count);
			Assert.Empty(skipped.ToArray());
			Assert.Empty(skipped.ToList());
		}

		[Theory]
		[MemberData(nameof(IteratorTestsData))]
		public void RunIteratorTests(IReadOnlyList<int> source)
		{
			var skipCounts = new[] { int.MinValue, -1, 0, 1, int.MaxValue };
			foreach (var count in skipCounts)
			{
				var iterator = source.Skip(count);
				new IteratorTests().RunTests(iterator);
			}
		}

		public static IEnumerable<object[]> IteratorTestsData()
		{
			yield return new object[] { Array.Empty<int>() };
			yield return new object[] { new int[1] };
			yield return new object[] { ReadOnlyList.Range(1, 30)};
		}
	}
}