using UnityEngine;
using System.Collections;

public class Warrior : PlayerChar {

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
                    // 挑発
                    break;
            case 1:
                    this.Berserk();
                    break;
            case 2:
                    this.Warcry();
                    break;
            case 3:
                    this.BloodRage();
                    break;
            case 4:
                    this.Defender();
                    break;
            case 5:
                    this.Lampart();
                    break;
            case 6:
                    this.Sentinel();
                    break;
            case 7:
                    this.PowerSlash();
                    break;
            case 8:
                    this.Fastblade();
                    break;
            case 9:
                    this.DashSword();
                    break;
            case 10:
                    this.SonicBoom();
                    break;
            case 11:
                    this.SpiritsWithIn();
                    break;
            case 12:
                    this.DrainSword();
                    break;
            case 13:
                    this.SpinSlash();
                    break;
            case 14:
                    this.BoparuSword();
                    break;
            case 15:
                    this.ShieldBash();
                    break;

        }

        return true;
    }

    /// <summary>
    /// バーサク (攻撃力上昇系)
    /// </summary>
    public void Berserk()
    {
        SkillBase berserk = SkillControl.GetSkill("バーサク");

        Debug.Log("バーサク");
        // 再生させるアニメーションを指定する
        anim.SetTrigger("Buffs");
        // 攻撃力アップ
        StartCoroutine(StrBuff(berserk.GetAttack(), berserk.GetEffectTime()));
        // 防御力ダウン
        StartCoroutine(DefBuff(berserk.GetDefence(), berserk.GetEffectTime()));
        Debug.Log(playerData.attack);
        // スキルフラグをfalseにしておく
        this.EndSkillFlag();
    }

    /// <summary>
    /// ウォークライ (攻撃力上昇系)
    /// </summary>
    public void Warcry()
    {
        SkillBase warcry = SkillControl.GetSkill("ウォークライ");

        // 再生させるアニメーションを指定する
        anim.SetTrigger("Buffs");
        // 攻撃力アップ
        StartCoroutine(StrBuff(warcry.GetAttack(), warcry.GetEffectTime()));
        // スキルフラグをfalseにしておく
        this.EndSkillFlag();
    }

    /// <summary>
    /// ブラッドレイジ (攻撃力上昇系)
    /// </summary>
    public void BloodRage()
    {
        SkillBase bloodrage = SkillControl.GetSkill("ブラッドレイジ");

        // 再生させるアニメーションを指定する
        anim.SetTrigger("BloodRage");
        // 攻撃力アップ
        StartCoroutine(StrBuff(bloodrage.GetAttack(), bloodrage.GetEffectTime()));
        // 防御力ダウン
        StartCoroutine(DefBuff(bloodrage.GetDefence(), bloodrage.GetEffectTime()));
        // スキルフラグをfalseにしておく
        this.EndSkillFlag();
    }

    /// <summary>
    /// ディフェンダー（防御力上昇系）
    /// </summary>
    public void Defender()
    {
        SkillBase defender = SkillControl.GetSkill("ディフェンダー");

        // 再生させるアニメーションを指定する
        anim.SetTrigger("Buffs");
        // 物理防御力アップ
        StartCoroutine(DefBuff(defender.GetAttack(), defender.GetEffectTime()));
        // スキルフラグをfalseにしておく
        this.EndSkillFlag();
    }

    /// <summary>
    /// ランパート（防御力上昇系）
    /// </summary>
    public void Lampart()
    {
        SkillBase lampart = SkillControl.GetSkill("ランパート");

        // 再生させるアニメーションを指定する
        anim.SetTrigger("Buffs");
        // 物理防御力アップ
        StartCoroutine(DefBuff(lampart.GetAttack(), lampart.GetEffectTime()));
        // スキルフラグをfalseにしておく
        this.EndSkillFlag();
    }

    /// <summary>
    /// センチネル（防御力上昇系）
    /// </summary>
    public void Sentinel()
    {
        SkillBase sentinel = SkillControl.GetSkill("センチネル");

        // 再生させるアニメーションを指定する
        anim.SetTrigger("Sentinel");
        // 物理防御力アップ
        StartCoroutine(DefBuff(sentinel.GetAttack(), sentinel.GetEffectTime()));
        // 魔法防御力アップ
        StartCoroutine(MndBuff(sentinel.GetAttack(), sentinel.GetEffectTime()));
        // スキルフラグをfalseにしておく
        this.EndSkillFlag();
    }

    /// <summary>
    /// ファストブレード(攻撃系)
    /// </summary>
    public void Fastblade()
    {
        SkillBase fastblade = SkillControl.GetSkill("ファストブレード");
       
        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * fastblade.GetAttack());
      
            // スキルごとのダメージを設定する
            SetAttack(damage);
            // 再生させるアニメーションを指定する
            anim.SetTrigger("FastBlade");
            // アタック中に切り替える
            Attack();
    }

    /// <summary>
    /// パワースラッシュ(攻撃系)
    /// </summary>
    public void PowerSlash()
    {
        SkillBase powerslash = SkillControl.GetSkill("パワースラッシュ");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * powerslash.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("PowerSlash");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// ダッシュソード(攻撃系)
    /// </summary>
    public void DashSword()
    {
        SkillBase dashsword = SkillControl.GetSkill("ダッシュ斬り");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * dashsword.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("DashSword");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// ソニックブーム(攻撃系) ShockWave
    /// </summary>
    public void SonicBoom()
    {
        SkillBase sonicboom = SkillControl.GetSkill("ソニックブーム");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * sonicboom.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("ShockWave");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// スピリッツウィズイン(攻撃系)
    /// </summary>
    public void SpiritsWithIn()
    {
        SkillBase spiritswithin = SkillControl.GetSkill("スピリッツウィズイン");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * spiritswithin.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("SpiritsWithIn");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// ドレインソード(攻撃系)
    /// </summary>
    public void DrainSword()
    {
        SkillBase drainsword = SkillControl.GetSkill("ドレインソード");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * drainsword.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("DrainSword");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// スピンスラッシュ(攻撃系) 元RotationSword
    /// </summary>
    public void SpinSlash()
    {
        SkillBase spinslash = SkillControl.GetSkill("スピンスラッシュ");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * spinslash.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("RotationSword");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// ボーパルソード(攻撃系)
    /// </summary>
    public void BoparuSword()
    {
        SkillBase boparusword = SkillControl.GetSkill("ボーパルソード");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * boparusword.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // 再生させるアニメーションを指定する
        anim.SetTrigger("BoparuSword");
        // アタック中に切り替える
        Attack();
    }

    /// <summary>
    /// シールドバッシュ(攻撃系)
    /// </summary>
    public void ShieldBash()
    {
        SkillBase shiledbash = SkillControl.GetSkill("シールドバッシュ");

        // ダメージ計算式
        int damage = playerData.attack + (int)(playerData.attack * shiledbash.GetAttack());

        // スキルごとのダメージを設定する
        SetAttack(damage);
        // スタンフラグをtrueに変える
        this.SetStanFlag();
        // 再生させるアニメーションを指定する
        anim.SetTrigger("ShiledBash");
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
