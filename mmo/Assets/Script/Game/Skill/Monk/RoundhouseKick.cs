using UnityEngine;
using System.Collections;

public class RoundhouseKick : MonoBehaviour {

    public SkillBase Rhk = new SkillBase();

    // Use this for initialization
    void Start()
    {
        Rhk.skillName = ("回し蹴り");
        Rhk.lv = 1;
        Rhk.difficult = 1;
        Rhk.attack = 0.1f;
        Rhk.sp = 20;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
