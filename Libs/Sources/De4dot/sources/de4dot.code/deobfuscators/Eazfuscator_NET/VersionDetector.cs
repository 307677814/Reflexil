﻿/*
    Copyright (C) 2011-2012 de4dot@gmail.com

    This file is part of de4dot.

    de4dot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    de4dot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with de4dot.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Collections.Generic;
using DeMono.Cecil;
using de4dot.blocks;

namespace de4dot.code.deobfuscators.Eazfuscator_NET {
	class VersionDetector {
		StringDecrypter stringDecrypter;

		public VersionDetector(StringDecrypter stringDecrypter) {
			this.stringDecrypter = stringDecrypter;
		}

		public string detect() {
			var decryptStringType = stringDecrypter.Type;
			var decryptStringMethod = stringDecrypter.Method;
			if (decryptStringType == null || decryptStringMethod == null)
				return null;

			var otherMethods = new List<MethodDefinition>();
			MethodDefinition cctor = null;
			foreach (var method in decryptStringType.Methods) {
				if (method == decryptStringMethod)
					continue;
				if (method.Name == ".cctor")
					cctor = method;
				else
					otherMethods.Add(method);
			}
			if (cctor == null)
				return null;

			bool hasConstantM2 = DeobUtils.hasInteger(decryptStringMethod, -2);

			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////

			var fields11 = new string[] {
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.IO.BinaryReader",
				"System.Byte[]",
				"System.Int16",
				"System.Boolean",
			};
			var locals11 = new string[] {
				"System.Boolean",
				"System.Byte[]",
				"System.Char[]",
				"System.Int16",
				"System.Int32",
				"System.Reflection.Assembly",
				"System.String",
			};
			if (otherMethods.Count == 0 &&
				!hasConstantM2 &&
				!decryptStringMethod.NoInlining &&
				decryptStringMethod.IsPublic &&
				decryptStringMethod.IsSynchronized &&
				decryptStringMethod.Body.MaxStackSize >= 35 &&
				decryptStringMethod.Body.MaxStackSize <= 50 &&
				decryptStringMethod.Body.ExceptionHandlers.Count == 0 &&
				new LocalTypes(decryptStringMethod).exactly(locals11) &&
				checkTypeFields(fields11)) {
				return "1.1 - 1.2";
			}

			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////

			var fields13 = new string[] {
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.IO.BinaryReader",
				"System.Byte[]",
				"System.Int16",
				"System.Boolean",
				"System.Byte[]",
			};
			var locals13 = new string[] {
				"System.Boolean",
				"System.Byte",
				"System.Byte[]",
				"System.Char[]",
				"System.Int16",
				"System.Int32",
				"System.Reflection.Assembly",
				"System.String",
			};
			if (otherMethods.Count == 0 &&
				!hasConstantM2 &&
				!decryptStringMethod.NoInlining &&
				decryptStringMethod.IsPublic &&
				decryptStringMethod.IsSynchronized &&
				decryptStringMethod.Body.MaxStackSize >= 35 &&
				decryptStringMethod.Body.MaxStackSize <= 50 &&
				decryptStringMethod.Body.ExceptionHandlers.Count == 0 &&
				new LocalTypes(decryptStringMethod).exactly(locals13) &&
				checkTypeFields(fields13)) {
				return "1.3";
			}

			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////

			var fields14 = new string[] {
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.IO.BinaryReader",
				"System.Byte[]",
				"System.Int16",
				"System.Boolean",
				"System.Byte[]",
			};
			var locals14 = new string[] {
				"System.Boolean",
				"System.Byte",
				"System.Byte[]",
				"System.Char[]",
				"System.Int16",
				"System.Int32",
				"System.Reflection.Assembly",
				"System.String",
			};
			if (otherMethods.Count == 0 &&
				!hasConstantM2 &&
				!decryptStringMethod.NoInlining &&
				decryptStringMethod.IsPublic &&
				decryptStringMethod.IsSynchronized &&
				decryptStringMethod.Body.MaxStackSize >= 150 &&
				decryptStringMethod.Body.MaxStackSize <= 200 &&
				decryptStringMethod.Body.ExceptionHandlers.Count == 0 &&
				new LocalTypes(decryptStringMethod).exactly(locals14) &&
				checkTypeFields(fields14)) {
				return "1.4 - 2.3";
			}

			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////

			var fields24 = new string[] {
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.IO.BinaryReader",
				"System.Byte[]",
				"System.Int16",
				"System.Boolean",
				"System.Byte[]",
			};
			var locals24 = new string[] {
				"System.Boolean",
				"System.Byte",
				"System.Byte[]",
				"System.Char[]",
				"System.Int16",
				"System.Int32",
				"System.Reflection.Assembly",
				"System.Reflection.AssemblyName",
				"System.String",
			};
			if (otherMethods.Count == 0 &&
				!hasConstantM2 &&
				!decryptStringMethod.NoInlining &&
				decryptStringMethod.IsPublic &&
				decryptStringMethod.IsSynchronized &&
				decryptStringMethod.Body.MaxStackSize >= 1 &&
				decryptStringMethod.Body.MaxStackSize <= 8 &&
				decryptStringMethod.Body.ExceptionHandlers.Count == 0 &&
				new LocalTypes(decryptStringMethod).exactly(locals24) &&
				checkTypeFields(fields24)) {
				return "2.4 - 2.5";
			}

			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////

			var fields26 = new string[] {
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.IO.BinaryReader",
				"System.Byte[]",
				"System.Int16",
				"System.Boolean",
				"System.Byte[]",
			};
			var locals26 = new string[] {
				"System.Boolean",
				"System.Byte",
				"System.Byte[]",
				"System.Char[]",
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.Int16",
				"System.Int32",
				"System.Reflection.Assembly",
				"System.Reflection.AssemblyName",
				"System.String",
			};
			if (otherMethods.Count == 0 &&
				!hasConstantM2 &&
				!decryptStringMethod.NoInlining &&
				decryptStringMethod.IsPublic &&
				!decryptStringMethod.IsSynchronized &&
				decryptStringMethod.Body.MaxStackSize >= 1 &&
				decryptStringMethod.Body.MaxStackSize <= 8 &&
				decryptStringMethod.Body.ExceptionHandlers.Count == 1 &&
				new LocalTypes(decryptStringMethod).exactly(locals26) &&
				checkTypeFields(fields26)) {
				return "2.6";
			}

			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////

			var fields27 = new string[] {
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.IO.BinaryReader",
				"System.Byte[]",
				"System.Int16",
				"System.Boolean",
				"System.Byte[]",
			};
			var locals27 = new string[] {
				"System.Boolean",
				"System.Byte",
				"System.Byte[]",
				"System.Char[]",
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.Int16",
				"System.Int32",
				"System.Reflection.Assembly",
				"System.Reflection.AssemblyName",
				"System.String",
			};
			if (otherMethods.Count == 0 &&
				!hasConstantM2 &&
				decryptStringMethod.NoInlining &&
				decryptStringMethod.IsPublic &&
				!decryptStringMethod.IsSynchronized &&
				decryptStringMethod.Body.MaxStackSize >= 1 &&
				decryptStringMethod.Body.MaxStackSize <= 8 &&
				decryptStringMethod.Body.ExceptionHandlers.Count == 1 &&
				new LocalTypes(decryptStringMethod).exactly(locals27) &&
				checkTypeFields(fields27)) {
				return "2.7";
			}

			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////

			var fields28 = new string[] {
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.IO.BinaryReader",
				"System.Byte[]",
				"System.Int16",
				"System.Boolean",
				"System.Byte[]",
				"System.Boolean",
			};
			var locals28 = new string[] {
				"System.Boolean",
				"System.Byte",
				"System.Byte[]",
				"System.Char[]",
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.Int16",
				"System.Int32",
				"System.Reflection.Assembly",
				"System.Reflection.AssemblyName",
				"System.String",
			};
			if (otherMethods.Count == 0 &&
				!hasConstantM2 &&
				decryptStringMethod.NoInlining &&
				decryptStringMethod.IsAssembly &&
				!decryptStringMethod.IsSynchronized &&
				decryptStringMethod.Body.MaxStackSize >= 1 &&
				decryptStringMethod.Body.MaxStackSize <= 8 &&
				decryptStringMethod.Body.ExceptionHandlers.Count == 1 &&
				new LocalTypes(decryptStringMethod).exactly(locals28) &&
				checkTypeFields(fields28)) {
				return "2.8";
			}

			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////

			var fields29 = new string[] {
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.IO.BinaryReader",
				"System.Byte[]",
				"System.Int16",
				"System.Int32",
				"System.Byte[]",
			};
			var locals29 = new string[] {
				"System.Boolean",
				"System.Byte",
				"System.Byte[]",
				"System.Char[]",
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.Diagnostics.StackFrame",
				"System.Diagnostics.StackTrace",
				"System.Int16",
				"System.Int32",
				"System.IO.Stream",
				"System.Reflection.Assembly",
				"System.Reflection.AssemblyName",
				"System.Reflection.MethodBase",
				"System.String",
				"System.Type",
			};
			if (otherMethods.Count == 0 &&
				!hasConstantM2 &&
				decryptStringMethod.NoInlining &&
				decryptStringMethod.IsAssembly &&
				!decryptStringMethod.IsSynchronized &&
				decryptStringMethod.Body.MaxStackSize >= 1 &&
				decryptStringMethod.Body.MaxStackSize <= 8 &&
				decryptStringMethod.Body.ExceptionHandlers.Count == 2 &&
				new LocalTypes(decryptStringMethod).exactly(locals29) &&
				checkTypeFields(fields29)) {
				return "2.9";
			}

			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////

			var fields30 = new string[] {
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.IO.BinaryReader",
				"System.Byte[]",
				"System.Int16",
				"System.Int32",
				"System.Byte[]",
			};
			var locals30 = new string[] {
				"System.Boolean",
				"System.Byte",
				"System.Byte[]",
				"System.Char[]",
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.Diagnostics.StackFrame",
				"System.Diagnostics.StackTrace",
				"System.Int16",
				"System.Int32",
				"System.IO.Stream",
				"System.Reflection.Assembly",
				"System.Reflection.AssemblyName",
				"System.Reflection.MethodBase",
				"System.String",
				"System.Type",
			};
			var olocals30 = new string[] {
				"System.Int32",
			};
			if (otherMethods.Count == 1 &&
				DotNetUtils.isMethod(otherMethods[0], "System.Int32", "(System.Byte[],System.Int32,System.Byte[])") &&
				otherMethods[0].IsPrivate &&
				otherMethods[0].IsStatic &&
				new LocalTypes(otherMethods[0]).exactly(olocals30) &&
				!hasConstantM2 &&
				decryptStringMethod.NoInlining &&
				decryptStringMethod.IsAssembly &&
				!decryptStringMethod.IsSynchronized &&
				decryptStringMethod.Body.MaxStackSize >= 1 &&
				decryptStringMethod.Body.MaxStackSize <= 8 &&
				decryptStringMethod.Body.ExceptionHandlers.Count == 2 &&
				new LocalTypes(decryptStringMethod).exactly(locals30) &&
				checkTypeFields(fields30)) {
				return "3.0";
			}

			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////

			var fields31 = new string[] {
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.IO.BinaryReader",
				"System.Byte[]",
				"System.Int16",
				"System.Int32",
				"System.Byte[]",
			};
			var locals31 = new string[] {
				"System.Boolean",
				"System.Byte",
				"System.Byte[]",
				"System.Char[]",
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.Diagnostics.StackFrame",
				"System.Diagnostics.StackTrace",
				"System.Int16",
				"System.Int32",
				"System.IO.Stream",
				"System.Reflection.Assembly",
				"System.Reflection.AssemblyName",
				"System.Reflection.MethodBase",
				"System.String",
				"System.Type",
			};
			var olocals31 = new string[] {
				"System.Int32",
			};
			if (otherMethods.Count == 1 &&
				DotNetUtils.isMethod(otherMethods[0], "System.Int32", "(System.Byte[],System.Int32,System.Byte[])") &&
				otherMethods[0].IsPrivate &&
				otherMethods[0].IsStatic &&
				new LocalTypes(otherMethods[0]).exactly(olocals31) &&
				hasConstantM2 &&
				decryptStringMethod.NoInlining &&
				decryptStringMethod.IsAssembly &&
				!decryptStringMethod.IsSynchronized &&
				decryptStringMethod.Body.MaxStackSize >= 1 &&
				decryptStringMethod.Body.MaxStackSize <= 8 &&
				decryptStringMethod.Body.ExceptionHandlers.Count == 2 &&
				new LocalTypes(decryptStringMethod).exactly(locals31) &&
				checkTypeFields(fields31)) {
				return "3.1";
			}

			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////////////

			var fields32 = new string[] {
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.IO.BinaryReader",
				"System.Byte[]",
				"System.Int16",
				"System.Int32",
				"System.Byte[]",
				"System.Int32",
			};
			var locals32 = new string[] {
				"System.Boolean",
				"System.Byte",
				"System.Byte[]",
				"System.Char[]",
				"System.Collections.Generic.Dictionary`2<System.Int32,System.String>",
				"System.Diagnostics.StackFrame",
				"System.Diagnostics.StackTrace",
				"System.Int16",
				"System.Int32",
				"System.Int64",
				"System.IO.Stream",
				"System.Reflection.Assembly",
				"System.Reflection.AssemblyName",
				"System.Reflection.MethodBase",
				"System.String",
				"System.Type",
			};
			var olocals32 = new string[] {
				"System.Int32",
			};
			if (otherMethods.Count == 1 &&
				DotNetUtils.isMethod(otherMethods[0], "System.Void", "(System.Byte[],System.Int32,System.Byte[])") &&
				otherMethods[0].IsPrivate &&
				otherMethods[0].IsStatic &&
				new LocalTypes(otherMethods[0]).exactly(olocals32) &&
				hasConstantM2 &&
				decryptStringMethod.NoInlining &&
				decryptStringMethod.IsAssembly &&
				!decryptStringMethod.IsSynchronized &&
				decryptStringMethod.Body.MaxStackSize >= 1 &&
				decryptStringMethod.Body.MaxStackSize <= 8 &&
				decryptStringMethod.Body.ExceptionHandlers.Count == 2 &&
				new LocalTypes(decryptStringMethod).exactly(locals32) &&
				checkTypeFields(fields32)) {
				return "3.2";
			}

			return null;
		}

		bool checkTypeFields(string[] fieldTypes) {
			if (fieldTypes.Length != stringDecrypter.Type.Fields.Count)
				return false;
			for (int i = 0; i < fieldTypes.Length; i++) {
				if (fieldTypes[i] != stringDecrypter.Type.Fields[i].FieldType.FullName)
					return false;
			}
			return true;
		}
	}
}
