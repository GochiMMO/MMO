using UnityEngine;
using System.Collections;

public class Berserk {

    public SkillBase bk = new SkillBase();

    // Use this for initialization
    void Start()
    {
        bk.skillName = ("バーサク");
        bk.lv = 0;
        bk.difficult = 1;
        bk.attack = 0;
        bk.sp = 20;
        bk.cooltime = 30.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
