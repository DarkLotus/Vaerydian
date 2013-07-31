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
using System.IO;
using System.Collections.Generic;

using fastJSON;

namespace Vaerydian
{
	public class JsonManager
	{
		private JSON j_JSON;

		public JsonManager ()
		{
			j_JSON = fastJSON.JSON.Instance;
			j_JSON.Parameters.EnableAnonymousTypes = true;
		}

		/// <summary>
		/// turns a json string into a string-object dictionary
		/// </summary>
		/// <returns>
		/// the string-object dictionary of the json
		/// </returns>
		/// <param name='json'>
		/// the json string to convert
		/// </param>
		public Dictionary<string,object> jsonToDict (string json)
		{
			return (Dictionary<string,object>) j_JSON.Parse(json);
		}

		/// <summary>
		/// converts a json string to a JsonObject.
		/// </summary>
		/// <returns>a JsonObject of the json string</returns>
		/// <param name="json">a json string</param>
		public JsonObject jsonToJsonObject(string json){
			return new JsonObject (jsonToDict(json));
		}

		/// <summary>
		/// Loads a json file
		/// </summary>
		/// <returns>
		/// the json string
		/// </returns>
		/// <param name='filename'>
		/// Filename
		/// </param>
		public string loadJSON (String filename)
		{
			FileStream fs = null;
			StreamReader sr = null;
			try {
				fs = new FileStream (filename, FileMode.Open);
				sr = new StreamReader (fs);

				string json = sr.ReadToEnd();
				sr.Close();
				fs.Close();
				return json;
			} catch (Exception e) {
				Console.Error.WriteLine("ERROR LOADING JSON:\n" + e.ToString());
				if(fs != null)
					fs.Close();
				if(sr != null)
					sr.Close();
			}

			return "";
		}

		/// <summary>
		/// Saves the json string
		/// </summary>
		/// <returns>
		/// The true if success
		/// </returns>
		/// <param name='filename'>
		/// file to save
		/// </param>
		/// <param name='json'>
		/// json string to save
		/// </param>
		public bool saveJSON (String filename, String json)
		{
			FileStream fs = null;
			StreamWriter sw = null;
			try {
				fs = new FileStream (filename, FileMode.Create);
				sw = new StreamWriter (fs);

				sw.Write(json);

				sw.Close();
				fs.Close();

				return true;
			} catch (Exception e) {
				Console.Error.WriteLine("ERROR SAVING JSON:\n" + e.ToString());
				if(fs != null)
					fs.Close();
				if(sw != null)
					sw.Close();
			}
			return false;
		}

		/// <summary>
		/// Objects to json string.
		/// </summary>
		/// <returns>
		/// The json string.
		/// </returns>
		/// <param name='obj'>
		/// Object.
		/// </param>
		public string objToJsonString(object obj){
			return j_JSON.Beautify(j_JSON.ToJSON(obj));
		}

		/// <summary>
		/// Json to object.
		/// </summary>
		/// <returns>
		/// The object.
		/// </returns>
		/// <param name='json'>
		/// Json string
		/// </param>
		/// <typeparam name='T'>
		/// The type of object
		/// </typeparam>
		public T jsonToObj<T>(string json){
			return j_JSON.ToObject<T>(json);
		}
	}
}

