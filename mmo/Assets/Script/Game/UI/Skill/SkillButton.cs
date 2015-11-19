using UnityEngine;
using System.Collections;

public class SkillButton : MonoBehaviour {

    /// <summary>
    /// スキルレベルを振る関数
    /// </summary>
    /// <param name="skillID">スキルのID</param>
    public void PushPlusButton(int skillID)
    {
        // 取得しようとしているスキルの参照を取得する
        SkillBase skill = SkillControl.GetSkill(skillID);
        // スキルが上限に達していたら
        if (skill.GetLv() == 10)
        {
            // 処理を行わない
            return;
        }
        // 自分のスキルポイントが取得しようとしているスキルの必要ポイント以上あるならば
        if (PlayerStatus.playerData.skillPoint >= skill.GetPoint())
        {
            // スキルポイントを減らす
            PlayerStatus.playerData.skillPoint -= skill.GetPoint();
            // スキルレベルを上げる
            PlayerStatus.playerData.skillLevel[skillID]++;
            skill.level++;
        }
    }
}
