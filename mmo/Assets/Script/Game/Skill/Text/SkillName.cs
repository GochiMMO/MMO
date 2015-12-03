using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillName : MonoBehaviour {

    [SerializeField, Tooltip("スキルID")]
    int id;

    void View()
    {
        SkillBase skill = SkillControl.skills[id];
        this.transform.GetComponent<Text>().text = skill.GetName();
    }

	void Start () {
        // 表示するスキルのデータクラスを取得する
        this.View();
	}
}
