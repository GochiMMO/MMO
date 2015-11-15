using UnityEngine;
using System.Collections;

public class Warrior : PlayerChar {
    public override bool UseSkill(int skillNumber, SkillBase skill)
    {
        return false;
        // throw new System.NotImplementedException();
    }

    /// <summary>
    /// バーサク (攻撃力上昇系)
    /// </summary>
    public void Berserk()
    {
        SkillBase berserk = SkillControl.GetSkill("バーサク");
        
        // 攻撃力アップ
        StartCoroutine(StrBuff(berserk.GetAttack(), berserk.GetEffectTime()));
        // 防御力ダウン
        StartCoroutine(DefBuff(berserk.GetDefence(), berserk.GetEffectTime()));
    }

    /// <summary>
    /// ウォークライ (攻撃力上昇系)
    /// </summary>
    public void Warcry()
    {
        SkillBase warcry = SkillControl.GetSkill("ウォークライ");
        // 攻撃力アップ
        StartCoroutine(StrBuff(warcry.GetAttack(), warcry.GetEffectTime()));
    }

    /// <summary>
    /// ブラッドレイジ (攻撃力上昇系)
    /// </summary>
    public void BloodRage()
    {
        SkillBase bloodrage = SkillControl.GetSkill("ブラッドレイジ");
        // 攻撃力アップ
        StartCoroutine(StrBuff(bloodrage.GetAttack(), bloodrage.GetEffectTime()));
        // 防御力ダウン
        StartCoroutine(DefBuff(bloodrage.GetDefence(), bloodrage.GetEffectTime()));
    }

    /// <summary>
    /// ディフェンダー（防御力上昇系）
    /// </summary>
    public void Defender()
    {
        SkillBase defender = SkillControl.GetSkill("ディフェンダー");
        // 物理防御力アップ
        StartCoroutine(DefBuff(defender.GetAttack(), defender.GetEffectTime()));
    }

    /// <summary>
    /// ランパート（防御力上昇系）
    /// </summary>
    public void Lampart()
    {
        SkillBase lampart = SkillControl.GetSkill("ランパート");
        // 物理防御力アップ
        StartCoroutine(DefBuff(lampart.GetAttack(), lampart.GetEffectTime()));
    }

    /// <summary>
    /// センチネル（防御力上昇系）
    /// </summary>
    public void Sentinel()
    {
        SkillBase sentinel = SkillControl.GetSkill("センチネル");

        // 物理防御力アップ
        StartCoroutine(DefBuff(sentinel.GetAttack(), sentinel.GetEffectTime()));
        // 魔法防御力アップ
        StartCoroutine(MndBuff(sentinel.GetAttack(), sentinel.GetEffectTime()));
    }

    protected override void Attack()
    {
        base.Attack();
    } 

    protected override void Damage()
    {
        
    }

    protected override void Attacking()
    {

    }

    protected override void Dead()
    {

    }

    protected override void EndOfAttack()
    {

    }

    protected override void Normal()
    {

    }

    protected override void Revive()
    {

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void Start()
    {
        base.Start();
    }
}
