/*
 Author:
      Thomas H. Jonell <@Net_Gnome>
 
 Copyright (c) 2013 Thomas H. Jonell

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Lesser General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;

namespace Vaerydian.Utils
{
	/// <summary>
	/// Switch case delegate for DynamicSwitch
	/// </summary>
	public delegate void SwitchCase<T>(T tCase);

	/// <summary>
	/// Dyanmic switch creates a programatic switch structure that is redefinable at runtime.
	/// </summary>
	public class DynamicSwitch<T>{
		
		private Dictionary<T,SwitchCase<T>> d_SwitchDict = new Dictionary<T,SwitchCase<T>>();
		private SwitchCase<T> d_Default = null;

		/// <summary>
		/// Adds a case to the dynamic switch
		/// </summary>
		/// <returns><c>true</c>, if case was added, <c>false</c> otherwise.</returns>
		/// <param name="tCase">T case identifier</param>
		/// <param name="method">Method delegate</param>
		public bool addCase(T tCase, SwitchCase<T> method){
			if (d_SwitchDict.ContainsKey (tCase))
				return false;
			else {
				d_SwitchDict.Add(tCase,method);
				return true;
			}
		}

		/// <summary>
		/// Removes a dynamic case and its delegate
		/// </summary>
		/// <returns><c>true</c>, if case was removed, <c>false</c> otherwise.</returns>
		/// <param name="tCase">T case identifier to remove</param>
		public bool removeCase(T tCase){
			if (d_SwitchDict.ContainsKey (tCase)) {
				return d_SwitchDict.Remove (tCase);
			} else
				return false;
		}

		/// <summary>
		/// Sets the default case
		/// </summary>
		/// <returns><c>true</c>, if default was set, <c>false</c> otherwise.</returns>
		/// <param name="method">Method delegte</param>
		public bool setDefault(SwitchCase<T> method){
			d_Default = method;
			return true;
		}

		/// <summary>
		/// Does the default case
		/// </summary>
		public void doDefault(){
			d_Default.Invoke (default (T));
		}

		/// <summary>
		/// Performs the switch
		/// </summary>
		/// <param name="tCase">T case to be switched</param>
		public void doSwitch(T tCase){
			if (d_SwitchDict.ContainsKey (tCase))
				d_SwitchDict [tCase].Invoke (tCase);
			else if (d_Default != null)
				doDefault ();
			else
				return;
		}
		
	}
}

