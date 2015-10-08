using UnityEngine;
using System.Collections;

public class Boparublade {

    public SkillBase bb = new SkillBase();

    // Use this for initialization
    void Start()
    {
        bb.skillName = ("ボーパルブレード");
        bb.lv = 1;
        bb.difficult = 3;
        bb.attack = 0.3f;
        bb.sp = 200;
        bb.cooltime = 5.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
