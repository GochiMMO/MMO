using UnityEngine;
using System.Collections;

/// <summary>
/// サハギンを動かすスクリプト
/// </summary>
public class Sahagin : LoiteringEnemyBase
{
    /// <summary>
    /// とびかかり攻撃が来る割合
    /// </summary>
    private const int SPECIAL_ATTACK_PERCENT = 10;

    /// <summary>
    /// ダメージを食らった時のアニメーションを行う確率
    /// </summary>
    private const int DAMAGE_ACTION_PERCENT = 20;

    /// <summary>
    /// ダメージを食らったアニメーションを行うのに必要なダメージ
    /// </summary>
    private const int DAMAGE_ACTION_NUM = 200;

    /// <summary>
    /// 名前を設定する
    /// </summary>
    protected override void SetName()
    {
        // 名前をサハギンとする
        enemyName = "サハギン";
    }

    /// <summary>
    /// ランダムで移動方向を変更する処理
    /// </summary>
    protected override void RotateDirection()
    {
        // ランダム行動用変数定義
        int random = Random.Range(0, 20);
        // ランダムの値が9を下回っていたら
        if (random < 9)
        {
            // 角度を変更する
            newRotation.y = 45f * random;
            // 角度が0度ならば
            if (newRotation.y == 0f)
            {
                // 歩きモーションフラグと走るモーションフラグをオフにする
                anim.SetBool("walkFlag", false);
                anim.SetBool("runFlag", false);
                // 移動速度を0にする
                moveValue.z = 0;
            }
        }
    }

    /// <summary>
    /// 攻撃が終了した時の処理
    /// </summary>
    protected override bool IsEndAttack()
    {
        // 攻撃アニメーションが終了したら
        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            // 状態を変更する
            this.enemyStatus = Status.NORMAL;
            // trueを返す
            return true;
        }
        // 攻撃中
        return false;
    }

    /// <summary>
    /// プレイヤーの近くにいる時の処理
    /// </summary>
    protected override void NearPlayerAction()
    {
        // 攻撃アニメーションでない時
        if (enemyStatus != Status.ATTACK)
        {
            // ステータスを攻撃に変更する
            enemyStatus = Status.ATTACK;
            // 移動速度を0にする
            moveValue.z = 0f;
            // ランダム行動用変数定義
            int random = Random.Range(0, 101);

            // 一定の割合で普通の攻撃
            if (random >= SPECIAL_ATTACK_PERCENT)
            {
                // 普通の攻撃アニメーション
                anim.SetTrigger("attackFlag");
                Debug.Log("Normal Attack");
            }
            // 一定の割合でスペシャル攻撃
            else
            {
                // ジャンプ攻撃のアニメーション
                anim.SetTrigger("jumpAttack");
                Debug.Log("JumpAttack");
            }
        }
    }

    /// <summary>
    /// 攻撃を受けた時の処理
    /// </summary>
    protected override void SufferDamageAction(Collider col, int damage)
    {
        // ダメージが一定の値を超えていたら
        if (damage >= DAMAGE_ACTION_NUM)
        {
            // 一定の確立で処理を行う
            if (Random.Range(0, 101) < DAMAGE_ACTION_PERCENT)
            {
                // ダメージを受けたモーションにする
                anim.SetTrigger("damaged");
                // ステータスを被弾にする
                enemyStatus = Status.DAMEGE;
            }
        }
    }

    /// <summary>
    /// ダメージを食らう処理が終わったかどうか
    /// </summary>
    /// <returns>終了:true 継続:false</returns>
    protected override bool IsEndDamage()
    {
        // ダメージ中のアニメーションが終わったら
        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Damage"))
        {
            // ステータスを普通の状態に戻す
            enemyStatus = Status.NORMAL;
            // trueを返し、ダメージを食らった処理が終わったことを通知する
            return true;
        }
        // まわ終わっていないのでfalseを返す
        return false;
    }
}