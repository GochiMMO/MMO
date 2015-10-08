using UnityEngine;
using System.Collections;

public class Sentinel {

    public SkillBase st = new SkillBase();

    // Use this for initialization
    void Start()
    {
        st.skillName = ("センチネル");
        st.lv = 0;
        st.difficult = 3;
        st.attack = 0;
        st.sp = 100;
        st.cooltime = 120.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
