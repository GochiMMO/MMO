using UnityEngine;
using System.Collections;

public class Lampert {

    public SkillBase lp = new SkillBase();

    // Use this for initialization
    void Start()
    {
        lp.skillName = ("ランパート");
        lp.lv = 0;
        lp.difficult = 2;
        lp.attack = 0;
        lp.sp = 50;
        lp.cooltime = 60.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
