using UnityEngine;
using System.Collections;

public class UseSkill : MonoBehaviour {
    public int skillID;
    public int skillPaletteNumber;    // スキルパレットの番号
    public bool skillCoolTimeFlag = false;    // クールタイムかのフラグ

    /// <summary>
    /// プレイヤーのスクリプトの参照
    /// </summary>
    public static PlayerChar playerChar;

    // Update is called once per frame
    void Update()
    {
        // プレイヤーのスクリプトが未割当の場合
        if (!playerChar)
        {
            // プレイヤーを探し出す
            playerChar = StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(PhotonNetwork.player.ID, "Player").GetComponent<PlayerChar>();
        }
        // スキルの番号が押された時
        if (Input.GetKeyDown(KeyCode.Alpha1 + skillPaletteNumber))
        {
            // スキルのクールタイム発生フラグが立っていなければ
            if (!skillCoolTimeFlag)
            {
                // スキルを使用する
                if (playerChar.UseSkill(skillID, SkillControl.GetSkill(skillID)))
                {
                    // クールタイムの発生
                    SetSkillIcon.GenerationSkillCoolTime(skillID);
                    skillCoolTimeFlag = true;
                    Debug.Log("スキルクールタイムセット");
                }
            }
        }
    }
}
