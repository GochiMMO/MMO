using UnityEngine;
using System.Collections;

public class SB {

    public SkillBase sb = new SkillBase();

    // Use this for initialization
    void Start()
    {
        sb.skillName = ("正拳突き");
        sb.lv = 1;
        sb.difficult = 1;
        sb.attack = 0.2f;
        sb.sp = 20;
        sb.cooltime = 1.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
