using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Linq2List.Tests.Unit
{
    public class FirstTests : ListLinqTests
    {
		[Fact]
		public void SameResultsRepeatCallsIntQuery()
		{
			var q = (from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
					where x > int.MinValue
					select x).ToList();

			Assert.Equal(q.First(), q.First());
		}

		[Fact]
		public void SameResultsRepeatCallsStringQuery()
		{
			var q = (from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", string.Empty }
					where !string.IsNullOrEmpty(x)
					select x).ToList();

			Assert.Equal(q.First(), q.First());
		}

#pragma warning disable xUnit1013 // Public method should be marked as test
		public void TestEmptyIList<T>()
#pragma warning restore xUnit1013 // Public method should be marked as test
		{
			T[] source = { };

			Assert.Throws<InvalidOperationException>(() => source.RunOnce().First());
		}

		[Fact]
		public void EmptyIListT()
		{
			TestEmptyIList<int>();
			TestEmptyIList<string>();
			TestEmptyIList<DateTime>();
			TestEmptyIList<FirstTests>();
		}

		[Fact]
		public void OneElement()
		{
			int[] source = { 5 };
			int expected = 5;

			Assert.Equal(expected, source.First());
		}

		[Fact]
		public void ManyElementsFirstIsDefault()
		{
			int?[] source = { null, -10, 2, 4, 3, 0, 2 };
			int? expected = null;

			Assert.Equal(expected, source.First());
		}

		[Fact]
		public void ManyElementsFirstIsNotDefault()
		{
			int?[] source = { 19, null, -10, 2, 4, 3, 0, 2 };
			int? expected = 19;

			Assert.Equal(expected, source.First());
		}

		[Fact]
		public void EmptySource()
		{
			int[] source = { };

			Assert.Throws<InvalidOperationException>(() => source.First(x => true));
			Assert.Throws<InvalidOperationException>(() => source.First(x => false));
		}

		[Fact]
		public void OneElementTruePredicate()
		{
			int[] source = { 4 };
			Func<int, bool> predicate = IsEven;
			int expected = 4;

			Assert.Equal(expected, source.First(predicate));
		}

		[Fact]
		public void ManyElementsPredicateFalseForAll()
		{
			int[] source = { 9, 5, 1, 3, 17, 21 };
			Func<int, bool> predicate = IsEven;

			Assert.Throws<InvalidOperationException>(() => source.First(predicate));
		}

		[Fact]
		public void PredicateTrueOnlyForLast()
		{
			int[] source = { 9, 5, 1, 3, 17, 21, 50 };
			Func<int, bool> predicate = IsEven;
			int expected = 50;

			Assert.Equal(expected, source.First(predicate));
		}

		[Fact]
		public void PredicateTrueForSome()
		{
			int[] source = { 3, 7, 10, 7, 9, 2, 11, 17, 13, 8 };
			Func<int, bool> predicate = IsEven;
			int expected = 10;

			Assert.Equal(expected, source.First(predicate));
		}

		[Fact]
		public void PredicateTrueForSomeRunOnce()
		{
			int[] source = { 3, 7, 10, 7, 9, 2, 11, 17, 13, 8 };
			Func<int, bool> predicate = IsEven;
			int expected = 10;

			Assert.Equal(expected, source.RunOnce().First(predicate));
		}

		[Fact]
		public void NullSource()
		{
			AssertExtensions.Throws<ArgumentNullException>("source", () => ((IReadOnlyList<int>)null).First());
		}

		[Fact]
		public void NullSourcePredicateUsed()
		{
			AssertExtensions.Throws<ArgumentNullException>("source", () => ((IReadOnlyList<int>)null).First(i => i != 2));
		}

		[Fact]
		public void NullPredicate()
		{
			Func<int, bool> predicate = null;
			AssertExtensions.Throws<ArgumentNullException>("predicate", () => ReadOnlyList.Range(0, 3).First(predicate));
		}
	}
}