using UnityEngine;
using System.Collections;

abstract public class FullCustomEnemyBase : EnemyData {
    /// <summary>
    /// 攻撃中の処理
    /// </summary>
    protected abstract void OnAttack();
    /// <summary>
    /// その他の状態の時の処理
    /// </summary>
    protected abstract void OnOther();
    /// <summary>
    /// 被弾中の処理
    /// </summary>
    protected abstract void OnDamage();
    /// <summary>
    /// 通常状態の処理
    /// </summary>
    protected abstract void OnNormal();
    /// <summary>
    /// 威嚇中の処理
    /// </summary>
    protected abstract void OnIntimidation();
    /// <summary>
    /// プレイヤーを発見している時の処理
    /// </summary>
    protected abstract void OnDiscover();
    /// <summary>
    /// 死んだ時の処理
    /// </summary>
    protected abstract void OnDead();

    /// <summary>
    /// 更新処理
    /// </summary>
    protected sealed override void Update()
    {
        // マスタークライアントならば
        if (PhotonNetwork.isMasterClient)
        {
            // ステータスによって処理分け
            switch (this.enemyStatus)
            {
                // 通常状態
                case Status.NORMAL:
                    // 通常状態の処理を行う
                    OnNormal();
                    break;

                // 威嚇状態
                case Status.INTIMIDATION:
                    // 威嚇状態の処理を行う
                    OnIntimidation();
                    break;
                // 被弾中
                case Status.DAMEGE:
                    // 被弾中の処理を行う
                    OnDamage();
                    break;
                // プレイヤーを発見
                case Status.DISCOVER:
                    // 発見している時の処理を行う
                    OnDiscover();
                    break;
                // 死亡時
                case Status.DEAD:
                    // 死んだ時の処理
                    OnDead();
                    break;
                // その他の状態
                case Status.OTHER:
                    // その他の状態の処理を行う
                    OnOther();
                    break;
                    // 攻撃中
                case Status.ATTACK:
                    // 攻撃中の処理を行う
                    OnAttack();
                    break;
            }
        }
    }
}
