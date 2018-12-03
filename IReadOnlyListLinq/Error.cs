using System;
using System.Collections.Generic;
using System.Text;

namespace ListLinq
{
    internal static class Error
	{
		public static Exception InvalidState => new InvalidStateException.InvalidStateException(
			$"This Exception should never be thrown, and indicates that an invalid state has been reached. If this exception is thrown in your code, please contact the Library maintainers at {RepoInformation.IssueReportingLocation}. DO NOT catch this exception, as your program will not be in a predictable state.");

	}
}
