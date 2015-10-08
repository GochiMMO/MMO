using UnityEngine;
using System.Collections;

public class SLegs : MonoBehaviour {

    public SkillBase sl = new SkillBase();

    // Use this for initialization
    void Start()
    {
        sl.skillName = ("双竜脚");
        sl.lv = 1;
        sl.difficult = 2;
        sl.attack = 0.2f;
        sl.sp = 100;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
