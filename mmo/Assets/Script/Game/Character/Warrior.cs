using UnityEngine;
using System.Collections;

public class Warrior : PlayerChar {

    bool isUseSkills = false;
    bool isStan = false;

    public override bool UseSkill(int skillNumber, SkillBase skill)
    {
        
        if (skill.GetSp() > SP || isUseSkills)
        {
            return false;
        }

        // SPを消費させる
        SP -= skill.GetSp();

        // スキルを使用中に切り替える
        SkillFlag();

        // スキルのIDで処理分けを行う
        switch (skillNumber)
        {
            case 0:
                    this.Prov();
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
    /// 使うスキルを格納する変数
    /// </summary>
    System.Action useSkill;

    bool attackFlag = false;

    /// <summary>
    /// 攻撃アニメーションが終了した瞬間の処理
    /// </summary>
    public override void EndAttackAnimation()
    {
        base.EndAttackAnimation();
        // 攻撃しているフラグをオフにする
        attackFlag = false;
    }

    /// <summary>
    /// 通常攻撃をする
    /// </summary>
    protected override void NormalAttack()
    {
        if (!attackFlag)
        {
            // ダメージ計算式
            int damage = playerData.attack;
            // スキルごとのダメージを設定する
            SetAttack(damage, -1, 0.1f, 10);
            // 攻撃してるフラグをオンにする
            attackFlag = true;
            // 攻撃オブジェクトを使えるようにする
            EnablePlayerAttack();
            // アニメーションを再生する
            SetTrigger("NormalAttack1");
            // 攻撃を行う
            Attack();
        }
    }

    /// <summary>
    /// 挑発 (攻撃力上昇系)
    /// </summary>
    public void Prov()
    {
        SkillBase prov = SkillControl.GetSkill("挑発");

        // 再生させるアニメーションを指定する
        SetTrigger("Buffs");
        this.Hate = (int)prov.GetAttack();
        // スキルフラグをfalseにしておく
        this.EndSkillFlag();
    }

    /// <summary>
    /// バーサク (攻撃力上昇系)
    /// </summary>
    public void Berserk()
    {
        SkillBase berserk = SkillControl.GetSkill("バーサク");

        // 再生させるアニメーションを指定する
        SetTrigger("Buffs");
        // 攻撃力アップ
        StartCoroutine(StrBuff(berserk.GetAttack(), berserk.GetEffectTime()));
        // 防御力ダウン
        StartCoroutine(DefBuff(berserk.GetDefence(), berserk.GetEffectTime()));
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
        SetTrigger("Buffs");
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
        SetTrigger("BloodRage");
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
        SetTrigger("Buffs");
        // 物理防御力アップ
        StartCoroutine(DefBuff(defender.GetDefence(), defender.GetEffectTime()));
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
        SetTrigger("Buffs");
        // 物理防御力アップ
        StartCoroutine(DefBuff(lampart.GetDefence(), lampart.GetEffectTime()));
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
        SetTrigger("Sentinel");
        // 物理防御力アップ
        StartCoroutine(DefBuff(sentinel.GetDefence(), sentinel.GetEffectTime()));
        // 魔法防御力アップ
        StartCoroutine(MndBuff(sentinel.GetDefence(), sentinel.GetEffectTime()));
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
            //
            this.EnablePlayerAttack();
            // 再生させるアニメーションを指定する
            SetTrigger("FastBlade");
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

    protected override void EnablePlayerAttack(int attackNumber = -1)
    {
        base.EnablePlayerAttack(attackNumber);
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
