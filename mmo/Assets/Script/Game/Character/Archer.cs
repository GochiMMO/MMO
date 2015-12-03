using UnityEngine;
using System.Collections;

public class Archer : PlayerChar {
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
        // 攻撃する
        useSkill();
        // 攻撃をnullに設定する
        useSkill = () => { };
    }

    /// <summary>
    /// 通常攻撃をする
    /// </summary>
    protected override void NormalAttack()
    {
        if (!attackFlag)
        {
            // 通常攻撃を登録する
            useSkill = () => NormalAttack1();
            // 攻撃してるフラグをオンにする
            attackFlag = true;
            // アニメーションを再生する
            SetTrigger("NormalAttack1");
        }
    }

    /// <summary>
    /// 通常攻撃を飛ばす処理
    /// </summary>
    void NormalAttack1()
    {
        // 通常攻撃のオブジェクトをインスタンス化する
        GameObject normalAttackObj = PhotonNetwork.Instantiate("Arrow/NormalAttack", transform.position, Quaternion.identity, 0);
        // 飛ばす方向を格納する
        normalAttackObj.GetComponent<FireShot>().SetShotVec(transform.forward);
        // 攻撃のプロパティを設定する
        normalAttackObj.GetComponent<PlayerAttack>().SetProperties(playerData.attack, 0.1f, PlayerAttack.AttackKind.PHYSICS, this, 10);
    }

    /// <summary>
    /// スキルを実際に使った処理
    /// </summary>
    /// <param name="skillNumber">スキルの番号</param>
    /// <param name="skill">スキルのクラスの参照</param>
    /// <returns>スキルを使用できるかどうかのフラグ</returns>
    public override bool UseSkill(int skillNumber, SkillBase skill)
    {
        // 消費SPを超えているか、他の魔法を発動中ならば
        if (skill.GetSp() > SP && !attackFlag)
        {
            // スキルを発動できない
            return false;
        }
        // SPを消費させる
        SP -= skill.GetSp();
        // ステータスを攻撃中に変更する
        Attack();
        
        // スキルのIDで処理分けを行う
        switch (skillNumber)
        {
            // 0番目のスキル(ホークアイ)
            case 0:
                // メソッドの登録
                this.useSkill = () => HawkEye();
                SetTrigger("HawkEye");
                break;
            // 1番目のスキル(イーグルアイ)
            case 1:
                // メソッドの登録
                this.useSkill = () => EagleEye();
                SetTrigger("HawkEye");
                break;
            // 2番目のスキル(スピンキック)
            case 2:
                // スピンキックを行う
                this.SpinKick();
                break;
            // 3番目のスキル(ヘヴィショット)
            case 3:
                // メソッドの登録
                this.useSkill = () => HeavyShot();
                SetTrigger("HeavyShot");
                break;
            // 4番目のスキル(ピアシングアロー)
            case 4:
                this.useSkill = () => this.PiercingArrow();
                SetTrigger("HeavyShot");
                break;
            // 5番目のスキル(ベノムバイト)
            case 5:
                this.useSkill = () => VenomBite();
                SetTrigger("HeavyShot");
                break;
            // 6番目のスキル(サイドワインダー)
            case 6:
                this.useSkill = () => SideWinder();
                SetTrigger("HeavyShot");
                break;
            // 7番目のスキル(マジックアロー)
            case 7:
                this.useSkill = () => MagicArrow();
                SetTrigger("NormalAttack1");
                break;
                // 8番目のスキル(レイン・オブ・デス)
            case 8:
                this.useSkill = () => RainOfDeath();
                SetTrigger("RainOfDeath");
                break;
                // 9番目のスキル(ムーンサルト)
            case 9:
                this.MoonSault();
                break;
                // 10番目のスキル(スナイパーショット)
            case 10:
                this.useSkill = () => this.SniperShot();
                SetTrigger("SnaiperShot");
                break;
        }
        // 攻撃したことにする
        Attack();
        // 攻撃したフラグを立てる
        attackFlag = true;
        // 魔法を発動できるのでtrueを返す
        return true;
    }

    /// <summary>
    /// スキルID = 3,「ヘヴィショット」
    /// </summary>
    void HeavyShot()
    {
        // ヘヴィショットのスキルを持ってくる
        SkillBase heavyShot = SkillControl.GetSkill(3);
        // ヘヴィショットを放つ
        GameObject heavyShotObj = PhotonNetwork.Instantiate("Arrow/HeavyShot", gameObject.transform.position, Quaternion.identity, 0);
        // ヘヴィショットの攻撃スクリプトを取得する
        PlayerAttack heavyShotAttack = heavyShotObj.GetComponent<PlayerAttack>();
        // 基礎攻撃力を取得する
        int attack = playerData.attack;
        // 攻撃力に掛ける倍率を計算する
        float attackRate = heavyShot.attack + heavyShot.GetBonus();
        // 攻撃力を計算する
        attack = (int)((attack + attack * attackRate) * strBuff);
        // ヘヴィショットの攻撃力等を設定する
        heavyShotAttack.SetProperties(attack, 0.1f, PlayerAttack.AttackKind.PHYSICS, this);
        // 攻撃を放つ
        heavyShotObj.GetComponent<FireShot>().SetShotVec(transform.forward);
    }

    /// <summary>
    /// スキルID = 4,「ピアシングアロー」
    /// </summary>
    private void PiercingArrow()
    {
        // ピアシングアローのスキルを持ってくる
        SkillBase piercingArrow = SkillControl.GetSkill(4);
        // ピアシングアローを放つ
        GameObject piercingArrowObj = PhotonNetwork.Instantiate("Arrow/PiercingArrow", gameObject.transform.position, Quaternion.identity, 0);
        // ピアシングアローの攻撃スクリプトを取得する
        PlayerAttack piercingArrowAttack = piercingArrowObj.GetComponent<PlayerAttack>();
        // 基礎攻撃力を取得する
        int attack = playerData.attack;
        // 攻撃力に掛ける倍率を計算する
        float attackRate = piercingArrow.attack + piercingArrow.GetBonus();
        // 攻撃力を計算する
        attack = (int)((attack + attack * attackRate) * strBuff);
        // ピアシングアローの攻撃力等を設定する
        piercingArrowAttack.SetProperties(attack, 0.1f, PlayerAttack.AttackKind.PHYSICS, this);
        // 攻撃を放つ
        piercingArrowObj.GetComponent<FireShot>().SetShotVec(transform.forward);
    }

    /// <summary>
    /// スキルID = 6, 「サイドワインダー」
    /// </summary>
    private void SideWinder()
    {
        // サイドワインダーのスキルを持ってくる
        SkillBase sideWinder = SkillControl.GetSkill(6);
        // サイドワインダーを放つ
        GameObject sideWinderObj = PhotonNetwork.Instantiate("Arrow/SideWinder", gameObject.transform.position, Quaternion.identity, 0);
        // サイドワインダーの攻撃スクリプトを取得する
        PlayerAttack sideWinderAttack = sideWinderObj.GetComponent<PlayerAttack>();
        // 基礎攻撃力を取得する
        int attack = playerData.attack;
        // 攻撃力に掛ける倍率を計算する
        float attackRate = sideWinder.attack + sideWinder.GetBonus();
        // 攻撃力を計算する
        attack = (int)((attack + attack * attackRate) * strBuff);
        // サイドワインダーの攻撃力等を設定する
        sideWinderAttack.SetProperties(attack, 0.1f, PlayerAttack.AttackKind.PHYSICS, this);
        // 攻撃を放つ
        sideWinderObj.GetComponent<SideWainder>().SetShotVec(transform.forward);
    }

    /// <summary>
    /// スキルID = 7, 「マジックアロー」
    /// </summary>
    private void MagicArrow()
    {
        // マジックアローのスキルを持ってくる
        SkillBase magicArrow = SkillControl.GetSkill(7);
        // マジックアローを放つ
        GameObject magicArrowObj = PhotonNetwork.Instantiate("Arrow/MagicArrow", gameObject.transform.position, Quaternion.identity, 0);
        // マジックアローの攻撃スクリプトを取得する
        PlayerAttack magicArrowAttack = magicArrowObj.GetComponent<PlayerAttack>();
        // 基礎攻撃力を取得する
        int attack = playerData.magicAttack;
        // 攻撃力に掛ける倍率を計算する
        float attackRate = magicArrow.attack + magicArrow.GetBonus();
        // 攻撃力を計算する
        attack = (int)((attack + attack * attackRate) * intBuff);
        // マジックアローの攻撃力等を設定する
        magicArrowAttack.SetProperties(attack, 0.1f, PlayerAttack.AttackKind.MAGIC, this);
        // 攻撃を放つ
        magicArrowObj.GetComponent<FireShot>().SetShotVec(transform.forward);
    }

    /// <summary>
    /// スキルID = 10, 「スナイパーショット」
    /// </summary>
    private void SniperShot()
    {
        // スナイパーショットのスキルを持ってくる
        SkillBase sniperShot = SkillControl.GetSkill(10);
        // スナイパーショットを放つ
        GameObject sniperShotObj = PhotonNetwork.Instantiate("Arrow/SniperShot", gameObject.transform.position, Quaternion.identity, 0);
        // スナイパーショットの攻撃スクリプトを取得する
        PlayerAttack sniperShotAttack = sniperShotObj.GetComponent<PlayerAttack>();
        // 基礎攻撃力を取得する
        int attack = playerData.attack;
        // 攻撃力に掛ける倍率を計算する
        float attackRate = sniperShot.attack + sniperShot.GetBonus();
        // 攻撃力を計算する
        attack = (int)((attack + attack * attackRate) * strBuff);
        // スナイパーショットの攻撃力等を設定する
        sniperShotAttack.SetProperties(attack, 0.1f, PlayerAttack.AttackKind.PHYSICS, this);
        // 攻撃を放つ
        sniperShotObj.GetComponent<FireShot>().SetShotVec(transform.forward);
    }

    /// <summary>
    /// スキルID = 8, 「レインオブデス」
    /// </summary>
    private void RainOfDeath()
    {
        // レインオブデスのスキルを持ってくる
        SkillBase rainOfDeath = SkillControl.GetSkill(8);
        // レインオブデスを放つ
        GameObject rainOfDeathObj = PhotonNetwork.Instantiate("Arrow/RainOfDeath", gameObject.transform.position, Quaternion.identity, 0);
        // レインオブデスの攻撃スクリプトを取得する
        PlayerAttack rainOfDeathAttack = rainOfDeathObj.GetComponent<PlayerAttack>();
        // 基礎攻撃力を取得する
        int attack = playerData.attack;
        // 攻撃力に掛ける倍率を計算する
        float attackRate = rainOfDeath.attack + rainOfDeath.GetBonus();
        // 攻撃力を計算する
        attack = (int)((attack + attack * attackRate) * strBuff);
        // レインオブデスの攻撃力等を設定する
        rainOfDeathAttack.SetProperties(attack, 0.1f, PlayerAttack.AttackKind.PHYSICS, this);
    }

    /// <summary>
    /// スキルID = 5, 「ヴェノムバイト」
    /// </summary>
    private void VenomBite()
    {
        // ヴェノムバイトのスキルを持ってくる
        SkillBase venomBite = SkillControl.GetSkill(5);
        // ヴェノムバイトを放つ
        GameObject venomBiteObj = PhotonNetwork.Instantiate("Arrow/SniperShot", gameObject.transform.position, Quaternion.identity, 0);
        // ヴェノムバイトの攻撃スクリプトを取得する
        PlayerAttack venomBiteAttack = venomBiteObj.GetComponent<PlayerAttack>();
        // 基礎攻撃力を取得する
        int attack = playerData.attack;
        // 攻撃力に掛ける倍率を計算する
        float attackRate = venomBite.attack + venomBite.GetBonus();
        // 攻撃力を計算する
        attack = (int)((attack + attack * attackRate) * strBuff);
        // ヴェノムバイトの攻撃力等を設定する
        venomBiteAttack.SetProperties(attack, 0.1f, PlayerAttack.AttackKind.PHYSICS, this);
        // 攻撃を放つ
        venomBiteObj.GetComponent<FireShot>().SetShotVec(transform.forward);
    }

    /// <summary>
    /// スキルID = 0, 「ホークアイ」
    /// </summary>
    private void HawkEye()
    {
        // ホークアイのスキルを持ってくる
        SkillBase hawkEye = SkillControl.GetSkill(0);
        // エフェクトを表示する
        PhotonNetwork.Instantiate("Arrow/HawkEye", gameObject.transform.position + Vector3.up * 1f, Quaternion.identity, 0);
        // ホークアイの効果値を取得する
        float effectValue = hawkEye.attack + hawkEye.bonus * hawkEye.level;
        // 攻撃力にバフを掛ける
        StartCoroutine(StrBuff(effectValue, hawkEye.GetEffectTime()));
    }

    /// <summary>
    /// スキルID = 1, 「イーグルアイ」
    /// </summary>
    private void EagleEye()
    {
        // イーグルアイのスキルを持ってくる
        SkillBase eagleEye = SkillControl.GetSkill(1);
        // エフェクトを表示する
        PhotonNetwork.Instantiate("Arrow/HawkEye", gameObject.transform.position + Vector3.up * 1f, Quaternion.identity, 0);
        // ホークアイの効果値を取得する
        float effectValue = eagleEye.attack + eagleEye.bonus * eagleEye.level;
        // 攻撃力にバフを掛ける
        StartCoroutine(StrBuff(effectValue, eagleEye.GetEffectTime()));
    }

    /// <summary>
    /// スキルID = 9, 「ムーンサルト」
    /// </summary>
    private void MoonSault()
    {
        // アニメーションを再生する
        SetTrigger("SummerSalt");
        // ムーンサルトのスキルを持ってくる
        SkillBase moonSault = SkillControl.GetSkill(9);
        // 攻撃力を設定する
        SetAttack(playerData.attack + (int)(playerData.attack * moonSault.GetAttack()));
        // 攻撃するコライダーをオンにする
        EnablePlayerAttack();
    }

    /// <summary>
    /// スキルID = 2, 「スピンキック」
    /// </summary>
    private void SpinKick()
    {
        // アニメーションを再生する
        SetTrigger("SpinKick");
        // ムーンサルトのスキルを持ってくる
        SkillBase spinKick = SkillControl.GetSkill(2);
        // 攻撃力を設定する
        SetAttack(playerData.attack + (int)(playerData.attack * spinKick.GetAttack()));
        // 攻撃するコライダーをオンにする
        EnablePlayerAttack();
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
