using UnityEngine;
using System.Collections;

public class LifeSpan : Photon.MonoBehaviour {
    [SerializeField]
    int lifeSpan;
    [SerializeField]
    bool usePhotonNetwork = true;

    int counter = 0;
    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
        if (lifeSpan * 60 <= counter++)     //寿命が来たら
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
