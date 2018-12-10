using System;
using System.Collections.Generic;

namespace Linq2List
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