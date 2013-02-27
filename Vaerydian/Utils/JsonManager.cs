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
		}

		public Dictionary<string,object> jsonToDict (string json)
		{
			return j_JSON.Parse(json);
		}


		private string loadJSON (String filename)
		{
			try {
				FileStream fs = new FileStream (filename, FileMode.Open);
				StreamReader sr = new StreamReader (fs);

				string json = sr.ReadToEnd();
				sr.Close();
				fs.Close();
				return json;
			} catch (Exception e) {
				Console.Error.WriteLine("ERROR LOADING JSON:\n" + e.ToString());
			}

			return "";
		}

		private bool saveJSON (String filename, String json)
		{
			try {
				FileStream fs = new FileStream (filename, FileMode.Create);
				StreamWriter sr = new StreamWriter (fs);

				sr.Write(json);

				sr.Close();
				fs.Close();

				return true;
			} catch (Exception e) {
				Console.Error.WriteLine("ERROR SAVING JSON:\n" + e.ToString());
			}
			return false;
		}
	}
}

