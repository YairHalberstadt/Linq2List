using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Linq2List.Tests.Unit
{
    public class IteratorTests : ListLinqTests
    {
		public void RunTests<TSource>(IReadOnlyList<TSource> iterator)
		{
			TestForEachMatchesIndexer(iterator);

			if (iterator is IList<TSource> list)
			{
				RunIListTests(list);
			}
			else
			{
				Assert.True(false, "An iterator should implement IList");
			}
		}

		private void TestForEachMatchesIndexer<TSource>(IReadOnlyList<TSource> iterator)
		{
			var index = 0;
			foreach (var elem in iterator)
			{
				Assert.Equal(elem, iterator[index]);
				index++;
			}

			Assert.Equal(index, iterator.Count);

			//Run Twice, to make sure iterator resets

			var index2 = 0;
			foreach (var elem in iterator)
			{
				Assert.Equal(elem, iterator[index2]);
				index2++;
			}

			Assert.Equal(index2, iterator.Count);
		}

		private void RunIListTests<TSource>(IList<TSource> iterator)
		{
			Assert.True(iterator.IsReadOnly);
			Assert.Throws<NotSupportedException>(() => iterator.Add(default));
			Assert.Throws<NotSupportedException>(iterator.Clear);
			Assert.Throws<NotSupportedException>(() => iterator.Insert(0, default));
			Assert.Throws<NotSupportedException>(() => iterator.Remove(default));
			Assert.Throws<NotSupportedException>(() => iterator.RemoveAt(0));

			for (int i = 0; i < iterator.Count; i++)
			{
				var elem = iterator[i];
				if(i != iterator.IndexOf(elem))
					Assert.Equal(elem, iterator[iterator.IndexOf(elem)]);
				Assert.True(iterator.Contains(elem));
			}

			Assert.Throws<ArgumentNullException>(() => iterator.CopyTo(null, 0));
			Assert.Throws<ArgumentOutOfRangeException>(() => iterator.CopyTo(new TSource[iterator.Count], -1));
			if(iterator.Count > 0)
				Assert.Throws<ArgumentException>(() => iterator.CopyTo(new TSource[iterator.Count - 1], 0));
			Assert.Throws<ArgumentException>(() => iterator.CopyTo(new TSource[iterator.Count], 1));

			var array = new TSource[iterator.Count + 1];
			iterator.CopyTo(array, 1);
			Assert.Equal(iterator.Prepend(default), array);
		}

	}
}
