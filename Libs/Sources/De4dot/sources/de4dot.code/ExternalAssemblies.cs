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

namespace de4dot.code {
	class ExternalAssembly {
		AssemblyDefinition asmDef;

		public ExternalAssembly(AssemblyDefinition asmDef) {
			this.asmDef = asmDef;
		}

		public TypeDefinition resolve(TypeReference type) {
			foreach (var module in asmDef.Modules) {
				var typeDef = DotNetUtils.getType(module, type);
				if (typeDef != null)
					return typeDef;
			}

			return null;
		}

		public void unload(string asmFullName) {
			foreach (var module in asmDef.Modules) {
				DotNetUtils.typeCaches.invalidate(module);
				AssemblyResolver.Instance.removeModule(module);
			}
			AssemblyResolver.Instance.removeModule(asmFullName);
		}
	}

	// Loads assemblies that aren't renamed
	class ExternalAssemblies {
		Dictionary<string, ExternalAssembly> assemblies = new Dictionary<string, ExternalAssembly>();

		ExternalAssembly load(TypeReference type) {
			var asmFullName = DotNetUtils.getFullAssemblyName(type);
			ExternalAssembly asm;
			if (assemblies.TryGetValue(asmFullName, out asm))
				return asm;

			AssemblyDefinition asmDef = null;
			try {
				asmDef = GlobalAssemblyResolver.Instance.Resolve(asmFullName);
			}
			catch (ResolutionException) {
			}
			catch (AssemblyResolutionException) {
			}
			if (asmDef == null) {
				// If we can't load it now, we can't load it later. Make sure above code returns null.
				assemblies[asmFullName] = null;
				Log.w("Could not load assembly {0}", asmFullName);
				return null;
			}
			if (assemblies.ContainsKey(asmDef.Name.FullName)) {
				assemblies[asmFullName] = assemblies[asmDef.Name.FullName];
				return assemblies[asmDef.Name.FullName];
			}

			if (asmFullName == asmDef.Name.FullName)
				Log.v("Loaded assembly {0}", asmFullName);
			else
				Log.v("Loaded assembly {0} (but wanted {1})", asmDef.Name.FullName, asmFullName);

			asm = new ExternalAssembly(asmDef);
			assemblies[asmFullName] = asm;
			assemblies[asmDef.Name.FullName] = asm;
			return asm;
		}

		public TypeDefinition resolve(TypeReference type) {
			var asm = load(type);
			if (asm == null)
				return null;
			return asm.resolve(type);
		}

		public void unloadAll() {
			foreach (var pair in assemblies) {
				if (pair.Value == null)
					continue;
				pair.Value.unload(pair.Key);
			}
			assemblies.Clear();
		}
	}
}
