using UnityEngine;
using System.Collections;

public class DLA : MonoBehaviour {

    public SkillBase dla = new SkillBase();

    // Use this for initialization
    void Start()
    {
        dla.skillName = ("連環六合圏");
        dla.lv = 2;
        dla.difficult = 2;
        dla.attack = 0.2f;
        dla.sp = 300;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
