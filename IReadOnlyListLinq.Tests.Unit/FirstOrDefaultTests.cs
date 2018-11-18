using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace IReadOnlyListLinq.Tests.Unit
{
    public class FirstOrDefaultTests : IReadOnlyListLinqTests
    {
		[Fact]
		public void SameResultsRepeatCallsIntQuery()
		{
			var ieInt = Enumerable.Range(0, 0).ToList();
			var q = from x in ieInt
					select x;

			Assert.Equal(q.FirstOrDefault(), q.FirstOrDefault());
		}

		[Fact]
		public void SameResultsRepeatCallsStringQuery()
		{
			var q = (from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", string.Empty }
					where !string.IsNullOrEmpty(x)
					select x).ToList();

			Assert.Equal(q.FirstOrDefault(), q.FirstOrDefault());
		}

		private static void TestEmpty<T>()
		{
			T[] source = { };
			T expected = default;

			Assert.Equal(expected, source.RunOnce().FirstOrDefault());
		}

		[Fact]
		public void EmptyIListT()
		{
			TestEmpty<int>();
			TestEmpty<string>();
			TestEmpty<DateTime>();
			TestEmpty<FirstOrDefaultTests>();
		}

		[Fact]
		public void OneElement()
		{
			int[] source = { 5 };
			int expected = 5;

			Assert.Equal(expected, source.FirstOrDefault());
		}

		[Fact]
		public void ManyElementsFirstIsDefault()
		{
			int?[] source = { null, -10, 2, 4, 3, 0, 2 };
			int? expected = null;

			Assert.Equal(expected, source.FirstOrDefault());
		}

		[Fact]
		public void ManyElementsFirstIsNotDefault()
		{
			int?[] source = { 19, null, -10, 2, 4, 3, 0, 2 };
			int? expected = 19;

			Assert.Equal(expected, source.FirstOrDefault());
		}

		[Fact]
		public void EmptySource()
		{
			int?[] source = { };

			Assert.Null(source.FirstOrDefault(x => true));
			Assert.Null(source.FirstOrDefault(x => false));
		}

		[Fact]
		public void OneElementTruePredicate()
		{
			int[] source = { 4 };
			Func<int, bool> predicate = IsEven;
			int expected = 4;

			Assert.Equal(expected, source.FirstOrDefault(predicate));
		}

		[Fact]
		public void ManyElementsPredicateFalseForAll()
		{
			int[] source = { 9, 5, 1, 3, 17, 21 };
			Func<int, bool> predicate = IsEven;
			int expected = default(int);

			Assert.Equal(expected, source.FirstOrDefault(predicate));
		}

		[Fact]
		public void PredicateTrueOnlyForLast()
		{
			int[] source = { 9, 5, 1, 3, 17, 21, 50 };
			Func<int, bool> predicate = IsEven;
			int expected = 50;

			Assert.Equal(expected, source.FirstOrDefault(predicate));
		}

		[Fact]
		public void PredicateTrueForSome()
		{
			int[] source = { 3, 7, 10, 7, 9, 2, 11, 17, 13, 8 };
			Func<int, bool> predicate = IsEven;
			int expected = 10;

			Assert.Equal(expected, source.FirstOrDefault(predicate));
		}

		[Fact]
		public void PredicateTrueForSomeRunOnce()
		{
			int[] source = { 3, 7, 10, 7, 9, 2, 11, 17, 13, 8 };
			Func<int, bool> predicate = IsEven;
			int expected = 10;

			Assert.Equal(expected, source.RunOnce().FirstOrDefault(predicate));
		}

		[Fact]
		public void NullSource()
		{
			AssertExtensions.Throws<ArgumentNullException>("source", () => ((IReadOnlyList<int>)null).FirstOrDefault());
		}

		[Fact]
		public void NullSourcePredicateUsed()
		{
			AssertExtensions.Throws<ArgumentNullException>("source", () => ((IReadOnlyList<int>)null).FirstOrDefault(i => i != 2));
		}

		[Fact]
		public void NullPredicate()
		{
			Func<int, bool> predicate = null;
			AssertExtensions.Throws<ArgumentNullException>("predicate", () => Enumerable.Range(0, 3).ToList().FirstOrDefault(predicate));
		}
	}
}