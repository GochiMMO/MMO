using UnityEngine;
using System.Collections;

public class Haight : MonoBehaviour {

    int haighting;

    /// <summary>
    /// ヘイト用ゲッター
    /// </summary>
    /// <returns></returns>
    public int GetHaight() 
    {
        return haighting;
    }

    void Start () {
        haighting = 0;
    }
    
    void Update () {
        //ここにヘイトを上昇、もしくは減少の処理をさせる
    }

    //状態の同期
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(haighting);   //ステータスを送信
        }
        else
        {
            haighting = (int)stream.ReceiveNext(); //ステータスを受信
        }
    }
}
