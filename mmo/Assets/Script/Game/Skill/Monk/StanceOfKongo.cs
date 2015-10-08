using UnityEngine;
using System.Collections;

public class StanceOfKongo : MonoBehaviour {

    public SkillBase sok = new SkillBase();

    // Use this for initialization
    void Start()
    {
        sok.skillName = ("金剛の構え");
        sok.lv = 1;
        sok.difficult = 1;
        sok.attack = 0.0f;
        sok.sp = 50;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
