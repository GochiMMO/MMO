using UnityEngine;
using System.Collections;

public class Warcry {

    public SkillBase wc = new SkillBase();

    // Use this for initialization
    void Start()
    {
        wc.skillName = ("ウォークライ");
        wc.lv = 0;
        wc.difficult = 2;
        wc.attack = 0;
        wc.sp = 50;
        wc.cooltime = 20.0f;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
