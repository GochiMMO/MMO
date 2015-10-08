using UnityEngine;
using System.Collections;

public class MSF : MonoBehaviour {

    public SkillBase msf = new SkillBase();

    // Use this for initialization
    void Start()
    {
        msf.skillName = ("夢想阿修羅拳");
        msf.lv = 1;
        msf.difficult = 3;
        msf.attack = 0.2f;
        msf.sp = 400;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
