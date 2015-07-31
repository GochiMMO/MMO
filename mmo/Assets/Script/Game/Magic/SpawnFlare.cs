using UnityEngine;
using System.Collections;

public class SpawnFlare : MonoBehaviour {
    [SerializeField, Tooltip("爆発のパーティクルシステム")]
    ParticleSystem explosionParticle;
    [SerializeField, Tooltip("エフェクトのパーティクルシステム")]
    ParticleSystem effectParticle;
    [SerializeField, Tooltip("エフェクトを表示する時間")]
    int effectShowTime;

    SphereCollider col;
    int counter = 0;
    bool playFlag = false;
    // Use this for initialization
    void Start () {
        col = GetComponent<SphereCollider>();   //コリジョンコンポーネントを取得
        col.enabled = false;        //一度無効化する(フレアは演出を挟むため)
    }
    
    // Update is called once per frame
    void Update () {
        if (counter++ >= effectShowTime * 60)   //演出時間が終了したら
        {
            if (!playFlag)   //爆発パーティクルが再生されていなければ
            {
                explosionParticle.Play();       //爆発パーティクル再生
                effectParticle.Stop();          //演出のストップ
                col.enabled = true;         //コリジョンコンポーネントの有効化
                playFlag = true;
            }
        }
    }
}
