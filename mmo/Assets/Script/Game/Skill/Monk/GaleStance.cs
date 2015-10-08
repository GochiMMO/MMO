using UnityEngine;
using System.Collections;

public class GaleStance : MonoBehaviour {

    public SkillBase rip = new SkillBase();

    // Use this for initialization
    void Start()
    {
        rip.skillName = ("疾風の構え");
        rip.lv = 1;
        rip.difficult = 1;
        rip.attack = 0.0f;
        rip.sp = 50;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
