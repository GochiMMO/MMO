using UnityEngine;
using System.Collections;

public class Defender {

    public SkillBase df = new SkillBase();

    // Use this for initialization
    void Start()
    {
        df.skillName = ("ディフェンダー");
        df.lv = 0;
        df.difficult = 1;
        df.attack = 0;
        df.sp = 20;
        df.cooltime = 30.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
