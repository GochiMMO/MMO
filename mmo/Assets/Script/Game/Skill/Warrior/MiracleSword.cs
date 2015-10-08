using UnityEngine;
using System.Collections;

public class MiracleSword {

    public SkillBase ms = new SkillBase();

    // Use this for initialization
    void Start()
    {
        ms.skillName = ("ミラクルソード");
        ms.lv = 1;
        ms.difficult = 2;
        ms.attack = 0.2f;
        ms.sp = 100;
        ms.cooltime = 10.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
