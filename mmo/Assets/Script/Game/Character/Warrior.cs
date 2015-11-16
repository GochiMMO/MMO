using UnityEngine;
using System.Collections;

public class Warrior : PlayerChar {
    public override bool UseSkill(int skillNumber, SkillBase skill)
    {
        return false;
        // throw new System.NotImplementedException();
    }

    private bool isSkills = false;

    /// <summary>
    /// バーサク (攻撃力上昇系)
    /// </summary>
    public void Berserk()
    {
        SkillBase berserk = SkillControl.GetSkill("バーサク");

        // 再生させるアニメーションを指定する
        anim.SetTrigger("Buff");
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

        // 再生させるアニメーションを指定する
        anim.SetTrigger("Buff");
        // 攻撃力アップ
        StartCoroutine(StrBuff(warcry.GetAttack(), warcry.GetEffectTime()));
    }

    /// <summary>
    /// ブラッドレイジ (攻撃力上昇系)
    /// </summary>
    public void BloodRage()
    {
        SkillBase bloodrage = SkillControl.GetSkill("ブラッドレイジ");

        if (!isSkills)
        {
            // 再生させるアニメーションを指定する
            anim.SetTrigger("Buff");
            // 攻撃力アップ
            StartCoroutine(StrBuff(bloodrage.GetAttack(), bloodrage.GetEffectTime()));
            // 防御力ダウン
            StartCoroutine(DefBuff(bloodrage.GetDefence(), bloodrage.GetEffectTime()));
        }
    
    }

    /// <summary>
    /// ディフェンダー（防御力上昇系）
    /// </summary>
    public void Defender()
    {
        SkillBase defender = SkillControl.GetSkill("ディフェンダー");

        if (!isSkills)
        {
            // 再生させるアニメーションを指定する
            anim.SetTrigger("Buff");
            // 物理防御力アップ
            StartCoroutine(DefBuff(defender.GetAttack(), defender.GetEffectTime()));
        }

    }

    /// <summary>
    /// ランパート（防御力上昇系）
    /// </summary>
    public void Lampart()
    {
        SkillBase lampart = SkillControl.GetSkill("ランパート");

        if (!isSkills)
        {
            // 再生させるアニメーションを指定する
            anim.SetTrigger("Buff");
            // 物理防御力アップ
            StartCoroutine(DefBuff(lampart.GetAttack(), lampart.GetEffectTime()));
        }
    
    }

    /// <summary>
    /// センチネル（防御力上昇系）
    /// </summary>
    public void Sentinel()
    {
        SkillBase sentinel = SkillControl.GetSkill("センチネル");

        if (!isSkills)
        {
            // 再生させるアニメーションを指定する
            anim.SetTrigger("Sentinel");
            // 物理防御力アップ
            StartCoroutine(DefBuff(sentinel.GetAttack(), sentinel.GetEffectTime()));
            // 魔法防御力アップ
            StartCoroutine(MndBuff(sentinel.GetAttack(), sentinel.GetEffectTime()));
        }

    }

    /// <summary>
    /// ファストブレード(攻撃系)
    /// </summary>
    public void Fastblade()
    {
        SkillBase fastblade = SkillControl.GetSkill("ファストブレード");
       
        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * fastblade.GetAttack());
       
        // スキルが使用中でなければ
        if (!isSkills)
        {
            // スキルごとのダメージを設定する
            SetAttack(damage);
            // 再生させるアニメーションを指定する
            anim.SetTrigger("FastBlade");
            // アタック中に切り替える
            Attack();
            // スキルを使用中に切り替える
            SkillFlag();

            Debug.Log("スキル使用");
        }

    }

    /// <summary>
    /// スキルを使用した時
    /// </summary>
    public void SkillFlag()
    {
        isSkills = true;
    }

    /// <summary>
    /// スキルを使い終わった時
    /// </summary>
    public void EndSkillFlag()
    {
        isSkills = false;
    }

    /// <summary>
    /// アタックに切り替え
    /// </summary>
    protected override void Attack()
    {
        base.Attack();
    } 

    protected override void Damage()
    {
        
    }

    // 今のところ使用しない
    protected override void Attacking()
    {

    }

    // 今のところ使用しない
    protected override void Dead()
    {

    }

    protected override void EndOfAttack()
    {
        base.DisablePlayerAttack();
        EndSkillFlag();
    }

    // 今のところ使用しない
    protected override void Normal()
    {

    }

    // 今のところ使用しない
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
