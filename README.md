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
