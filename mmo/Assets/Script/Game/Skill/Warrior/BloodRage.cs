using UnityEngine;
using System.Collections;

public class BloodRage {

    public SkillBase br = new SkillBase();

    // Use this for initialization
    void Start()
    {
        br.skillName = ("ブラッドレイジ");
        br.lv = 0;
        br.difficult = 3;
        br.attack = 0;
        br.sp = 100;
        br.cooltime = 120.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
