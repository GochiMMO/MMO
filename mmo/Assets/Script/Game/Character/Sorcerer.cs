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
            // ファイアの攻撃力を設定する
            fireAttack.attack = playerData.magicAttack + (int)((float)this.playerData.magicAttack * fireSkill.GetAttack());
            // ファイアは魔法なので、種類を魔法にする
            fireAttack.attackKind = PlayerAttack.AttackKind.MAGIC;
            // ファイアに角度を与え、発射する
            fire.GetComponent<FireShot>().SetShotVec(gameObject.transform.rotation.eulerAngles.y);
        }
    }

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
            GameObject flare = PhotonNetwork.Instantiate("Magic/Flare", transform.forward * 5f, Quaternion.identity, 0);
        }
    }
}
