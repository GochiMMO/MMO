using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_Sahagin : ScriptableObject
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
		
		public string Name;
		public int HP;
		public int Attack;
		public int Defense;
		public int MagicAttack;
		public int MagicDefense;
		public float MoveSpeed;
		public float TrackingSpeed;
		public int ActionInterval;
		public int FieldOfView;
		public float ViewDistance;
		public float ActionDistance;
	}
}

