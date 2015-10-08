using UnityEngine;
using System.Collections;

public class LSS : MonoBehaviour {

    public SkillBase lss = new SkillBase();

    // Use this for initialization
    void Start()
    {
        lss.skillName = ("羅刹衝");
        lss.lv = 1;
        lss.difficult = 1;
        lss.attack = 0.1f;
        lss.sp = 100;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
