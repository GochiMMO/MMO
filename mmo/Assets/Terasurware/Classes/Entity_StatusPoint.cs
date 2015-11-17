using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_StatusPoint : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public float HP;
		public float SP;
		public float Attack;
		public float MagicAttack;
		public float Defense;
		public float MagicDefense;
	}
}

