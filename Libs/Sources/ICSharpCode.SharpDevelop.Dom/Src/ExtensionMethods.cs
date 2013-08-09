// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <author name="Daniel Grunwald"/>
//     <version>$Revision: 2929 $</version>
// </file>

using System;
using System.Collections;
using System.Collections.Generic;

namespace ICSharpCode.SharpDevelop.Dom
{
	/// <summary>
	/// Description of ExtensionMethods.
	/// </summary>
	static class ExtensionMethods
	{
		public static void AddRange(this ArrayList arrayList, IEnumerable elements)
		{
			foreach (object o in elements)
				arrayList.Add(o);
		}
		
		public static void AddRange<T>(this IList<T> list, IEnumerable<T> elements)
		{
			foreach (T o in elements)
				list.Add(o);
		}
	}
}
