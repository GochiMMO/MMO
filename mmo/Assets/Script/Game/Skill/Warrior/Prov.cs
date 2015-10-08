using UnityEngine;
using System.Collections;

//スキル:挑発
public class Prov{

    public SkillBase prov = new SkillBase();

	// Use this for initialization
	void Start ()
    {
        prov.skillName = ("挑発");
        prov.lv = 1;
        prov.difficult = 1;
        prov.attack = 0.0f;
        prov.sp = 20;
        prov.cooltime = 10.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
