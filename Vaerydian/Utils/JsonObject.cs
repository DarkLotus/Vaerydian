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

		public JsonObject this [params string[] values] {
			get{
				if(values.Length < 1)
					return null;

				var retVal = j_JsonDict;

				for(int i = 0; i < values.Length-1; i++){
					retVal = (Dictionary<string,object>) retVal[values[i]];
				}

				j_InternalObject = retVal[values[values.Length-1]];
				return this;
			}
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

