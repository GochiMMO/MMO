using UnityEngine;
using System.Collections;

public class ReleaseinPower : MonoBehaviour {

    public SkillBase rip = new SkillBase();

    // Use this for initialization
    void Start()
    {
        rip.skillName = ("発勁");
        rip.lv = 1;
        rip.difficult = 2;
        rip.attack = 0.0f;
        rip.sp = 100;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
