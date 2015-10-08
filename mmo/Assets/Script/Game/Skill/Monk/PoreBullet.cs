using UnityEngine;
using System.Collections;

public class PoreBullet : MonoBehaviour {

    public SkillBase pob = new SkillBase();

    // Use this for initialization
    void Start()
    {
        pob.skillName = ("気孔弾");
        pob.lv = 1;
        pob.difficult = 2;
        pob.attack = 0.1f;
        pob.sp = 200;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
