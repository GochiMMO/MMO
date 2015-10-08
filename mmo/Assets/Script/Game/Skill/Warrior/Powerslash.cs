using UnityEngine;
using System.Collections;

public class Powerslash {

    public SkillBase ps = new SkillBase();

    // Use this for initialization
    void Start()
    {
        ps.skillName = ("パワーシュラッシュ");
        ps.lv = 0;
        ps.difficult = 1;
        ps.attack = 0.1f;
        ps.sp = 20;
        ps.cooltime = 1.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
