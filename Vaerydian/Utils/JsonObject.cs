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

	}
}

