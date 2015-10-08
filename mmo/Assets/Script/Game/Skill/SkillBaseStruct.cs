using UnityEngine;
using System.Collections;

/// <summary>
/// (スキル名. スキルLv. 重み. 攻撃力. 消費SP)
/// </summary>
public struct SkillBase {
    public string skillName;
    public int lv;
    public int difficult;
    public float attack;
    public int sp;
    public float cooltime;
}
