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

namespace Vaerydian
{
	public class JsonObject
	{
		private Dictionary<string, object> j_JsonDict = new Dictionary<string, object>();
		private object j_InternalObject;

		public JsonObject (Dictionary<string,object> jsonDict)
		{
			j_JsonDict = jsonDict;
		}

		/// <summary>
		/// changes the internal object to the locationspecified in the index accessor
		/// </summary>
		/// <param name="values">location to move the internal object in the json file</param>
		public JsonObject this [params string[] values] {
			get{
				try{
					if(values.Length < 1)
						return null;

					var retVal = j_JsonDict;

					for(int i = 0; i < values.Length-1; i++){
						retVal = (Dictionary<string,object>) retVal[values[i]];
					}

					j_InternalObject = retVal[values[values.Length-1]];
					return this;
				}catch(Exception e){
					Console.Error.WriteLine("ERROR: Couldn't access location: " + stringIndex(values) + "\n"
					                        + e.ToString());
					throw e;//re-throw e because the error may need to bubble up.
				}
			}
		}

		/// <summary>
		/// pretty prints the json index values
		/// </summary>
		/// <returns>a pretty string</returns>
		/// <param name="values">index values</param>
		private string stringIndex(string[] values){
			string retstr = "";
			for (int i = 0; i < values.Length; i++) {
				retstr += "." + values[i];
			}
			return retstr;
		}

		public short asShort(){
			return (short)(long)j_InternalObject;
		}

		public int asInt(){
			return (int)(long)j_InternalObject;
		}

		public long asLong(){
			return (long)j_InternalObject;
		}

		public float asFloat(){
			return (float)(double)j_InternalObject;
		}

		public double asDouble(){
			return (double)j_InternalObject;
		}

		public string asString(){
			return (string)j_InternalObject;
		}

		public bool asBool(){
			return (bool)j_InternalObject;
		}

		public List<T> asList<T>(){
			List<object> objList = (List<object>) j_InternalObject;
			List<T> returnList = new List<T> ();
			
			for (int i = 0; i < objList.Count; i++) {
				returnList.Add((T) objList[i]);
			}
			
			return returnList;
		}

		public T asEnum<T>(){
			return (T) Enum.Parse (typeof(T), (string)j_InternalObject);
		}

		public T asObject<T>(){
			return (T)j_InternalObject;
		}

		/// <summary>
		/// checks to see if the index values exist in the current json
		/// </summary>
		/// <returns><c>true</c>, if entry exists, <c>false</c> otherwise.</returns>
		/// <param name="values">index values</param>
		public bool hasEntry(params string[] values){
			if(values.Length < 1)
				return false;
			
			var val = j_JsonDict;
			bool retVal = false;
			
			for (int i = 0; i < values.Length-1; i++) {
				
				if (val.ContainsKey(values [i])){
					val = (Dictionary<string,object>)val[values [i]];
					retVal = true;
					continue;
				}else{
					retVal = false;
					break;
				}
			}
			
			if (val.ContainsKey (values [values.Length - 1]))
				retVal = true;
			else
				retVal = false;
			
			
			return retVal;
		}
	}
}

