using UnityEngine;
using System.Collections;

public class SSD {

    public SkillBase ssd = new SkillBase();

    // Use this for initialization
    void Start()
    {
        ssd.skillName = ("双掌打");
        ssd.lv = 1;
        ssd.difficult = 2;
        ssd.attack = 0.2f;
        ssd.sp = 100;
        ssd.cooltime = 1.0f; 
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
