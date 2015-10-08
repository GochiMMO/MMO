using UnityEngine;
using System.Collections;

public class ShockWave{

    public SkillBase sw = new SkillBase();

    // Use this for initialization
    void Start()
    {
        sw.skillName = ("衝撃波");
        sw.lv = 1;
        sw.difficult = 1;
        sw.attack = 0.1f;
        sw.sp = 50;
        sw.cooltime = 10.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
