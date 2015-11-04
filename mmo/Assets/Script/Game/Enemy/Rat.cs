using UnityEngine;
using System.Collections;

public class Rat : LoiteringEnemyBase {
    /// <summary>
    /// 名前を設定する
    /// </summary>
    protected override void SetName()
    {
        // ラットにする
        enemyName = "ラット";
    }

    /// <summary>
    /// 回転の決定などを行う
    /// </summary>
    protected override void RotateDirection()
    {
        
    }

    /// <summary>
    /// プレイヤーの近くに来た時の処理
    /// </summary>
    /// <param name="distance">距離</param>
    protected override void NearPlayerAction(float distance)
    {

    }

    /// <summary>
    /// 攻撃を食らった処理が終了したかどうか
    /// </summary>
    /// <returns>true : 終了、false : 継続</returns>
    protected override bool IsEndDamage()
    {
        // ダメージアニメーション再生中ならば
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Damage"))
        {
            // falseを返す
            return false;
        }
        // 終わったのでtrueを返す
        return true;
    }

    /// <summary>
    /// 攻撃が終了したかどうか
    /// </summary>
    /// <returns>true : 終了、false : 継続</returns>
    protected override bool IsEndAttack()
    {
        // 攻撃中フラグが立っていたら
        if (attackFlag)
        {
            // falseを返す
            return false;
        }
        // 終わったらtrueを返す
        return true;
    }
}