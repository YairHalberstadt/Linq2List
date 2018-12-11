# Linq2List

### Nuget

`Install-Package Linq2List -Version 1.0.0`

https://www.nuget.org/packages/Linq2List/1.0.0

### Aims

This repo provides a Linq-like set of extension methods to IReadOnlyList for those Linq methods which are either more performant on Lists, or for which it is possible to return an IReadOnlyList.

This allows for improved performance and ease of use in a number of situations.

The code in this repo aims to be heavily focused on performance. With time I hope to add automated benchmark tests, to test both speed and allocation costs.

I have not, and do not not intend to, reimplement all Linq methods. Only those where a performance or ease of use advantage could be provided by specifying an extension specifically on an IReadOnlylist are supported. A good way to test for performance advantages is to see if coreFX currently checks for IList. If they do, there almost certainly is an advantage.

### Usage

The extension methods try to mimic Linq as close as possible. As such they are lazily executed, and have a very similiar API.

One point to bear in mind, is that rather than throwing an exception, modifying a collection whilst an enumerator is running is undefined behaviour. Various optimizations can thus be made under the assumption that this wont happen, but there will not necessarily be any warning if it does.

The Linq2List extension methods use for loop enumeration rather than foreach enumeration internally. As such, there may be some classes for which they are slower than Linq. For example, indexing an ImmutableList is O(log n) time, so for enumarating it is O(n log n) whereas foreach enumeration is O(n). In general, if getting by index is slower than a call to MoveNext and Current, ListLinq will likely be slower than Linq.

Where the extension methods provided return a collection, the collection will always be an IReadOnlyList. As such indexing into these results, or getting their count is usually a constant time operation. In certain rare cases it may be slightly slower, but still significantly faster than Enumerable.ElementAt or Enumerable.Count.

### Current Supported Methods

Select

Zip

Take

Skip

ElementAt

ElementAtOrDefault

First

FirstOrDefault

Last

LastOrDefault

Reverse

Cast

Concat

Append

Prepend

Range

Repeat

Empty

Count

LongCount

### Potentially Supported Methods

DefaultIfEmpty

Single

SingleOrDefault

Any

SelectMany (would not be only O(1) indexing)

### Examples

Linq2List can be used pretty much exactly the way you would use Linq. However, an IReadOnlyList is returned instead of IEnumerable, allowing you to index into the returned value in constant time, and to get the Count in constant time. Here is a (meaningless) example program which calls almost every avalable method in Linq2List:

```csharp
using System;
using Linq2List;

namespace Linq2ListExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = ReadOnlyList.Range(0, 50);

            var threes = ReadOnlyList.Repeat<int?>(3, 10);

            Console.WriteLine(numbers[3] == threes[8]);

            var nonNullThrees = threes.Cast<int?, int>();

            var joint = numbers.Concat(nonNullThrees);

            var appended = joint.Append(joint[54]);

            var prepended = appended.Prepend(joint.Last()).Prepend(joint.LastOrDefault()).Prepend(joint.First())
                .Prepend(joint.FirstOrDefault());

            var reversed = prepended.Reverse();

            Console.WriteLine(reversed.ElementAtOrDefault(100) == default(int));

            var zipped = reversed.Zip(reversed.Skip(8).Take(10), (a, b) => Math.Max(a, b));

            var squared = zipped.Select(x => x * x);

            var joint2 = squared.Concat(ReadOnlyList.Empty<int>());

            foreach (var element in joint2)
            {
                Console.WriteLine(element);
            }

            for (int i = 0; i < joint2.Count; i++)
            {
                Console.WriteLine(joint2[i] == joint2.ElementAt(i));
            }
        }
    }
}
```

This would print:

```
True
True
9
9
9
2401
2304
2209
2116
2025
1936
1849
True
True
True
True
True
True
True
True
True
True
```

### Contributing

Please feel free to file bug issues, add tests, add benchmarks, and improve the icon artwork, and the chances are I will accept your PR.

If you want to change any of the code in the Linq2List project though, I suggest you open an issue first to discuss what you want to do, as it is mostly in a completed state.

If anyone has the time to implement a MoreLinq2List project which would provide an IReadOnlyList target for a selection of the MoreLinq methods, that would be great!
