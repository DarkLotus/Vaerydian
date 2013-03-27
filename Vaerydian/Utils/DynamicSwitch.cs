using System;
using System.Collections.Generic;

namespace Vaerydian.Utils
{
	public delegate void SwitchCase();
	
	public class DyanmicSwitch<T>{
		
		private Dictionary<T,SwitchCase> d_SwitchDict = new Dictionary<T,SwitchCase>();
		private SwitchCase d_Default = null;
		
		public bool addCase(T tCase, SwitchCase method){
			if (d_SwitchDict.ContainsKey (tCase))
				return false;
			else {
				d_SwitchDict.Add(tCase,method);
				return true;
			}
		}
		
		public bool removeCase(T tCase){
			if (d_SwitchDict.ContainsKey (tCase)) {
				return d_SwitchDict.Remove (tCase);
			} else
				return false;
		}
		
		public bool setDefault(SwitchCase method){
			d_Default = method;
			return true;
		}

		public void doDefault(){
			d_Default.Invoke ();
		}
		
		public void doSwitch(T tCase){
			if (d_SwitchDict.ContainsKey (tCase))
				d_SwitchDict [tCase].Invoke ();
			else if (d_Default != null)
				doDefault ();
			else
				return;
		}
		
	}
}

