using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_Archer : ScriptableObject
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
		
		public int HP;
		public int SP;
		public int Attack;
		public int Defense;
		public int MagicAttack;
		public int MagicDefense;
	}
}

