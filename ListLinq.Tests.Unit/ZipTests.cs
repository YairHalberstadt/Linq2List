using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Linq2List.Tests.Unit
{
	public class ZipTests : ListLinqTests
	{
		[Fact]
		public void ImplicitTypeParameters()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3};
			IReadOnlyList<int> second = new int[] {2, 5, 9};
			IReadOnlyList<int> expected = new int[] {3, 7, 12};

			Assert.Equal(expected, first.Zip(second, (x, y) => x + y));
			Assert.Equal(expected.Count, first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void ExplicitTypeParameters()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3};
			IReadOnlyList<int> second = new int[] {2, 5, 9};
			IReadOnlyList<int> expected = new int[] {3, 7, 12};

			Assert.Equal(expected, first.Zip<int, int, int>(second, (x, y) => x + y));
			Assert.Equal(expected.Count, first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void FirstIsNull()
		{
			IReadOnlyList<int> first = null;
			IReadOnlyList<int> second = new int[] {2, 5, 9};

			AssertExtensions.Throws<ArgumentNullException>("first",
				() => first.Zip<int, int, int>(second, (x, y) => x + y));
		}

		[Fact]
		public void SecondIsNull()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3};
			IReadOnlyList<int> second = null;

			AssertExtensions.Throws<ArgumentNullException>("second",
				() => first.Zip<int, int, int>(second, (x, y) => x + y));
		}

		[Fact]
		public void FuncIsNull()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3};
			IReadOnlyList<int> second = new int[] {2, 4, 6};
			Func<int, int, int> func = null;

			AssertExtensions.Throws<ArgumentNullException>("resultSelector", () => first.Zip(second, func));
		}

		[Fact]
		public void ExceptionThrownFromFirstsReadOnlyList()
		{
			ThrowsOnMatchReadOnlyList<int> first = new ThrowsOnMatchReadOnlyList<int>(new int[] {1, 3, 3}, 2);
			IReadOnlyList<int> second = new int[] {2, 4, 6};
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] {3, 7, 9};

			Assert.Equal(expected, first.Zip(second, func));

			first = new ThrowsOnMatchReadOnlyList<int>(new int[] {1, 2, 3}, 2);

			var zip = first.Zip(second, func);

			Assert.Throws<Exception>(() => zip.ToList());
		}

		[Fact]
		public void ExceptionThrownFromSecondsEnumerator()
		{
			ThrowsOnMatchReadOnlyList<int> second = new ThrowsOnMatchReadOnlyList<int>(new int[] {1, 3, 3}, 2);
			IReadOnlyList<int> first = new int[] {2, 4, 6};
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] {3, 7, 9};

			Assert.Equal(expected, first.Zip(second, func));

			second = new ThrowsOnMatchReadOnlyList<int>(new int[] {1, 2, 3}, 2);

			var zip = first.Zip(second, func);

			Assert.Throws<Exception>(() => zip.ToList());
		}

		[Fact]
		public void FirstAndSecondEmpty()
		{
			IReadOnlyList<int> first = new int[] { };
			IReadOnlyList<int> second = new int[] { };
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] { };

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}


		[Fact]
		public void FirstEmptySecondSingle()
		{
			IReadOnlyList<int> first = new int[] { };
			IReadOnlyList<int> second = new int[] {2};
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] { };

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void FirstEmptySecondMany()
		{
			IReadOnlyList<int> first = new int[] { };
			IReadOnlyList<int> second = new int[] {2, 4, 8};
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] { };

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}


		[Fact]
		public void SecondEmptyFirstSingle()
		{
			IReadOnlyList<int> first = new int[] {1};
			IReadOnlyList<int> second = new int[] { };
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] { };

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void SecondEmptyFirstMany()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3};
			IReadOnlyList<int> second = new int[] { };
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] { };

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void FirstAndSecondSingle()
		{
			IReadOnlyList<int> first = new int[] {1};
			IReadOnlyList<int> second = new int[] {2};
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] {3};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void FirstAndSecondEqualSize()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3};
			IReadOnlyList<int> second = new int[] {2, 3, 4};
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] {3, 5, 7};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void SecondOneMoreThanFirst()
		{
			IReadOnlyList<int> first = new int[] {1, 2};
			IReadOnlyList<int> second = new int[] {2, 4, 8};
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] {3, 6};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}


		[Fact]
		public void SecondManyMoreThanFirst()
		{
			IReadOnlyList<int> first = new int[] {1, 2};
			IReadOnlyList<int> second = new int[] {2, 4, 8, 16};
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] {3, 6};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void FirstOneMoreThanSecond()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3};
			IReadOnlyList<int> second = new int[] {2, 4};
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] {3, 6};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}


		[Fact]
		public void FirstManyMoreThanSecond()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3, 4};
			IReadOnlyList<int> second = new int[] {2, 4};
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] {3, 6};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}


		[Fact]
		public void DelegateFuncChanged()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3, 4};
			IReadOnlyList<int> second = new int[] {2, 4, 8};
			Func<int, int, int> func = (x, y) => x + y;
			IEnumerable<int> expected = new int[] {3, 6, 11};

			Assert.Equal(expected, first.Zip(second, func));

			func = (x, y) => x - y;
			expected = new int[] {-1, -2, -5};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void LambdaFuncChanged()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3, 4};
			IReadOnlyList<int> second = new int[] {2, 4, 8};
			IEnumerable<int> expected = new int[] {3, 6, 11};

			Assert.Equal(expected, first.Zip(second, (x, y) => x + y));

			expected = new int[] {-1, -2, -5};

			Assert.Equal(expected, first.Zip(second, (x, y) => x - y));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void FirstHasFirstElementNull()
		{
			IReadOnlyList<int?> first = new[] {(int?) null, 2, 3, 4};
			IReadOnlyList<int> second = new int[] {2, 4, 8};
			Func<int?, int, int?> func = (x, y) => x + y;
			IEnumerable<int?> expected = new int?[] {null, 6, 11};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void FirstHasLastElementNull()
		{
			IReadOnlyList<int?> first = new[] {1, 2, (int?) null};
			IReadOnlyList<int> second = new int[] {2, 4, 6, 8};
			Func<int?, int, int?> func = (x, y) => x + y;
			IEnumerable<int?> expected = new int?[] {3, 6, null};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void FirstHasMiddleNullValue()
		{
			IReadOnlyList<int?> first = new[] {1, (int?) null, 3};
			IReadOnlyList<int> second = new int[] {2, 4, 6, 8};
			Func<int?, int, int?> func = (x, y) => x + y;
			IEnumerable<int?> expected = new int?[] {3, null, 9};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void FirstAllElementsNull()
		{
			IReadOnlyList<int?> first = new int?[] {null, null, null};
			IReadOnlyList<int> second = new int[] {2, 4, 6, 8};
			Func<int?, int, int?> func = (x, y) => x + y;
			IEnumerable<int?> expected = new int?[] {null, null, null};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void SecondHasFirstElementNull()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3, 4};
			IReadOnlyList<int?> second = new int?[] {null, 4, 6};
			Func<int, int?, int?> func = (x, y) => x + y;
			IEnumerable<int?> expected = new int?[] {null, 6, 9};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void SecondHasLastElementNull()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3, 4};
			IReadOnlyList<int?> second = new int?[] {2, 4, null};
			Func<int, int?, int?> func = (x, y) => x + y;
			IEnumerable<int?> expected = new int?[] {3, 6, null};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void SecondHasMiddleElementNull()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3, 4};
			IReadOnlyList<int?> second = new int?[] {2, null, 6};
			Func<int, int?, int?> func = (x, y) => x + y;
			IEnumerable<int?> expected = new int?[] {3, null, 9};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void SecondHasAllElementsNull()
		{
			IReadOnlyList<int> first = new int[] {1, 2, 3, 4};
			IReadOnlyList<int?> second = new int?[] {null, null, null};
			Func<int, int?, int?> func = (x, y) => x + y;
			IEnumerable<int?> expected = new int?[] {null, null, null};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void SecondLargerFirstAllNull()
		{
			IReadOnlyList<int?> first = new int?[] {null, null, null, null};
			IReadOnlyList<int?> second = new int?[] {null, null, null};
			Func<int?, int?, int?> func = (x, y) => x + y;
			IEnumerable<int?> expected = new int?[] {null, null, null};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}


		[Fact]
		public void FirstSameSizeSecondAllNull()
		{
			IReadOnlyList<int?> first = new int?[] {null, null, null};
			IReadOnlyList<int?> second = new int?[] {null, null, null};
			Func<int?, int?, int?> func = (x, y) => x + y;
			IEnumerable<int?> expected = new int?[] {null, null, null};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void FirstSmallerSecondAllNull()
		{
			IReadOnlyList<int?> first = new int?[] {null, null, null};
			IReadOnlyList<int?> second = new int?[] {null, null, null, null};
			Func<int?, int?, int?> func = (x, y) => x + y;
			IEnumerable<int?> expected = new int?[] {null, null, null};

			Assert.Equal(expected, first.Zip(second, func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void RunOnce()
		{
			IReadOnlyList<int?> first = new[] {1, (int?) null, 3};
			IReadOnlyList<int> second = new[] {2, 4, 6, 8};
			Func<int?, int, int?> func = (x, y) => x + y;
			IEnumerable<int?> expected = new int?[] {3, null, 9};

			Assert.Equal(expected, first.RunOnce().Zip(second.RunOnce(), func));
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void EnumerateZippedEnumerableMultipleTimes()
		{
			IReadOnlyList<int?> first = new[] {1, (int?) null, 3};
			IReadOnlyList<int> second = new[] {2, 4, 6, 8};
			Func<int?, int, int?> func = (x, y) => x + y;
			IEnumerable<int?> expected = new int?[] {3, null, 9};

			var zipped = first.Zip(second, func);
			Assert.Equal(zipped, expected);
			Assert.Equal(zipped, expected);
			Assert.Equal(zipped.Select(x => x), expected);
			zipped.Take(1);
			Assert.Equal(zipped, expected);

			Assert.Equal(expected, first.RunOnce().Zip(second.RunOnce(), func).RunOnce());
			Assert.Equal(expected.Count(), first.Zip(second, (x, y) => x + y).Count);
		}

		[Fact]
		public void ForAndForeachEnumerationMatchesExpected()
		{
			IReadOnlyList<int?> first = new[] {1, (int?) null, 3};
			IReadOnlyList<int> second = new[] {2, 4, 6, 8};
			Func<int?, int, int?> func = (x, y) => x + y;
			var expected = new int?[] {3, null, 9};

			var zipped = first.Zip(second, func);
			for (int i = 0; i < zipped.Count; i++)
			{
				var value = zipped[i];
				Assert.Equal(value, expected[i]);
			}

			var index = 0;
			foreach (int? value in zipped)
			{
				Assert.Equal(value, expected[index]);
				index++;
			}
		}

		[Theory]
		[MemberData(nameof(IteratorTestsData))]
		public void RunIteratorTests(IReadOnlyList<int> source)
		{
			foreach (var list in IteratorTestsData().SelectMany(x => x).Cast<IReadOnlyList<int>>())
			{
				var iterator = source.Zip(list, (a, b) => (a, b));
				new IteratorTests().RunTests(iterator);
			}
		}

		public static IEnumerable<object[]> IteratorTestsData()
		{
			yield return new object[] {Array.Empty<int>()};
			yield return new object[] {new int[1]};
			yield return new object[] { ReadOnlyList.Range(1, 30)};
		}

		#region Zip2Tuple

		[Fact]
		public void Zip2_CorrectResults()
		{
			int[] first = new int[] {1, 2, 3};
			int[] second = new int[] {2, 5, 9};
			var expected = new (int, int)[] {(1, 2), (2, 5), (3, 9)};
			Assert.Equal(expected, first.Zip(second));
		}

		[Fact]
		public void Zip2_FirstIsNull()
		{
			IReadOnlyList<int> first = null;
			int[] second = new int[] {2, 5, 9};
			AssertExtensions.Throws<ArgumentNullException>("first", () => first.Zip(second));
		}

		[Fact]
		public void Zip2_SecondIsNull()
		{
			int[] first = new int[] {1, 2, 3};
			IReadOnlyList<int> second = null;
			AssertExtensions.Throws<ArgumentNullException>("second", () => first.Zip(second));
		}

		[Fact]
		public void Zip2()
		{
			int count = new [] {0, 1, 2}.Zip(new []{10, 11, 12}).Count();
			Assert.Equal(3, count);
		}

		[Fact]
		public void TupleNames()
		{
			int[] first = {1};
			int[] second = {2};
			var tuple = first.Zip(second).First();
			Assert.Equal(tuple.Item1, tuple.First);
			Assert.Equal(tuple.Item2, tuple.Second);
		}

		[Theory]
		[MemberData(nameof(IteratorTestsData))]
		public void Zip2RunIteratorTests(IReadOnlyList<int> source)
		{
			foreach (var list in IteratorTestsData().SelectMany(x => x).Cast<IReadOnlyList<int>>())
			{
				var iterator = source.Zip(list);
				new IteratorTests().RunTests(iterator);
			}
		}

		#endregion
	}
}