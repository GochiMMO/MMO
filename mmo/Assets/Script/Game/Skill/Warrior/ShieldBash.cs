using UnityEngine;
using System.Collections;

public class ShieldBash {

    public SkillBase sb = new SkillBase();

    // Use this for initialization
    void Start()
    {
        sb.skillName = ("シールドバッシュ");
        sb.lv = 1;
        sb.difficult = 1;
        sb.attack = 0.1f;
        sb.sp = 50;
        sb.cooltime = 10.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
