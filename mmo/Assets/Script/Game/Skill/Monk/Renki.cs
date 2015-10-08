using UnityEngine;
using System.Collections;

public class Renki : MonoBehaviour {

    public SkillBase ri = new SkillBase();

    // Use this for initialization
    void Start()
    {
        ri.skillName = ("練気");
        ri.lv = 1;
        ri.difficult = 2;
        ri.attack = 0.0f;
        ri.sp = 100;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
