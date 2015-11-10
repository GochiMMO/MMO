using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class PopEnemy : Photon.MonoBehaviour {
    [SerializeField, Tooltip("敵の出現間隔(秒)")]
    float rePopSec;
    [SerializeField, Tooltip("敵の最大出現数")]
    int maxEnemyNum;
    [SerializeField, Tooltip("出現させる敵のプレハブ")]
    GameObject popEnemyPrefab;

    int nowPopEnemyNum = 0;
    float time = 0f;
    // Use this for initialization
    void Start()
    {
        time = Time.time;
    }

    /*
    //現在出現している敵の数を同期させる
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(nowPopEnemyNum);
            //Debug.Log("sendPopEnemy" + nowPopEnemyNum);
        }
        else
        {
            nowPopEnemyNum = (int)stream.ReceiveNext();
            //Debug.Log("receivePopEnemy" + nowPopEnemyNum);
        }
    }
    */

    // Update is called once per frame
    void Update()
    {
        // マスタークライアントのみ処理を行う
        if (PhotonNetwork.isMasterClient)
        {
            // 敵がまだ出現できるとき
            if (nowPopEnemyNum < maxEnemyNum)
            {
                // 出現時間に達したら
                if (Time.time - time >= rePopSec)
                {
                    // 敵をインスタンス化する
                    GameObject enemy = PhotonNetwork.InstantiateSceneObject("Enemy/" + popEnemyPrefab.name, gameObject.transform.position, Quaternion.identity, 0, null);
                    // 敵の総数を加算する
                    nowPopEnemyNum++;
                    // 時間を再設定する
                    time = Time.time;
                    // 敵のデータを取得する
                    var enemyData = enemy.GetComponent<EnemyData>();
                    // 自分の参照を入れておく
                    enemyData.myPopScriptRefarence = this;
                }
            }
        }
    }

    public void DeadEnemy()
    {
        nowPopEnemyNum--;
    }
}
