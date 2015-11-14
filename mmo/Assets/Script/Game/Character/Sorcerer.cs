using UnityEngine;
using System.Collections;

public class Sorcerer : PlayerChar {
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
    /// 初級魔法「ファイア」
    /// </summary>
    private void Fire()
    {
        // ファイアのスキルを持ってくる
        SkillBase fireSkill = SkillControl.GetSkill("ファイア");
        // 現在SPが消費SPを上回っていたら
        if (fireSkill.GetSp() < playerData.SP)
        {
            // SPを消費させる
            playerData.SP -= fireSkill.GetSp();
            // ファイアオブジェクトをインスタンス化する
            GameObject fire = PhotonNetwork.Instantiate("Magics/Fire", gameObject.transform.position + Vector3.up * 5f, Quaternion.identity, 0);
            // ファイアの攻撃スクリプトをゲットする
            PlayerAttack fireAttack = fire.GetComponent<PlayerAttack>();
            // 基礎攻撃力を取得する
            float magicAttack = playerData.magicAttack;
            // 攻撃力に掛ける倍率を計算する
            float magicAttackRate = fireSkill.GetAttack() + fireSkill.GetBonus();
            // ファイアの攻撃力を設定する
            fireAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate)  * intBuff);
            // ファイアは魔法なので、種類を魔法にする
            fireAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
            // ファイアに角度を与え、発射する
            fire.GetComponent<FireShot>().SetShotVec(gameObject.transform.rotation.eulerAngles.y);
        }
    }

    /// <summary>
    /// 前方３メートル先を爆発させるスキル「バーン」
    /// </summary>
    private void Burn()
    {
        // バーンのスキルのデータを取得する
        SkillBase burnSkill = SkillControl.GetSkill("バーン");
        // 現在SPが消費SPを上回っていたら
        if (burnSkill.GetSp() < playerData.SP)
        {
            // SPを消費させる
            playerData.SP -= burnSkill.GetSp();
            // バーンのオブジェクトをインスタンス化する
            GameObject burn = PhotonNetwork.Instantiate("Magic/Burn", transform.position + transform.forward * 3f + Vector3.up * 3f, Quaternion.identity, 0);
            // バーンの攻撃スクリプトを取得する
            PlayerAttack burnAttack = burn.GetComponent<PlayerAttack>();
            // 基礎攻撃力の設定
            float magicAttack = playerData.magicAttack;
            // 魔法の倍率の計算
            float magicAttackRate = burnSkill.GetAttack() + burnSkill.GetBonus();
            // 攻撃力の設定
            burnAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate) * intBuff);
            // 攻撃の種類を魔法に設定する
            burnAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
        }
    }

    /// <summary>
    /// ５メートル先を爆発させる「フレアー」
    /// </summary>
    private void Flare()
    {
        // フレアのスキルのデータを取得する
        SkillBase flareSkill = SkillControl.GetSkill("フレア");
        // 現在SPが消費SPを上回っていたら
        if (flareSkill.GetSp() < playerData.SP)
        {
            // SPを消費させる
            playerData.SP -= flareSkill.GetSp();
            // フレアのオブジェクトをインスタンス化する
            GameObject flare = PhotonNetwork.Instantiate("Magic/Flare", transform.position + transform.forward * 5f + Vector3.up * 3f, Quaternion.identity, 0);
            // フレアの攻撃スクリプトを取得する
            PlayerAttack flareAttack = flare.GetComponent<PlayerAttack>();
            // 基礎攻撃力の設定
            float magicAttack = playerData.magicAttack;
            // 倍率の設定
            float magicAttackRate = flareSkill.GetAttack() + flareSkill.GetBonus();
            // 攻撃力の設定
            flareAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate) * intBuff);
            // 攻撃の種類を魔法に設定する
            flareAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
        }
    }

    /// <summary>
    /// ５メートル先に雷を発生させる「サンダー」
    /// </summary>
    private void Thunder()
    {
        // サンダーのスキルのデータを取得する
        SkillBase thunderSkill = SkillControl.GetSkill("サンダー");
        // 現在SPが消費SPを上回っていたら
        if (thunderSkill.GetSp() < playerData.SP)
        {
            // SPを消費させる
            playerData.SP -= thunderSkill.GetSp();
            // サンダーのオブジェクトをインスタンス化する
            GameObject thunder = PhotonNetwork.Instantiate("Magic/Thunder", transform.position + transform.forward * 5f, Quaternion.identity, 0);
            // サンダーの攻撃スクリプトを取得する
            PlayerAttack thunderAttack = thunder.GetComponent<PlayerAttack>();
            // 基礎攻撃力の設定
            float magicAttack = playerData.magicAttack;
            // 倍率の設定
            float magicAttackRate = thunderSkill.GetAttack() + thunderSkill.GetBonus();
            // 攻撃力の設定
            thunderAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate) * intBuff);
            // 攻撃の種類を魔法に設定する
            thunderAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
        }
    }

    /// <summary>
    /// 雷の球体を発射し、着弾点を爆発させるスキル「ショック」
    /// </summary>
    private void shock()
    {
        // サンダーのスキルのデータを取得する
        SkillBase shockSkill = SkillControl.GetSkill("ショック");
        // 現在SPが消費SPを上回っていたら
        if (shockSkill.GetSp() < playerData.SP)
        {
            // SPを消費させる
            playerData.SP -= shockSkill.GetSp();
            // ショックのオブジェクトをインスタンス化する
            GameObject shock = PhotonNetwork.Instantiate("Magic/Thunder", transform.position + Vector3.up * 4f, Quaternion.identity, 0);
            // ショックの攻撃スクリプトを取得する
            PlayerAttack shockAttack = shock.GetComponent<PlayerAttack>();
            // 発射角度を設定する
            shock.GetComponent<Shock>().SetDirection(gameObject.transform.position.y);
            // 基礎攻撃力の設定
            float magicAttack = playerData.magicAttack;
            // 倍率の設定
            float magicAttackRate = shockSkill.GetAttack() + shockSkill.GetBonus();
            // 攻撃力の設定
            shockAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate) * intBuff);
            // 攻撃の種類を魔法に設定する
            shockAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
        }
    }

    /// <summary>
    /// ７メートル先に雷を発生させる「サンダーボルト」
    /// </summary>
    private void ThunderVolt()
    {
        // サンダーのスキルのデータを取得する
        SkillBase thunderVoltSkill = SkillControl.GetSkill("サンダーボルト");
        // 現在SPが消費SPを上回っていたら
        if (thunderVoltSkill.GetSp() < playerData.SP)
        {
            // SPを消費させる
            playerData.SP -= thunderVoltSkill.GetSp();
            // サンダーボルトのオブジェクトをインスタンス化する
            GameObject thunderVolt = PhotonNetwork.Instantiate("Magic/Thunder", transform.position + transform.forward * 7f, Quaternion.identity, 0);
            // サンダーボルトの攻撃スクリプトを取得する
            PlayerAttack thunderVoltAttack = thunderVolt.GetComponent<PlayerAttack>();
            // 基礎攻撃力の設定
            float magicAttack = playerData.magicAttack;
            // 倍率の設定
            float magicAttackRate = thunderVoltSkill.GetAttack() + thunderVoltSkill.GetBonus();
            // 攻撃力の設定
            thunderVoltAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate) * intBuff);
            // 攻撃の種類を魔法に設定する
            thunderVoltAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
        }
    }

    /// <summary>
    /// 10メートル先に隕石を落とす「メテオ」
    /// </summary>
    private void Meteo()
    {
        // メテオのスキルのデータを取得する
        SkillBase meteoSkill = SkillControl.GetSkill("メテオ");
        // 現在SPが消費SPを上回っていたら
        if (meteoSkill.GetSp() < playerData.SP)
        {
            // SPを消費させる
            playerData.SP -= meteoSkill.GetSp();
            // メテオのオブジェクトをインスタンス化する
            GameObject meteo = PhotonNetwork.Instantiate("Magic/Thunder", transform.position + transform.forward * 10f, Quaternion.identity, 0);
            // メテオの攻撃スクリプトを取得する
            PlayerAttack meteoAttack = meteo.GetComponent<PlayerAttack>();
            // 基礎攻撃力の設定
            float magicAttack = playerData.magicAttack;
            // 倍率の設定
            float magicAttackRate = meteoSkill.GetAttack() + meteoSkill.GetBonus();
            // 攻撃力の設定
            meteoAttack.attack = (int)((magicAttack + magicAttack * magicAttackRate) * intBuff);
            // 攻撃の種類を魔法に設定する
            meteoAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
        }
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
        playerData.SP += (int)((float)playerData.MaxSP * etherFlow.GetAttack());
    }

    /// <summary>
    /// HPとSPを入れ替えるスキル「コンバート」
    /// </summary>
    private void Convert()
    {
        // エフェクトを表示する
        // PhotonNetwork.Instantiate("Magics/Convert", transform.position, Quaternion.identify, 0);

        // SPをHPにする
        playerData.SP = playerData.HP;
        // HPをSPにする
        playerData.HP = playerData.SP;
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
