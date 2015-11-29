using UnityEngine;
using System.Collections;

public class SpawnFlare : MonoBehaviour {
    [SerializeField, Tooltip("爆発のパーティクルシステム")]
    ParticleSystem explosionParticle;
    [SerializeField, Tooltip("エフェクトのパーティクルシステム")]
    ParticleSystem effectParticle;
    [SerializeField, Tooltip("エフェクトを表示する時間")]
    float effectShowTime;

    CapsuleCollider col;
    float firstTime = 0f;
    bool playFlag = false;
    float colliderRadius;

    // Use for initialization.
    void Awake () {
        col = GetComponent<CapsuleCollider>();   //コリジョンコンポーネントを取得
        col.enabled = false;        //一度無効化する(フレアは演出を挟むため)
        colliderRadius = col.radius;
        col.radius = 0f;    //コライダーの半径を0にする
    }

    void Start()
    {
        firstTime = Time.time;  //開始時間を登録する
    }
    
    // Update is called once per frame
    void Update () {
        if (Time.time - firstTime >= effectShowTime)   //演出時間が終了したら
        {
            if (!playFlag)   //爆発パーティクルが再生されていなければ
            {
                explosionParticle.Play();   //爆発パーティクル再生
                effectParticle.Stop();      //演出のストップ
                col.radius = colliderRadius;//半径の設定し直し
                col.enabled = true;         //コリジョンコンポーネントの有効化
                playFlag = true;
            }
        }
    }
}
