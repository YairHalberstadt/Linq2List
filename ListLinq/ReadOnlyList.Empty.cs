using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ListLinq
{
	public static partial class ReadOnlyList
	{
		/// <summary>
		/// Calls <see cref="Array.Empty{TResult}()"/>
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <returns></returns>
		public static IReadOnlyList<TResult> Empty<TResult>() => Array.Empty<TResult>();
	}
}