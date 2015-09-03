using UnityEngine;
using System.Collections;

public class LifeSpan : Photon.MonoBehaviour {
    [SerializeField, Tooltip("寿命(秒)")]
    float lifeSpan;
    [SerializeField, Tooltip("PhotonNetworkを使うかどうか")]
    bool usePhotonNetwork = true;

    float firstTime = 0f;
    // Use this for initialization
    void Start () {
        firstTime = Time.time;
    }
    
    // Update is called once per frame
    void Update () {
        if (Time.time - firstTime >= lifeSpan)     //寿命が来たら
        {
            if (usePhotonNetwork)   //ネットワーク使用フラグ
            {   
                if (photonView.isMine)  //自分が制御
                {
                    PhotonNetwork.Destroy(this.gameObject);     //削除
                }
            }
            else
            {
                GameObject.Destroy(this.gameObject);    //削除
            }
        }
    }
}
