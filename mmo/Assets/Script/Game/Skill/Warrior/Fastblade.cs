using UnityEngine;
using System.Collections;

public class Fastblade {

    public SkillBase fb = new SkillBase();

    // Use this for initialization
    void Start()
    {
        fb.skillName = ("ファストブレード");
        fb.lv = 0;
        fb.difficult = 1;
        fb.attack = 0.2f;
        fb.sp = 30;
        fb.cooltime = 1.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
