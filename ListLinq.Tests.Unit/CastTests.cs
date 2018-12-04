using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ListLinq.Tests.Unit
{
    public class CastTests : ListLinqTests
    {
        #region IReadOnlyList CastTests
        [Fact]
        public void CastIntToLongThrows()
        {
            var q = from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
                    select x;

            var rst = q.Cast<int, long>();

            Assert.Throws<InvalidCastException>(() => { foreach (var t in rst) ; });
        }

        [Fact]
        public void CastByteToUShortThrows()
        {
            var q = from x in new byte[] { 0, 255, 127, 128, 1, 33, 99 }
                    select x;

            var rst = q.Cast<byte, ushort>();
            Assert.Throws<InvalidCastException>(() => { foreach (var t in rst) ; });
        }

        [Fact]
        public void EmptySource()
        {
            object[] source = { };
            Assert.Empty(source.Cast<object, int>());

        }

        [Fact]
        public void NullableIntFromAppropriateObjects()
        {
            int? i = 10;
            object[] source = { -4, 1, 2, 3, 9, i };
            int?[] expected = { -4, 1, 2, 3, 9, i };

            Assert.Equal(expected, source.Cast<object, int?>());
        }

        [Fact]
        public void LongFromNullableIntInObjectsThrows()
        {
            int? i = 10;
            object[] source = { -4, 1, 2, 3, 9, i };

            var cast = source.Cast<object, long>();
            Assert.Throws<InvalidCastException>(() => cast.ToList());
        }

        [Fact]
        public void LongFromNullableIntInObjectsIncludingNullThrows()
        {
            int? i = 10;
            object[] source = { -4, 1, 2, 3, 9, null, i };

            var cast = source.Cast<object, long?>();
            Assert.Throws<InvalidCastException>(() => cast.ToList());
        }

        [Fact]
        public void NullableIntFromAppropriateObjectsIncludingNull()
        {
            int? i = 10;
            object[] source = { -4, 1, 2, 3, 9, null, i };
            int?[] expected = { -4, 1, 2, 3, 9, null, i };

            Assert.Equal(expected, source.Cast<object, int?>());
        }

        [Fact]
        public void ThrowOnUncastableItem()
        {
            object[] source = { -4, 1, 2, 3, 9, "45", 11 };
            int[] expectedBeginning = { -4, 1, 2, 3, 9 };

            var cast = source.Cast<object, int>();
            Assert.Throws<InvalidCastException>(() => cast.ToList());
            Assert.Equal(expectedBeginning, cast.Take(5));
            Assert.Throws<InvalidCastException>(() => cast.ElementAt(5));
            Assert.Throws<InvalidCastException>(() => cast[5]);
            Assert.Equal(11, cast[6]);
        }

        [Fact]
        public void ThrowCastingIntToDouble()
        {
            int[] source = { -4, 1, 2, 9 };

            var cast = source.Cast<int, double>();
            Assert.Throws<InvalidCastException>(() => cast.ToList());
        }

        private static void TestCastThrow<T>(object o)
        {
            byte? i = 10;
            object[] source = { -1, 0, o, i };

            var cast = source.Cast<object, T>();
            Assert.Throws<InvalidCastException>(() => cast.ToList());
        }

        [Fact]
        public void ThrowOnHeterogenousSource()
        {
            TestCastThrow<long?>(null);
            TestCastThrow<long>(9L);
        }

        [Fact]
        public void CastToString()
        {
            object[] source = { "Test1", "4.5", null, "Test2" };
            string[] expected = { "Test1", "4.5", null, "Test2" };

            Assert.Equal(expected, source.Cast<object, string>());
        }

        [Fact]
        public void ArrayConversionThrows()
        {
            Assert.Throws<InvalidCastException>(() => new[] { -4 }.Cast<int, long>().ToList());
        }

        [Fact]
        public void FirstElementInvalidForCast()
        {
            object[] source = { "Test", 3, 5, 10 };

            var cast = source.Cast<object, int>();
            Assert.Throws<InvalidCastException>(() => cast.ToList());
        }

        [Fact]
        public void LastElementInvalidForCast()
        {
            object[] source = { -5, 9, 0, 5, 9, "Test" };

            var cast = source.Cast<object, int>();
            Assert.Throws<InvalidCastException>(() => cast.ToList());
        }

        [Fact]
        public void NullableIntFromNullsAndInts()
        {
            object[] source = { 3, null, 5, -4, 0, null, 9 };
            int?[] expected = { 3, null, 5, -4, 0, null, 9 };

            Assert.Equal(expected, source.Cast<object, int?>());
        }

        [Fact]
        public void ThrowCastingIntToLong()
        {
            int[] source = { -4, 1, 2, 3, 9 };

            var cast = source.Cast<int, long>();
            Assert.Throws<InvalidCastException>(() => cast.ToList());
        }

        [Fact]
        public void ThrowCastingIntToNullableLong()
        {
            int[] source = { -4, 1, 2, 3, 9 };

            var cast = source.Cast<int, long?>();
            Assert.Throws<InvalidCastException>(() => cast.ToList());
        }

        [Fact]
        public void ThrowCastingNullableIntToLong()
        {
            int?[] source = { -4, 1, 2, 3, 9 };

            var cast = source.Cast<int?, long>();
            Assert.Throws<InvalidCastException>(() => cast.ToList());
        }

        [Fact]
        public void ThrowCastingNullableIntToNullableLong()
        {
            int?[] source = { -4, 1, 2, 3, 9, null };

            var cast = source.Cast<int?, long?>();
            Assert.Throws<InvalidCastException>(() => cast.ToList());
        }

        [Fact]
        public void CastingNullToNonnullableIsNullReferenceException()
        {
            int?[] source = { -4, 1, null, 3 };
            var cast = source.Cast<int?, int>();
            Assert.Throws<NullReferenceException>(() => cast.ToList());
        }

        [Fact]
        public void NullSource()
        {
            AssertExtensions.Throws<ArgumentNullException>("source", () => ((IReadOnlyList<object>)null).Cast<object, string>());
        }

        [Fact]
        public void Cast()
        {
            var count = (new object[] { 0, 1, 2 }).Cast<object, int>().Count();
            Assert.Equal(3, count);
        }

		[Fact]
		public void NullableIntFromAppropriateObjectsRunOnce()
		{
			int? i = 10;
			object[] source = { -4, 1, 2, 3, 9, i };
			int?[] expected = { -4, 1, 2, 3, 9, i };

			Assert.Equal(expected, source.RunOnce().Cast<object, int?>());
		}

		[Theory]
		[MemberData(nameof(IteratorTestsData))]
		public void RunIteratorTests(IReadOnlyList<int> source)
		{
			var iterator = source.Cast<int, int?>();
			new IteratorTests().RunTests(iterator);
		}

		#endregion

		#region IReadOnlyList<object> CastTests

		[Fact]
		public void CastStringToLongThrows()
		{
			var q = from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
					select x.ToString();

			var rst = q.Cast<long>();

			Assert.Throws<InvalidCastException>(() => { foreach (var t in rst) ; });
		}

		[Fact]
		public void EmptySource2()
		{
			object[] source = { };
			Assert.Empty(source.Cast<int>());

		}

		[Fact]
		public void NullableIntFromAppropriateObjects2()
		{
			int? i = 10;
			object[] source = { -4, 1, 2, 3, 9, i };
			int?[] expected = { -4, 1, 2, 3, 9, i };

			Assert.Equal(expected, source.Cast<int?>());
		}

		[Fact]
		public void LongFromNullableIntInObjectsThrows2()
		{
			int? i = 10;
			object[] source = { -4, 1, 2, 3, 9, i };

			var cast = source.Cast<long>();
			Assert.Throws<InvalidCastException>(() => cast.ToList());
		}

		[Fact]
		public void LongFromNullableIntInObjectsIncludingNullThrows2()
		{
			int? i = 10;
			object[] source = { -4, 1, 2, 3, 9, null, i };

			var cast = source.Cast<long?>();
			Assert.Throws<InvalidCastException>(() => cast.ToList());
		}

		[Fact]
		public void NullableIntFromAppropriateObjectsIncludingNull2()
		{
			int? i = 10;
			object[] source = { -4, 1, 2, 3, 9, null, i };
			int?[] expected = { -4, 1, 2, 3, 9, null, i };

			Assert.Equal(expected, source.Cast<int?>());
		}

		[Fact]
		public void ThrowOnUncastableItem2()
		{
			object[] source = { -4, 1, 2, 3, 9, "45", 11 };
			int[] expectedBeginning = { -4, 1, 2, 3, 9 };

			var cast = source.Cast<int>();
			Assert.Throws<InvalidCastException>(() => cast.ToList());
			Assert.Equal(expectedBeginning, cast.Take(5));
			Assert.Throws<InvalidCastException>(() => cast.ElementAt(5));
			Assert.Throws<InvalidCastException>(() => cast[5]);
			Assert.Equal(11, cast[6]);
		}

		[Fact]
		public void ThrowCastingIntToDouble2()
		{
			object[] source = { -4, 1, 2, 9 };

			var cast = source.Cast<double>();
			Assert.Throws<InvalidCastException>(() => cast.ToList());
		}

		private static void TestCastThrow2<T>(object o)
		{
			byte? i = 10;
			object[] source = { -1, 0, o, i };

			var cast = source.Cast<T>();
			Assert.Throws<InvalidCastException>(() => cast.ToList());
		}

		[Fact]
		public void ThrowOnHeterogenousSource2()
		{
			TestCastThrow<long?>(null);
			TestCastThrow<long>(9L);
		}

		[Fact]
		public void CastToString2()
		{
			object[] source = { "Test1", "4.5", null, "Test2" };
			string[] expected = { "Test1", "4.5", null, "Test2" };

			Assert.Equal(expected, source.Cast<string>());
		}

		[Fact]
		public void FirstElementInvalidForCast2()
		{
			object[] source = { "Test", 3, 5, 10 };

			var cast = source.Cast<int>();
			Assert.Throws<InvalidCastException>(() => cast.ToList());
		}

		[Fact]
		public void LastElementInvalidForCast2()
		{
			object[] source = { -5, 9, 0, 5, 9, "Test" };

			var cast = source.Cast<int>();
			Assert.Throws<InvalidCastException>(() => cast.ToList());
		}

		[Fact]
		public void NullableIntFromNullsAndInts2()
		{
			object[] source = { 3, null, 5, -4, 0, null, 9 };
			int?[] expected = { 3, null, 5, -4, 0, null, 9 };

			Assert.Equal(expected, source.Cast<int?>());
		}

		[Fact]
		public void CastingNullToNonnullableIsNullReferenceException2()
		{
			object[] source = { -4, 1, null, 3 };
			var cast = source.Cast<int>();
			Assert.Throws<NullReferenceException>(() => cast.ToList());
		}

		[Fact]
		public void NullSource2()
		{
			AssertExtensions.Throws<ArgumentNullException>("source", () => ((IReadOnlyList<object>)null).Cast<string>());
		}

		[Fact]
		public void Cast2()
		{
			var count = (new object[] { 0, 1, 2 }).Cast<int>().Count();
			Assert.Equal(3, count);
		}

		[Fact]
		public void NullableIntFromAppropriateObjectsRunOnce2()
		{
			int? i = 10;
			object[] source = { -4, 1, 2, 3, 9, i };
			int?[] expected = { -4, 1, 2, 3, 9, i };

			Assert.Equal(expected, source.RunOnce().Cast<object, int?>());
		}

		[Theory]
		[MemberData(nameof(IteratorTestsData))]
		public void RunIteratorTests2(IReadOnlyList<int> source)
		{
			var iterator = source.Cast<int, object>().Cast<int?>();
			new IteratorTests().RunTests(iterator);
		}

		#endregion

		public static IEnumerable<object[]> IteratorTestsData()
		{
			yield return new object[] { Array.Empty<int>() };
			yield return new object[] { new int[1] };
			yield return new object[] { Enumerable.Range(1, 30).ToList() };
		}
	}

}