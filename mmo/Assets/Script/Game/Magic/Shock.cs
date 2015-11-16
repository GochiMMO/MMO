using UnityEngine;
using System.Collections;

public class Shock : Photon.MonoBehaviour {
    [SerializeField, Tooltip("ショットのエフェクト")]
    ParticleSystem shotEffect;
    [SerializeField, Tooltip("爆発のエフェクト")]
    ParticleSystem shockEffect;

    public int speed;   //移動速度
    public float shotLife;  //ショット寿命
    public float shockLife; //ショック寿命

    float startTime;    //開始時間
    Vector3 moveVector; //移動ベクトル
    bool hitFlag;       //当たったかのフラグ

    PhotonTransformView photonTransformView;

    // Use this for initialization
    void Init() {
        // ヒット時の爆発パーティクルエフェクトを停止しておく
        shockEffect.Stop();
        // 通信同期用コンポーネントを取得する
        photonTransformView = GetComponent<PhotonTransformView>();
        // 魔法が出た瞬間の時間を登録する
        startTime = Time.time;
    }

    // 方向の登録
    public void SetDirection(Vector3 direction)
    {
        // 初期化処理を行う
        Init();
        // 移動方向の決定(プレイヤーの正面に向ける)
        moveVector = direction * speed;
        // 自分で出した魔法ならば
        if (photonView.isMine)
        {
            // 移動速度を同期する
            photonTransformView.SetSynchronizedValues(speed: moveVector, turnSpeed: 0);
        }
    }

    /// <summary>
    /// ヒットしたときに呼ばれる関数
    /// </summary>
    /// <param name="col">当たったコライダー</param>
    void OnTriggerEnter(Collider col)
    {
        // 今までヒットしておらず、ヒットしたオブジェクトがプレイヤーでなければ
        if (!hitFlag && col.gameObject.tag != "Player")
        {
            // ヒットしたことにする
            hitFlag = true;
            // 飛ばすためのパーティクルエフェクトを停止させる
            shotEffect.Stop();
            // 当たった時の爆発パーティクルエフェクトを再生する
            shockEffect.Play();
            // 移動ベクトルを0にする
            moveVector = Vector3.zero;
            // 時間を今度は爆発待機時間計測に使うため、初期化する
            startTime = Time.time;
            // 自分で出した魔法ならば
            if (photonView.isMine)
            {
                // 速度を同期(移動ベクトル = 0)する
                photonTransformView.SetSynchronizedValues(speed: moveVector, turnSpeed: 0);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        // まだヒットしていなければ
        if (!hitFlag)
        {
            // 移動させる
            this.transform.Translate(moveVector * Time.deltaTime, Space.World);
            // 自分自身が出した魔法ならば
            if (photonView.isMine)
            {
                // 時間が設定時間分経ったら
                if (Time.time - startTime >= shotLife)
                {
                    // 消す
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
        }
        // ヒットした後ならば
        else
        {
            // 自分が出した魔法ならば
            if (photonView.isMine)
            {
                // 時間が設定時間分経過したら
                if (Time.time - startTime >= shockLife)
                {
                    // 消す
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
        }
    }
}