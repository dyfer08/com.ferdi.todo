using UnityEngine;
using System.Collections.Generic;

namespace Ferdi{
[System.Serializable]
public class TODO_Data : ScriptableObject {
	[System.Serializable]
	public class TODO {
		public string Task = "";
		public Type Type;
	}
	public List<TODO> TODOList = new List<TODO>();
	public enum Type {Grey, Green, Yellow, Red, Done};
}
}
