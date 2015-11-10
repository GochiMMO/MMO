using UnityEngine;
using System.Collections;

public class UseSkill : MonoBehaviour {
    public int skillID;
    public int skillPaletteNumber;    // スキルパレットの番号
    public bool skillCoolTimeFlag = false;    // クールタイムかのフラグ

    // Update is called once per frame
    void Update()
    {
        // スキルの番号が押された時
        if (Input.GetKeyDown(KeyCode.Alpha1 + skillPaletteNumber))
        {
            // スキルのクールタイム発生フラグが立っていなければ
            if (!skillCoolTimeFlag)
            {
                SetSkillIcon.GenerationSkillCoolTime(skillID);
                // スキルを使う処理
                Debug.Log("Use " + SkillControl.skills[skillID].GetName());
                // クールタイムを発生させる
                skillCoolTimeFlag = true;
                Debug.Log("スキルクールタイムセット");
            }
        }
    }
}
