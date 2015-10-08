using UnityEngine;
using System.Collections;

public class EvilImmediatelyZan {

    public SkillBase eiz = new SkillBase();

    // Use this for initialization
    void Start()
    {
        eiz.skillName = ("悪即斬");
        eiz.lv = 1;
        eiz.difficult = 1;
        eiz.attack = 0.1f;
        eiz.sp = 50;
        eiz.cooltime = 5.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
