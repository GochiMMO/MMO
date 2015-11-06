using UnityEngine;
using System.Collections;

public class UseSkill : MonoBehaviour {
    public int skillID;
    public int skillPaletteNumber;    // スキルパレットの番号
    public static bool itemCoolTimeFlag = false;    // クールタイムかのフラグ
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // スキルの番号が押された時
        if (Input.GetKeyDown(KeyCode.Alpha1 + skillPaletteNumber))
        {
            // アイテムを使う処理
            Debug.Log("Use " + SkillControl.skills[skillID].GetName());
            // クールタイムを発生させる
            Debug.Log("スキルクールタイムセット");

        }
    }
}
