# IReadOnlyListLinq

### Aims

To provide a Linq-like set of extension methods to IReadOnlyList for those Linq methods which are either more performant on Lists, or for which it is possible to return an IReadOnlyList.

This should allow improved performance and ease of use in a number of situations.

The code in this repo aims to be heavily focused on performance. With time I hope to add automated benchmark tests, to test both speed and allocation costs.

I do not intend to reimplement all Linq methods. Only those where a performance or ease of use advantage could be provided by specifying an extension specifically on an IReadOnlylist will be supported. A good way to test for performance advantages is to see if coreFX currently checks for IList. If they do, there almost certainly is an advantage.

### Current Supported Methods

Select

### Planned Supported Methods

Take
Skip
Zip
ElementAt
ElementAtOrDefault
First
FirstOrDefault
Last
LastOrDefault
Reverse
Cast
Any
Concat
DefaultIfEmpty
Count
LongCount
