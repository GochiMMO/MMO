using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーの攻撃コンポーネント
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PhotonView))]
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
    /// 攻撃が当たったときに加算されるSPの値
    /// </summary>
    public int sp;
    /// <summary>
    /// 攻撃力
    /// </summary>
    // [HideInInspector]
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
    public float damageRate = 0.1f;
    /// <summary>
    /// プレイヤーの攻撃の種類
    /// </summary>
    [HideInInspector]
    public AttackKind attackKind = AttackKind.PHYSICS;
    /// <summary>
    /// その攻撃オブジェクトを出現させたプレイヤー
    /// </summary>
    public PlayerChar parentPlayer = null;

    /// <summary>
    /// プロパティを設定する
    /// </summary>
    /// <param name="attack">攻撃力</param>
    /// <param name="damageRate">ダメージの振れ幅</param>
    /// <param name="kind">攻撃の種類</param>
    /// <param name="parentPlayer">攻撃を出した親オブジェクト</param>
    public void SetProperties(int attack, float damageRate, AttackKind kind, PlayerChar parentPlayer, int sp = 0)
    {
        // 攻撃力を設定する
        this.attack = attack;
        // 攻撃力の振れ幅を設定する
        this.damageRate = damageRate;
        // 攻撃の種類を設定する
        this.attackKind = kind;
        // 攻撃の親オブジェクトを設定する
        this.parentPlayer = parentPlayer;
        // 加算されるSPの値を入れる
        this.sp = sp;
    }

    public void SetProperties(PlayerAttack playerAttack)
    {
        SetProperties(playerAttack.attack, playerAttack.damageRate, playerAttack.attackKind, playerAttack.parentPlayer, playerAttack.sp);
    }

    /// <summary>
    /// 何かと衝突した時に呼ばれる関数
    /// </summary>
    /// <param name="col">衝突したコライダー</param>
    void OnTriggerEnter(Collider col)
    {
        // 当たった物体が敵ならば
        if (col.tag == "Enemy")
        {
            // SPを加算する
            // parentPlayer.SP += sp;
        }
    }

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
