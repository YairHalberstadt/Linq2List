# IReadOnlyListLinq

### Aims

To provide a Linq-like set of extension methods to IReadOnlyList for those Linq methods which are either more performant on Lists, or for which it is possible to return an IReadOnlyList.

This should allow improved performance and ease of use in a number of situations.

The code in this repo aims to be heavily focused on performance. With time I hope to add automated benchmark tests, to test both speed and allocation costs.

I do not intend to reimplement all Linq methods. Only those where a performance or ease of use advantage could be provided by specifying an extension specifically on an IReadOnlylist will be supported. A good way to test for performance advantages is to see if coreFX currently checks for IList. If they do, there almost certainly is an advantage.

### Usage

The extension methods try to mimic Linq as close as possible. As such they are lazily executed, and have a very similiar API.

One point to bear in mind, is that rather than throwing an exception, modifying a collection whilst an enumerator is running is undefined behaviour. Various optimizations can thus be made under the assumption that this wont happen, but there will not necessarily be any warning if it does.

The IReadOnlyListLinq extension methods use for loop enumeration rather than foreach enumeration internally. As such, there may be some classes for which they are slower than Linq. For example, indexing an ImmutableList is O(log n) time, so for enumarating it is O(n log n) whereas foreach enumeration is O(n). In general, if getting by index is slower than call to MoveNext and Current, IReadOnlyListLinq will likely be slower than Linq.

Where possible the extension methods return an IReadOnlyList. As such indexing into these results, or getting their count is usual a constant time operation.

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

### Planned Supported Methods

Reverse

Cast

Any

Concat

DefaultIfEmpty

Count

LongCount

Single

Append

Prepend