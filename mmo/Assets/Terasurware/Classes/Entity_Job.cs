using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_Job : ScriptableObject
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
		
		public int id;
		public string name;
		public int lv;
		public int difficult;
		public float attack;
		public float defens;
		public int sp;
		public float cooltime;
		public float casttime;
		public float effect;
		public int point;
		public float bonus;
		public int type;
	}
}

