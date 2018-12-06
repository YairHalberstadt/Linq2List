using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ListLinq.Tests.Unit
{
	public class AppendPrependTests : ListLinqTests
	{
		[Fact]
		public void SameResultsRepeatCallsIntQueryAppend()
		{
			var q1 = from x1 in new int?[] { 2, 3, null, 2, null, 4, 5 }
					 select x1;

			Assert.Equal(q1.Append(42), q1.Append(42));
			Assert.Equal(q1.Append(42), q1.Concat(new int?[] { 42 }));
		}

		[Fact]
		public void SameResultsRepeatCallsIntQueryPrepend()
		{
			var q1 = from x1 in new int?[] { 2, 3, null, 2, null, 4, 5 }
					 select x1;

			Assert.Equal(q1.Prepend(42), q1.Prepend(42));
			Assert.Equal(q1.Prepend(42), (new int?[] { 42 }).Concat(q1));
		}

		[Fact]
		public void SameResultsRepeatCallsStringQueryAppend()
		{
			var q1 = from x1 in new[] { "AAA", string.Empty, "q", "C", "#", "!@#$%^", "0987654321", "Calling Twice" }
					 select x1;

			Assert.Equal(q1.Append("hi"), q1.Append("hi"));
			Assert.Equal(q1.Append("hi"), q1.Concat(new string[] { "hi" }));
		}

		[Fact]
		public void SameResultsRepeatCallsStringQueryPrepend()
		{
			var q1 = from x1 in new[] { "AAA", string.Empty, "q", "C", "#", "!@#$%^", "0987654321", "Calling Twice" }
					 select x1;

			Assert.Equal(q1.Prepend("hi"), q1.Prepend("hi"));
			Assert.Equal(q1.Prepend("hi"), (new string[] { "hi" }).Concat(q1));
		}

		[Fact]
		public void RepeatIteration()
		{
			var q = ReadOnlyList.Range(3, 4).Append(12);
			Assert.Equal(q, q);
			q = q.Append(14);
			Assert.Equal(q, q);
		}

		[Fact]
		public void EmptyAppend()
		{
			int[] first = { };
			Assert.Single(first.Append(42), 42);
		}

		[Fact]
		public void EmptyPrepend()
		{
			string[] first = { };
			Assert.Single(first.Prepend("aa"), "aa");
		}

		[Fact]
		public void PrependNoIteratingSourceBeforeFirstItem()
		{
			var ie = new List<int>();
			var prepended = (from i in ie select i).Prepend(4);

			ie.Add(42);

			Assert.Equal(prepended, ie.Prepend(4));
		}

		[Fact]
		public void SourceNull()
		{
			AssertExtensions.Throws<ArgumentNullException>("source", () => ((IEnumerable<int>)null).Append(1));
			AssertExtensions.Throws<ArgumentNullException>("source", () => ((IEnumerable<int>)null).Prepend(1));
		}

		[Fact]
		public void Combined()
		{
			var v = "foo".ToList().Append('1').Append('2').Prepend('3').Concat("qq".ToList().Append('Q').Prepend('W'));

			Assert.Equal(v.ToArray(), "3foo12WqqQ".ToArray());

			var v1 = "a".ToList().Append('b').Append('c').Append('d');

			Assert.Equal(v1.ToArray(), "abcd".ToArray());

			var v2 = "a".ToList().Prepend('b').Prepend('c').Prepend('d');

			Assert.Equal(v2.ToArray(), "dcba".ToArray());
		}

		[Fact]
		public void AppendCombinations()
		{
			var source = ReadOnlyList.Range(0, 3).Append(3).Append(4);
			var app0a = source.Append(5);
			var app0b = source.Append(6);
			var app1aa = app0a.Append(7);
			var app1ab = app0a.Append(8);
			var app1ba = app0b.Append(9);
			var app1bb = app0b.Append(10);

			Assert.Equal(new[] { 0, 1, 2, 3, 4, 5 }, app0a);
			Assert.Equal(new[] { 0, 1, 2, 3, 4, 6 }, app0b);
			Assert.Equal(new[] { 0, 1, 2, 3, 4, 5, 7 }, app1aa);
			Assert.Equal(new[] { 0, 1, 2, 3, 4, 5, 8 }, app1ab);
			Assert.Equal(new[] { 0, 1, 2, 3, 4, 6, 9 }, app1ba);
			Assert.Equal(new[] { 0, 1, 2, 3, 4, 6, 10 }, app1bb);
		}

		[Fact]
		public void PrependCombinations()
		{
			var source = ReadOnlyList.Range(2, 2).Prepend(1).Prepend(0);
			var pre0a = source.Prepend(5);
			var pre0b = source.Prepend(6);
			var pre1aa = pre0a.Prepend(7);
			var pre1ab = pre0a.Prepend(8);
			var pre1ba = pre0b.Prepend(9);
			var pre1bb = pre0b.Prepend(10);

			Assert.Equal(new[] { 5, 0, 1, 2, 3 }, pre0a);
			Assert.Equal(new[] { 6, 0, 1, 2, 3 }, pre0b);
			Assert.Equal(new[] { 7, 5, 0, 1, 2, 3 }, pre1aa);
			Assert.Equal(new[] { 8, 5, 0, 1, 2, 3 }, pre1ab);
			Assert.Equal(new[] { 9, 6, 0, 1, 2, 3 }, pre1ba);
			Assert.Equal(new[] { 10, 6, 0, 1, 2, 3 }, pre1bb);
		}

		[Fact]
		public void Append1ToArrayToList()
		{
			var source = ReadOnlyList.Range(0, 2).Append(2);
			Assert.Equal(ReadOnlyList.Range(0, 3), source.ToList());
			Assert.Equal(ReadOnlyList.Range(0, 3), source.ToArray());
		}

		[Fact]
		public void Prepend1ToArrayToList()
		{
			var source = ReadOnlyList.Range(1, 2).Prepend(0);
			Assert.Equal(ReadOnlyList.Range(0, 3), source.ToList());
			Assert.Equal(ReadOnlyList.Range(0, 3), source.ToArray());
		}

		[Fact]
		public void AppendNToArrayToList()
		{
			var source = ReadOnlyList.Range(0, 2).Append(2).Append(3);
			Assert.Equal(ReadOnlyList.Range(0, 4), source.ToList());
			Assert.Equal(ReadOnlyList.Range(0, 4), source.ToArray());
		}

		[Fact]
		public void PrependNToArrayToList()
		{
			var source = ReadOnlyList.Range(2, 2).Prepend(1).Prepend(0);
			Assert.Equal(ReadOnlyList.Range(0, 4), source.ToList());
			Assert.Equal(ReadOnlyList.Range(0, 4), source.ToArray());
		}

		[Fact]
		public void AppendPrependToArrayToList()
		{
			var source = ReadOnlyList.Range(2, 2).Prepend(1).Append(4).Prepend(0).Append(5);
			Assert.Equal(ReadOnlyList.Range(0, 6), source.ToList());
			Assert.Equal(ReadOnlyList.Range(0, 6), source.ToArray());

			source = ReadOnlyList.Range(2, 2).Append(4).Prepend(1).Append(5).Prepend(0);
			Assert.Equal(ReadOnlyList.Range(0, 6), source.ToList());
			Assert.Equal(ReadOnlyList.Range(0, 6), source.ToArray());

			source = ReadOnlyList.Range(2, 2).Prepend(1).Prepend(0).Append(4).Append(5);
			Assert.Equal(ReadOnlyList.Range(0, 6), source.ToList());
			Assert.Equal(ReadOnlyList.Range(0, 6), source.ToArray());
		}

		[Fact]
		public void AppendPrependRunOnce()
		{
			var source = ReadOnlyList.Range(2, 2).RunOnce().Prepend(1).RunOnce().Prepend(0).RunOnce().Append(4).RunOnce().Append(5).RunOnce();
			Assert.Equal(ReadOnlyList.Range(0, 6), source.ToList());
			source = ReadOnlyList.Range(2, 2).Prepend(1).Prepend(0).Append(4).Append(5).RunOnce();
			Assert.Equal(ReadOnlyList.Range(0, 6), source.ToList());
		}

		[Fact]
		public void AppendPrependMany()
		{
			IReadOnlyList<int> source = new[] {99999, 100000, 100001};
			for (int i = 99998, j = 100002; i >= 0; i--, j++)
			{
				source = source.Prepend(i).Append(j);
			}

			Assert.Equal(source, ReadOnlyList.Range(0, 200001));
		}

		[Fact]
		public void AppendPrependManyTwiceOnEachIterator()
		{
			IReadOnlyList<int> source = new[] { 99999, 100000, 100001 };
			for (int i = 99998, j = 100002; i >= 0; i--, j++)
			{
				source.Prepend(i);
				source = source.Prepend(i);
				source.Append(j);
				source =source.Append(j);
			}

			Assert.Equal(source, ReadOnlyList.Range(0, 200001));
		}

		[Theory]
		[MemberData(nameof(IteratorTestsData))]
		public void RunIteratorTests(IReadOnlyList<int> source)
		{
			var a = source.Append(1000);
			var b = a.Prepend(1001);
			var c = b.Append(1002);
			var d = c.Append(1003);
			var e = d.Prepend(1004);
			var f = e.Prepend(1005);

			Assert.Equal(a.Count, source.Count + 1);
			Assert.Equal(b.Count, source.Count + 2);
			Assert.Equal(c.Count, source.Count + 3);
			Assert.Equal(d.Count, source.Count + 4);
			Assert.Equal(e.Count, source.Count + 5);
			Assert.Equal(f.Count, source.Count + 6);

			var runner = new IteratorTests();
			runner.RunTests(a);
			runner.RunTests(b);
			runner.RunTests(c);
			runner.RunTests(d);
			runner.RunTests(e);
			runner.RunTests(f);
		}

		public static IEnumerable<object[]> IteratorTestsData()
		{
			yield return new object[] { Array.Empty<int>() };
			yield return new object[] { new int[1] };
			yield return new object[] { ReadOnlyList.Range(1, 30) };
		}
	}
}