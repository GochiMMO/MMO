using UnityEngine;
using System.Collections;

public class Monk : PlayerChar {
    bool isUseSkills = false;
    bool isStan = false;

    public override bool UseSkill(int skillNumber, SkillBase skill)
    {
        // デバッグ用
        Debug.Log("Call UseSkil");

        if (skill.GetSp() > SP || isUseSkills)
        {
            Debug.Log("Skill Cancel");
            // スキル使用させない
            return false;
        }

        Debug.Log("Spend SP");
        // SPを消費させる
        SP -= skill.GetSp();

        // スキルを使用中に切り替える
        SkillFlag();

        // スキルのIDで処理分けを行う
        switch (skillNumber)
        {
            case 0:
                this.StanceKongou();
                break;
            case 1:
                this.StanceGale();
                break;
            case 2:
                this.StanceGlen();
                break;
            case 3:
                this.Renki();
                break;
            case 4:
                this.Hakei();
                break;
            case 5:
                this.Rengeki();
                break;
            case 6:
                this.Seiken();
                break;
            case 7:
                this.RotationKick();
                break;
            case 8:
                this.Rasetu();
                break;
            case 9:
                this.Sousyoda();
                break;
            case 10:
                this.Souryu();
                break;
            case 11:
                this.Kikou();
                break;
            case 12:
                this.Renkan();
                break;
            case 13:
                this.Musou();
                break;

        }

        return true;
    }

    /// <summary>
    /// 金剛の構え (防御上昇系)
    /// </summary>
    public void StanceKongou()
    {
        SkillBase stance_K = SkillControl.GetSkill("金剛の構え");

        Debug.Log("金剛の構え");
        // 再生させるアニメーションを指定する
        anim.SetTrigger("Buffs");
        // 防御力アップ
        StartCoroutine(StrBuff(stance_K.GetDefence(), stance_K.GetEffectTime()));
        Debug.Log(playerData.defense);
        // スキルフラグをfalseにしておく
        this.EndSkillFlag();
    }

    /// <summary>
    ///  (移動速度上昇系)
    /// </summary>
    public void StanceGale()
    {
        SkillBase stance_G = SkillControl.GetSkill("疾風の構え");

        // 再生させるアニメーションを指定する
        anim.SetTrigger("Buffs");
        // 移動速度上昇
        
        // スキルフラグをfalseにしておく
        this.EndSkillFlag();
    }

    /// <summary>
    /// 紅蓮の構え (攻撃力上昇系)
    /// </summary>
    public void StanceGlen()
    {
        SkillBase stance_G = SkillControl.GetSkill("紅蓮の構え");

        // 再生させるアニメーションを指定する
        anim.SetTrigger("Buffs");
        // 攻撃力アップ
        StartCoroutine(StrBuff(stance_G.GetAttack(), stance_G.GetEffectTime()));
        // スキルフラグをfalseにしておく
        this.EndSkillFlag();
    }

    /// <summary>
    /// 練気（回復系）
    /// </summary>
    public void Renki()
    {
        SkillBase renki = SkillControl.GetSkill("練気");
   
        // 再生させるアニメーションを指定する
        anim.SetTrigger("Buffs");
        // Hp回復
        Recover((int)((float)playerData.MaxHP * (renki.GetBonus() + renki.GetDefence())));
        // スキルフラグをfalseにしておく
        this.EndSkillFlag();
    }

    /// <summary>
    /// 発勁（回復系）
    /// </summary>
    public void Hakei()
    {
        SkillBase hakei = SkillControl.GetSkill("発勁");

        // 再生させるアニメーションを指定する
        anim.SetTrigger("Buffs");
        // Sp回復
        SP += (int)((float)playerData.MaxSP * (hakei.GetBonus() + hakei.GetDefence()));
        // スキルフラグをfalseにしておく
        this.EndSkillFlag();
    }

    /// <summary>
    /// 連撃（攻撃系）
    /// </summary>
    public void Rengeki()
    {
        SkillBase rengeki = SkillControl.GetSkill("連撃");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * rengeki.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("FastBlade");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// 正拳突き(攻撃系)
    /// </summary>
    public void Seiken()
    {
        SkillBase seiken = SkillControl.GetSkill("正拳突き");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * seiken.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("FastBlade");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// 回し蹴り(攻撃系)
    /// </summary>
    public void RotationKick()
    {
        SkillBase rotation = SkillControl.GetSkill("回し蹴り");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * rotation.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("PowerSlash");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// 羅刹衝(攻撃系)
    /// </summary>
    public void Rasetu()
    {
        SkillBase rasetu = SkillControl.GetSkill("羅刹衝");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * rasetu.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("DashSword");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// 双掌打 (攻撃系)
    /// </summary>
    public void Sousyoda()
    {
        SkillBase sousyoda = SkillControl.GetSkill("双掌打");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * sousyoda.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("ShockWave");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// 双竜脚 (攻撃系)
    /// </summary>
    public void Souryu()
    {
        SkillBase souryu = SkillControl.GetSkill("双竜脚");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * souryu.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("SpiritsWithIn");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// 気孔弾 (攻撃系)
    /// </summary>
    public void Kikou()
    {
        SkillBase kikou = SkillControl.GetSkill("気孔弾");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * kikou.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("DrainSword");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// 連環六合圏 (攻撃系)
    /// </summary>
    public void Renkan()
    {
        SkillBase renkan = SkillControl.GetSkill("連環六合圏");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * renkan.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("RotationSword");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// 夢想阿修羅拳 (攻撃系)
    /// </summary>
    public void Musou()
    {
        SkillBase musou = SkillControl.GetSkill("夢想阿修羅拳");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * musou.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("BoparuSword");
        // アタック中に切り替える
        Attack();
    }


    /// <summary>
    /// スキルを使用した時
    /// </summary>
    public void SkillFlag() { this.isUseSkills = true; }

    /// <summary>
    /// スキルを使い終わった時
    /// </summary>
    public void EndSkillFlag() { this.isUseSkills = false; }

    /// <summary>
    /// スタンフラグの取得
    /// </summary>
    /// <returns></returns>
    public bool GetStanFlag() { return this.isStan; }

    /// <summary>
    /// スタンフラグをtrueにする
    /// </summary>
    protected void SetStanFlag() { this.isStan = true; }

    /// <summary>
    /// スタンフラグをfalseにする
    /// </summary>
    protected void EndStanFlag() { this.isStan = false; }

    /// <summary>
    /// アタックに切り替え
    /// </summary>
    protected override void Attack()
    {
        // Baseクラスに書いてある処理をさせる
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

    /// <summary>
    /// 詠唱が終了したモーションが終了したら呼び出される
    /// </summary>
    public override void EndAttackAnimation()
    {
        // 基礎クラスの処理を行う
        base.EndAttackAnimation();
    }

    protected override void EndOfAttack()
    {
        // Baseクラスに書いてある処理をさせる
        base.EndOfAttack();
        // スキルフラグをfalseにしておく
        this.EndSkillFlag();
        // スタンフラグをfalseにしておく
        this.EndStanFlag();
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
