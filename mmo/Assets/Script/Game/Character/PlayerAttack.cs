using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーの攻撃コンポーネント
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerAttack : MonoBehaviour {
    /// <summary>
    /// プレイヤーの攻撃の種類
    /// </summary>
    public enum AttackKind
    {
        /// <summary>
        /// 物理攻撃
        /// </summary>
        PHYSICS,
        /// <summary>
        /// 魔法攻撃
        /// </summary>
        MAGIC
    }
    /// <summary>
    /// 攻撃力
    /// </summary>
    [HideInInspector]
    public int attack;
    /// <summary>
    /// 当り判定を行うコンポーネント
    /// </summary>
    [HideInInspector]
    public CapsuleCollider col = null;
    /// <summary>
    /// ダメージの振れ幅
    /// </summary>
    [HideInInspector]
    public float damageRate;
    /// <summary>
    /// プレイヤーの攻撃の種類
    /// </summary>
    [HideInInspector]
    public AttackKind attackKind = AttackKind.PHYSICS;

    /// <summary>
    /// 振れ幅計算を行い、ダメージを返す関数
    /// </summary>
    /// <returns>ダメージ</returns>
    public int GetDamage()
    {
        // ダメージを計算し、返す
        return attack + (int)((float)attack * Random.Range(-damageRate, damageRate));
    }

    /// <summary>
    /// このスクリプトが生成された瞬間行われる処理
    /// </summary>
    void Awake()
    {
        // コライダーを取得する
        col = gameObject.GetComponent<CapsuleCollider>();
        // 当り判定を行うだけにする
        col.isTrigger = true;
        // タグを設定する
        gameObject.tag = "PlayerAttack";
    }
}
