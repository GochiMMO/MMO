using UnityEngine;
using System.Collections;

public class StanceGlen : MonoBehaviour {

    public SkillBase sg = new SkillBase();

    // Use this for initialization
    void Start()
    {
        sg.skillName = ("紅蓮の構え");
        sg.lv = 1;
        sg.difficult = 1;
        sg.attack = 0.0f;
        sg.sp = 50;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
