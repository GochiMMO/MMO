using UnityEngine;
using System.Collections;

public class Sorcerer : PlayerChar {
    /// <summary>
    /// 詠唱を行っているかのフラグ
    /// </summary>
    bool chantFlag;
    /// <summary>
    /// 発動する魔法を入れる変数
    /// </summary>
    System.Action magic;
    /// <summary>
    /// 詠唱を行う関数
    /// </summary>
    /// <param name="chantTime">詠唱時間</param>
    /// <returns>反復子</returns>
    IEnumerator Chant(float chantTime)
    {
        // 詠唱が終了するまで待つ
        yield return new WaitForSeconds(chantTime);
        // 終了したらモーションを変更する
        SetTrigger("EndChant");
        // コルーチンから抜け出す
        yield break;
    }

    /// <summary>
    /// 攻撃する際の処理
    /// </summary>
    protected override void Attack()
    {
        base.Attack();
    }

    /// <summary>
    /// 攻撃中の処理
    /// </summary>
    protected override void Attacking()
    {

    }

    /// <summary>
    /// ダメージを受ける時の処理
    /// </summary>
    protected override void Damage()
    {
        
    }

    /// <summary>
    /// 死んだ時の処理
    /// </summary>
    protected override void Dead()
    {

    }

    /// <summary>
    /// 攻撃処理の終了
    /// </summary>
    protected override void EndOfAttack()
    {
        base.EndOfAttack();
    }

    /// <summary>
    /// 通常状態の処理
    /// </summary>
    protected override void Normal()
    {

    }

    /// <summary>
    /// プレイヤーがシーンから削除された時の処理
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    /// <summary>
    /// 生き返った瞬間の処理
    /// </summary>
    protected override void Revive()
    {

    }

    /// <summary>
    /// オブジェクトがシーンに読み込まれて最初のUpdate文の前に呼ばれる関数
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// スキルを発動する関数
    /// </summary>
    /// <param name="skillNumber">スキルのID</param>
    /// <param name="skill">スキルクラス</param>
    /// <returns>true : 発動できる、 false : 発動できない</returns>
    public override bool UseSkill(int skillNumber, SkillBase skill)
    {
        // 詠唱を行うかのフラグ
        bool chantFlag = true;
        // 消費SPを超えているか、他の魔法を発動中ならば
        if (skill.GetSp() > SP || this.chantFlag)
        {
            // スキルを発動できない
            return false;
        }
        // SPを消費させる
        SP -= skill.GetSp();
        // スキルのIDで処理分けを行う
        switch (skillNumber)
        {
            // 0番目のスキル(Fire)
            case 0:
                // メソッドの登録
                this.magic = () => Fire();
                break;
            // 1番目のスキル(バーン)
            case 1:
                // メソッドの登録
                this.magic = () => Burn();
                break;
            // 2番目のスキル(フレア)
            case 2:
                // メソッドの登録
                this.magic = () => Flare();
                break;
            // 3番目のスキル(メテオ)
            case 3:
                // メソッドの登録
                this.magic = () => Meteo();
                break;
            // 7番目のスキル(サンダー)
            case 7:
                this.magic = () => Thunder();
                break;
            // 8番目のスキル(ショック)
            case 8:
                this.magic = () => Shock();
                break;
            // 9番目のスキル(サンダーボルト)
            case 9:
                this.magic = () => ThunderVolt();
                break;
            // 10番目のスキル(ヒール)
            case 10:
                this.magic = () => Heal(MouseRay.targetPlayer);
                break;
            // 12番目のスキル(リジェネ)
            case 12:
                this.magic = () => Regene(MouseRay.targetPlayer);
                break;
            // 13番目のスキル(ベネディクション)
            case 13:
                this.magic = () => Venediction(MouseRay.targetPlayer);
                break;
            // 14番目のスキル(コンバート)
            case 14:
                // SPが0ならば
                if (SP == 0)
                {
                    // 発動できない
                    return false;
                }
                this.magic = () => Convert();
                // 詠唱を行うフラグを折る
                chantFlag = false;
                // モーションを再生する
                SetTrigger("Buffs");
                break;
            // 15番目のスキル(エーテルフロー)
            case 15:
                // メソッドの登録
                this.magic = () => EtherFlow();
                // 詠唱を行うフラグを折る
                chantFlag = false;
                // モーションを再生する
                SetTrigger("Buffs");
                break;
        }
        // 詠唱を行うかのフラグが立っていたら
        if (chantFlag)
        {
            // スキルの難易度で処理分けを行う(詠唱のモーションが違うため)
            switch (skill.GetDifficult())
            {
                case 1:
                    // １等級魔法の詠唱を行う
                    SetTrigger("FirstLevelMagic");
                    break;
                case 2:
                    // ２等級魔法の詠唱を行う
                    SetTrigger("SecondLevelMagic");
                    break;
                default:
                    // ３等級魔法の詠唱を行う
                    SetTrigger("ThirdLevelMagic");
                    break;
            }
            // 詠唱を行う
            StartCoroutine(Chant(skill.GetCastTime()));
        }
        // 詠唱を行うフラグを立てる
        this.chantFlag = true;
        // 魔法を発動できるのでtrueを返す
        return true;
    }

