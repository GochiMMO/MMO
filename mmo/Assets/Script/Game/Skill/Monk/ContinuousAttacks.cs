using UnityEngine;
using System.Collections;

public class ContinuousAttacks : MonoBehaviour {

    public SkillBase ca = new SkillBase();

    // Use this for initialization
    void Start()
    {
        ca.skillName = ("連撃");
        ca.lv = 1;
        ca.difficult = 1;
        ca.attack = 0.1f;
        ca.sp = 20;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
