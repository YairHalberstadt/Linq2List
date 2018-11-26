using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ListLinq.Tests.Unit
{
	public class ConcatTests : ListLinqTests
	{
		[Theory]
		[InlineData(new int[] {2, 3, 2, 4, 5}, new int[] {1, 9, 4})]
		public void SameResultsWithQueryAndRepeatCallsInt(IReadOnlyList<int> first, IReadOnlyList<int> second)
		{
			// workaround: xUnit type inference doesn't work if the input type is not T (like IEnumerable<T>)
			SameResultsWithQueryAndRepeatCallsWorker(first, second);
		}

		[Theory]
		[InlineData(new[] {"AAA", "", "q", "C", "#", "!@#$%^", "0987654321", "Calling Twice"},
			new[] {"!@#$%^", "C", "AAA", "", "Calling Twice", "SoS"})]
		public void SameResultsWithQueryAndRepeatCallsString(IReadOnlyList<string> first, IReadOnlyList<string> second)
		{
			// workaround: xUnit type inference doesn't work if the input type is not T (like IEnumerable<T>)
			SameResultsWithQueryAndRepeatCallsWorker(first, second);
		}

		private static void SameResultsWithQueryAndRepeatCallsWorker<T>(IReadOnlyList<T> first, IReadOnlyList<T> second)
		{
			first = from item in first select item;
			second = from item in second select item;

			VerifyEqualsWorker(first.Concat(second), first.Concat(second));
			VerifyEqualsWorker(second.Concat(first), second.Concat(first));
		}

		[Theory]
		[InlineData(new int[] { }, new int[] { }, new int[] { })] // Both inputs are empty
		[InlineData(new int[] { }, new int[] {2, 6, 4, 6, 2}, new int[] {2, 6, 4, 6, 2})] // One is empty
		[InlineData(new int[] {2, 3, 5, 9}, new int[] {8, 10}, new int[] {2, 3, 5, 9, 8, 10})]
		// Neither side is empty
		public void PossiblyEmptyInputs(IReadOnlyList<int> first, IReadOnlyList<int> second,
			IReadOnlyList<int> expected)
		{
			VerifyEqualsWorker(expected, first.Concat(second));
			VerifyEqualsWorker(expected.Skip(first.Count()).Concat(expected.Take(first.Count())),
				second.Concat(first)); // Swap the inputs around
		}

		[Fact]
		public void FirstNull()
		{
			AssertExtensions.Throws<ArgumentNullException>("first",
				() => ((IReadOnlyList<int>) null).Concat(Array.Empty<int>()));
			AssertExtensions.Throws<ArgumentNullException>("first",
				() => ((IReadOnlyList<int>) null).Concat(null)); // If both inputs are null, throw for "first" first
		}

		[Fact]
		public void SecondNull()
		{
			AssertExtensions.Throws<ArgumentNullException>("second", () => Enumerable.Range(0, 0).Concat(null));
		}

		[Theory]
		[MemberData(nameof(ArraySourcesData))]
		[MemberData(nameof(SelectArraySourcesData))]
		[MemberData(nameof(ListSourcesData))]
		[MemberData(nameof(ConcatOfConcatsData))]
		[MemberData(nameof(ConcatWithSelfData))]
		[MemberData(nameof(ChainedCollectionConcatData))]
		[MemberData(nameof(AppendedPrependedConcatAlternationsData))]
		public void VerifyEquals(IReadOnlyList<int> expected, IReadOnlyList<int> actual)
		{
			// workaround: xUnit type inference doesn't work if the input type is not T (like IEnumerable<T>)
			VerifyEqualsWorker(expected, actual);
		}

		private static void VerifyEqualsWorker<T>(IReadOnlyList<T> expected, IReadOnlyList<T> actual)
		{
			// Returns a list of functions that, when applied to enumerable, should return
			// another one that has equivalent contents.
			var identityTransforms = IdentityTransforms<T>();

			// We run the transforms N^2 times, by testing all transforms
			// of expected against all transforms of actual.
			foreach (var outTransform in identityTransforms)
			{
				foreach (var inTransform in identityTransforms)
				{
					Assert.Equal(outTransform(expected), inTransform(actual));
				}
			}
		}

		public static IEnumerable<object[]> ArraySourcesData() => GenerateSourcesData(outerTransform: e => e.ToArray());

		public static IEnumerable<object[]> SelectArraySourcesData() =>
			GenerateSourcesData(outerTransform: e => e.Select(i => i).ToArray());

		public static IEnumerable<object[]> ListSourcesData() => GenerateSourcesData(outerTransform: e => e.ToList());

		public static IEnumerable<object[]> ConcatOfConcatsData()
		{
			yield return new object[]
			{
				Enumerable.Range(0, 20).ToList(),
				Enumerable.Range(0, 4).ToList()
					.Concat(
						Enumerable.Range(4, 6).ToList())
					.Concat(
						Enumerable.Range(10, 3).ToList()
							.Concat(Enumerable.Range(13, 7).ToList()))
			};
		}

		public static IEnumerable<object[]> ConcatWithSelfData()
		{
			IReadOnlyList<int> source = Enumerable.Repeat(1, 4).ToList().Concat(Enumerable.Repeat(1, 5).ToList());
			source = source.Concat(source);

			yield return new object[] {Enumerable.Repeat(1, 18).ToList(), source};
		}

		public static IEnumerable<object[]> ChainedCollectionConcatData() =>
			GenerateSourcesData(innerTransform: e => e.ToList());

		public static IEnumerable<object[]> AppendedPrependedConcatAlternationsData()
		{
			const int ListsCount = 4; // How many enumerables to concat together per test case.

			var foundation = Array.Empty<int>();
			var expected = new List<int>();
			IReadOnlyList<int> actual = foundation;

			// each bit in the last EnumerableCount bits of i represent whether we want to prepend/append a sequence for this iteration.
			// if it's set, we'll prepend. otherwise, we'll append.
			for (int i = 0; i < (1 << ListsCount); i++)
			{
				// each bit in last EnumerableCount bits of j is set if we want to ensure the nth enumerable
				// concat'd is an ICollection.
				// Note: It is important we run over the all-bits-set case, since currently
				// Concat is specialized for when all inputs are ICollection.
				for (int j = 0; j < (1 << ListsCount); j++)
				{
					for (int k = 0;
						k < ListsCount;
						k++) // k is how much bits we shift by, and also the item that gets appended/prepended.
					{
						var nextRange = Enumerable.Range(k, 1).ToList();
						bool prepend = ((i >> k) & 1) != 0;

						actual = prepend ? nextRange.Concat(actual) : actual.Concat(nextRange);
						if (prepend)
						{
							expected.Insert(0, k);
						}
						else
						{
							expected.Add(k);
						}
					}

					yield return new object[] {expected.ToArray(), actual.ToArray()};

					actual = foundation;
					expected.Clear();
				}
			}
		}

		private static IEnumerable<object[]> GenerateSourcesData(
			Func<IReadOnlyList<int>, IReadOnlyList<int>> outerTransform = null,
			Func<IReadOnlyList<int>, IReadOnlyList<int>> innerTransform = null)
		{
			outerTransform = outerTransform ?? (e => e);
			innerTransform = innerTransform ?? (e => e);

			for (int i = 0; i <= 6; i++)
			{
				var expected = Enumerable.Range(0, i * 3).ToList();
				IReadOnlyList<int> actual = Array.Empty<int>();
				for (int j = 0; j < i; j++)
				{
					actual = outerTransform(actual.Concat(innerTransform(Enumerable.Range(j * 3, 3).ToList())));
				}

				yield return new object[] {expected, actual};
			}
		}

		[Theory]
		[MemberData(nameof(ManyConcatsData))]
		public void ManyConcats(IEnumerable<IReadOnlyList<int>> sources, IReadOnlyList<int> expected)
		{
			foreach (var transform in IdentityTransforms<int>())
			{
				IReadOnlyList<int> concatee = Array.Empty<int>();
				foreach (var source in sources)
				{
					concatee = concatee.Concat(transform(source));
				}

				Assert.Equal(sources.Sum(s => s.Count), concatee.Count);
				VerifyEqualsWorker(sources.SelectMany(s => s).ToList(), concatee);
			}
		}

		[Theory]
		[MemberData(nameof(ManyConcatsData))]
		public void ManyConcatsRunOnce(IEnumerable<IReadOnlyList<int>> sources, IReadOnlyList<int> expected)
		{
			foreach (var transform in IdentityTransforms<int>())
			{
				IReadOnlyList<int> concatee = Array.Empty<int>();
				foreach (var source in sources)
				{
					concatee = concatee.RunOnce().Concat(transform(source));
				}

				Assert.Equal(sources.Sum(s => s.Count()), concatee.Count());
			}
		}

		public static IEnumerable<object[]> ManyConcatsData()
		{
			yield return new object[] {Enumerable.Repeat(Array.Empty<int>(), 256), Array.Empty<int>()};
			yield return new object[]
				{Enumerable.Repeat(Enumerable.Repeat(6, 1).ToList(), 256), Enumerable.Repeat(6, 256).ToList()};
			// Make sure Concat doesn't accidentally swap around the sources, e.g. [3, 4], [1, 2] should not become [1..4]
			yield return new object[]
			{
				Enumerable.Range(0, 500).Select(i => Enumerable.Repeat(i, 1).ToList()).Reverse(),
				((IReadOnlyList<int>) Enumerable.Range(0, 500).ToList()).Reverse()
			};
		}

		[Fact]
		public void CountOfConcatIteratorShouldThrowExceptionOnIntegerOverflow()
		{
			var list = new InfiniteList<int>();
			Assert.Throws<OverflowException>(() => list.Concat(list).Count);
		}
	}
}