    /// <summary>
    /// 詠唱が終了したモーションが終了したら呼び出される
    /// </summary>
    public override void EndAttackAnimation()
    {
        // 基礎クラスの処理を行う
        base.EndAttackAnimation();
        // 詠唱を行っているフラグを折る
        chantFlag = false;
        // 魔法を発動する
        magic();
    }

    /// <summary>
    /// 初級魔法「ファイア」
    /// </summary>
    private void Fire()
    {
        // ファイアのスキルを持ってくる
        SkillBase fireSkill = SkillControl.GetSkill("ファイア");
        // ファイアオブジェクトをインスタンス化する
        GameObject fire = PhotonNetwork.Instantiate("Magics/Fire", gameObject.transform.position + Vector3.up * 5f, Quaternion.identity, 0);
        // ファイアの攻撃スクリプトをゲットする
        PlayerAttack fireAttack = fire.GetComponent<PlayerAttack>();
        // 基礎攻撃力を取得する
        float magicAttack = playerData.magicAttack;
        // 攻撃力に掛ける倍率を計算する
        float magicAttackRate = fireSkill.GetAttack() + fireSkill.GetBonus();
        // ファイアの攻撃力を設定する
        fireAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate) * intBuff);
        // ファイアは魔法なので、種類を魔法にする
        fireAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
        // 自分を設定する
        fireAttack.parentPlayer = this;
        // ファイアに角度を与え、発射する
        fire.GetComponent<FireShot>().SetShotVec(gameObject.transform.forward);

    }

    /// <summary>
    /// 前方３メートル先を爆発させるスキル「バーン」
    /// </summary>
    private void Burn()
    {
        // バーンのスキルのデータを取得する
        SkillBase burnSkill = SkillControl.GetSkill("バーン");
        // バーンのオブジェクトをインスタンス化する
        GameObject burn = PhotonNetwork.Instantiate("Magics/Burn", transform.position + transform.forward * 3f + Vector3.up * 3f, Quaternion.identity, 0);
        // バーンの攻撃スクリプトを取得する
        PlayerAttack burnAttack = burn.GetComponent<PlayerAttack>();
        // 基礎攻撃力の設定
        float magicAttack = playerData.magicAttack;
        // 魔法の倍率の計算
        float magicAttackRate = burnSkill.GetAttack() + burnSkill.GetBonus();
        // 攻撃力の設定
        burnAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate) * intBuff);
        // 自分を設定する
        burnAttack.parentPlayer = this;
        // 攻撃の種類を魔法に設定する
        burnAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
    }

    /// <summary>
    /// ５メートル先を爆発させる「フレアー」
    /// </summary>
    private void Flare()
    {
        // フレアのスキルのデータを取得する
        SkillBase flareSkill = SkillControl.GetSkill("フレア");
        // フレアのオブジェクトをインスタンス化する
        GameObject flare = PhotonNetwork.Instantiate("Magics/Flare", transform.position + transform.forward * 5f + Vector3.up * 3f, Quaternion.identity, 0);
        // フレアの攻撃スクリプトを取得する
        PlayerAttack flareAttack = flare.GetComponent<PlayerAttack>();
        // 基礎攻撃力の設定
        float magicAttack = playerData.magicAttack;
        // 倍率の設定
        float magicAttackRate = flareSkill.GetAttack() + flareSkill.GetBonus();
        // 攻撃力の設定
        flareAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate) * intBuff);
        // 自分を設定する
        flareAttack.parentPlayer = this;
        // 攻撃の種類を魔法に設定する
        flareAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
    }

    /// <summary>
    /// ５メートル先に雷を発生させる「サンダー」
    /// </summary>
    private void Thunder()
    {
        // サンダーのスキルのデータを取得する
        SkillBase thunderSkill = SkillControl.GetSkill("サンダー");
        // サンダーのオブジェクトをインスタンス化する
        GameObject thunder = PhotonNetwork.Instantiate("Magics/Thunder", transform.position + transform.forward * 5f, Quaternion.identity, 0);
        // サンダーの攻撃スクリプトを取得する
        PlayerAttack thunderAttack = thunder.GetComponent<PlayerAttack>();
        // 基礎攻撃力の設定
        float magicAttack = playerData.magicAttack;
        // 倍率の設定
        float magicAttackRate = thunderSkill.GetAttack() + thunderSkill.GetBonus();
        // 攻撃力の設定
        thunderAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate) * intBuff);
        // 自分を設定する
        thunderAttack.parentPlayer = this;
        // 攻撃の種類を魔法に設定する
        thunderAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
    }

    /// <summary>
    /// 雷の球体を発射し、着弾点を爆発させるスキル「ショック」
    /// </summary>
    private void Shock()
    {
        // サンダーのスキルのデータを取得する
        SkillBase shockSkill = SkillControl.GetSkill("ショック");
        // ショックのオブジェクトをインスタンス化する
        GameObject shock = PhotonNetwork.Instantiate("Magics/Shock", transform.position + Vector3.up * 4f, Quaternion.identity, 0);
        // ショックの攻撃スクリプトを取得する
        PlayerAttack shockAttack = shock.GetComponent<PlayerAttack>();
        // 発射角度を設定する
        shock.GetComponent<Shock>().SetDirection(gameObject.transform.forward);
        // 基礎攻撃力の設定
        float magicAttack = playerData.magicAttack;
        // 倍率の設定
        float magicAttackRate = shockSkill.GetAttack() + shockSkill.GetBonus();
        // 攻撃力の設定
        shockAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate) * intBuff);
        // 自分を設定する
        shockAttack.parentPlayer = this;
        // 攻撃の種類を魔法に設定する
        shockAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
    }

    /// <summary>
    /// ７メートル先に雷を発生させる「サンダーボルト」
    /// </summary>
    private void ThunderVolt()
    {
        // サンダーのスキルのデータを取得する
        SkillBase thunderVoltSkill = SkillControl.GetSkill("サンダーボルト");
        // サンダーボルトのオブジェクトをインスタンス化する
        GameObject thunderVolt = PhotonNetwork.Instantiate("Magics/ThunderVolt", transform.position + transform.forward * 7f, Quaternion.identity, 0);
        // サンダーボルトの攻撃スクリプトを取得する
        PlayerAttack thunderVoltAttack = thunderVolt.GetComponent<PlayerAttack>();
        // 基礎攻撃力の設定
        float magicAttack = playerData.magicAttack;
        // 倍率の設定
        float magicAttackRate = thunderVoltSkill.GetAttack() + thunderVoltSkill.GetBonus();
        // 攻撃力の設定
        thunderVoltAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate) * intBuff);
        // 自分を設定する
        thunderVoltAttack.parentPlayer = this;
        // 攻撃の種類を魔法に設定する
        thunderVoltAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
    }

    /// <summary>
    /// 10メートル先に隕石を落とす「メテオ」
    /// </summary>
    private void Meteo()
    {
        // メテオのスキルのデータを取得する
        SkillBase meteoSkill = SkillControl.GetSkill("メテオ");
        // メテオのオブジェクトをインスタンス化する
        GameObject meteo = PhotonNetwork.Instantiate("Magics/Meteo", transform.position + transform.forward * 10f, Quaternion.identity, 0);
        // メテオの攻撃スクリプトを取得する
        PlayerAttack meteoAttack = meteo.GetComponent<PlayerAttack>();
        // 基礎攻撃力の設定
        float magicAttack = playerData.magicAttack;
        // 倍率の設定
        float magicAttackRate = meteoSkill.GetAttack() + meteoSkill.GetBonus();
        // 攻撃力の設定
        meteoAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate) * intBuff);
        // 自分を設定する
        meteoAttack.parentPlayer = this;
        // 攻撃の種類を魔法に設定する
        meteoAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
    }

    /// <summary>
    /// 自分のMPを回復する「エーテルフロー」
    /// </summary>
    private void EtherFlow()
    {
        // エフェクトを表示する
        // PhotonNetwork.Instantiate("Magics/Ether", transform.position, Quaternion.identify, 0);
        // エーテルフローのスキルをゲットする
        SkillBase etherFlow = SkillControl.GetSkill("エーテルフロー");
        // SPを回復させる
        SP += (int)((float)playerData.MaxSP * etherFlow.GetAttack());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    private void Regene(GameObject player)
    {
        // リジェネのスキルを取得する
        SkillBase regeneSkill = SkillControl.GetSkill("リジェネ");
        // プレイヤーのスクリプトを取得する
        PlayerChar playerChar = player.GetComponent<PlayerChar>();
        // playerがプレイヤーならば
        if (player)
        {
            // 回復量を計算する
            int healHP = (int)((float)playerChar.GetPlayerData().MaxHP * (regeneSkill.GetBonus() + regeneSkill.GetAttack()));
            // 効果時間を計算する
            float skillTime = 20 + regeneSkill.GetLv() * 5;
            // 回復する時間を計算する
            float healTime = 5f - regeneSkill.GetLv() * 0.2f;
            // リジェネを発動させる
            photonView.RPC("GenerationRegeneration", player.GetPhotonView().owner, healHP, skillTime, healTime);
        }
        else{
            // 回復量を計算する
            int healHP = (int)((float)playerData.MaxHP * (regeneSkill.GetBonus() + regeneSkill.GetAttack()));
            // 効果時間を計算する
            float skillTime = 20 + regeneSkill.GetLv() * 5;
            // 回復する時間を計算する
            float healTime = 5f - regeneSkill.GetLv() * 0.5f;
            // リジェネを発動する
            GenerationRegeneration(healHP, skillTime, healTime);
        }
    }

    /// <summary>
    /// HPとSPを入れ替えるスキル「コンバート」
    /// </summary>
    private void Convert()
    {
        // エフェクトを表示する
        // PhotonNetwork.Instantiate("Magics/Convert", transform.position, Quaternion.identity, 0);
        // SPが0の時は発動できない(HPが0になってしまうため
        if (SP != 0)
        {
            // SPを退避する
            int tempSP = SP;
            // SPをHPにする
            SP = HP;
            // HPをSPにする
            HP = tempSP;
        }
    }

    /// <summary>
    /// ヒール
    /// </summary>
    /// <param name="player">回復させるプレイヤーオブジェクト</param>
    private void Heal(GameObject player)
    {
        // プレイヤーの情報を取得する
        PlayerChar playerChar = player.GetComponent<PlayerChar>();
        // ヒールのスキルを取得する
        SkillBase healSkill = SkillControl.GetSkill("ヒール");
        // プレイヤーが取得できれば
        if (playerChar)
        {
            // 回復させるHPの量を計算する
            int recoverHP = (int)((float)playerChar.GetPlayerData().MaxHP * (healSkill.GetBonus() + healSkill.GetAttack()));
            // そのプレイヤーのHPを回復させる
            gameObject.GetPhotonView().RPC("Recover", player.GetPhotonView().owner, recoverHP);
        }
        else
        {
            // 自分のHPを回復する
            Recover((int)((float)playerData.MaxHP * (healSkill.GetBonus() + healSkill.GetAttack())));
        }
    }

    /// <summary>
    /// プレイヤーを全回復させる関数
    /// </summary>
    /// <param name="player">回復させる対象</param>
    private void Venediction(GameObject player)
    {
        // プレイヤーの情報をゲットする
        PlayerChar playerChar = player.GetComponent<PlayerChar>();
        // プレイヤーが取得できれば
        if (playerChar)
        {
            // 他の誰かのHPを回復させる
            gameObject.GetPhotonView().RPC("Recover", player.GetPhotonView().owner, playerChar.GetPlayerData().MaxHP);
        }
        // 情報が取得できなければ
        else
        {
            // 自分のHPを回復させる
            Recover(playerData.MaxHP);
        }
    }
}
