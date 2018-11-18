using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace IReadOnlyListLinq.Tests.Unit
{
    public class LastTests : IReadOnlyListLinqTests
    {
		[Fact]
		public void SameResultsRepeatCallsIntQuery()
		{
			var q = from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
					select x;

			Assert.Equal(q.Last(), q.Last());
		}

		[Fact]
		public void SameResultsRepeatCallsStringQuery()
		{
			var q = from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", string.Empty }
					select x;

			Assert.Equal(q.Last(), q.Last());
		}

		public void TestEmpty<T>()
		{
			T[] source = { };

			Assert.Throws<InvalidOperationException>(() => source.RunOnce().Last());
		}

		[Fact]
		public void EmptyIListT()
		{
			TestEmpty<int>();
			TestEmpty<string>();
			TestEmpty<DateTime>();
			TestEmpty<LastTests>();
		}

		[Fact]
		public void OneElement()
		{
			int[] source = { 5 };
			int expected = 5;

			Assert.Equal(expected, source.Last());
		}

		[Fact]
		public void ManyElementsLastIsDefault()
		{
			int?[] source = { -10, 2, 4, 3, 0, 2, null };
			int? expected = null;

			Assert.Equal(expected, source.Last());
		}

		[Fact]
		public void ManyElementsLastIsNotDefault()
		{
			int?[] source = { -10, 2, 4, 3, 0, 2, null, 19 };
			int? expected = 19;

			Assert.Equal(expected, source.Last());
		}

		[Fact]
		public void EmptySourcePredicate()
		{
			int[] source = { };

			Assert.Throws<InvalidOperationException>(() => source.Last(x => true));
			Assert.Throws<InvalidOperationException>(() => source.Last(x => false));
		}

		[Fact]
		public void OneElementTruePredicate()
		{
			int[] source = { 4 };
			Func<int, bool> predicate = IsEven;
			int expected = 4;

			Assert.Equal(expected, source.Last(predicate));
		}

		[Fact]
		public void ManyElementsPredicateFalseForAll()
		{
			int[] source = { 9, 5, 1, 3, 17, 21 };
			Func<int, bool> predicate = IsEven;

			Assert.Throws<InvalidOperationException>(() => source.Last(predicate));
		}

		[Fact]
		public void PredicateTrueOnlyForLast()
		{
			int[] source = { 9, 5, 1, 3, 17, 21, 50 };
			Func<int, bool> predicate = IsEven;
			int expected = 50;

			Assert.Equal(expected, source.Last(predicate));
		}

		[Fact]
		public void PredicateTrueForSome()
		{
			int[] source = { 3, 7, 10, 7, 9, 2, 11, 18, 13, 9 };
			Func<int, bool> predicate = IsEven;
			int expected = 18;

			Assert.Equal(expected, source.Last(predicate));
		}

		[Fact]
		public void PredicateTrueForSomeRunOnce()
		{
			int[] source = { 3, 7, 10, 7, 9, 2, 11, 18, 13, 9 };
			Func<int, bool> predicate = IsEven;
			int expected = 18;

			Assert.Equal(expected, source.RunOnce().Last(predicate));
		}

		[Fact]
		public void NullSource()
		{
			AssertExtensions.Throws<ArgumentNullException>("source", () => ((IReadOnlyList<int>)null).Last());
		}

		[Fact]
		public void NullSourcePredicateUsed()
		{
			AssertExtensions.Throws<ArgumentNullException>("source", () => ((IReadOnlyList<int>)null).Last(i => i != 2));
		}

		[Fact]
		public void NullPredicate()
		{
			Func<int, bool> predicate = null;
			AssertExtensions.Throws<ArgumentNullException>("predicate", () => Enumerable.Range(0, 3).ToList().Last(predicate));
		}
	}
}