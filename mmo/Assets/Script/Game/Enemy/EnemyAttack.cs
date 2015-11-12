using UnityEngine;
using System.Collections;

// カプセルコライダーをアタッチする(攻撃当たり判定用)
[RequireComponent(typeof(CapsuleCollider))]
/// <summary>
/// 敵の攻撃力等格納用スクリプト
/// </summary>
public class EnemyAttack : MonoBehaviour {
    /// <summary>
    /// 敵の攻撃の種類
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
    new public CapsuleCollider collider { private set; get; }
    /// <summary>
    /// ダメージの振れ幅
    /// </summary>
    [HideInInspector]
    public float damageRate;
    /// <summary>
    /// 敵の攻撃の種類
    /// </summary>
    public AttackKind attackKind = AttackKind.PHYSICS;

    /// <summary>
    /// 疑似コンストラクタ
    /// </summary>
    private void Start()
    {
        // コライダーを取得する
        collider = gameObject.GetComponent<CapsuleCollider>();
        // 当り判定を行うだけにする
        collider.isTrigger = true;
        // タグを設定する
        gameObject.tag = "EnemyAttack";
    }
}
