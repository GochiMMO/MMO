using UnityEngine;
using System.Collections;

public class RotationSword : MonoBehaviour {

    public SkillBase rs = new SkillBase();

    // Use this for initialization
    void Start()
    {
        rs.skillName = ("回転斬り");
        rs.lv = 1;
        rs.difficult = 1;
        rs.attack = 0.2f;
        rs.sp = 100;
        rs.cooltime = 10.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
