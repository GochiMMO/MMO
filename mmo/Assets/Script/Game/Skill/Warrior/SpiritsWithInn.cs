using UnityEngine;
using System.Collections;

public class SpiritsWithInn {

    public SkillBase swi = new SkillBase();

    // Use this for initialization
    void Start()
    {
        swi.skillName = ("スピリッツウィズイン");
        swi.lv = 1;
        swi.difficult = 3;
        swi.attack = 0.2f;
        swi.sp = 150;
        swi.cooltime = 10.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
