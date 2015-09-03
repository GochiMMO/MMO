using UnityEngine;
using System.Collections;

public class PopEnemy : Photon.MonoBehaviour {
    [SerializeField, Tooltip("敵の出現間隔(秒)")]
    float rePopSec;
    [SerializeField, Tooltip("敵の最大出現数")]
    int maxEnemyNum;
    [SerializeField, Tooltip("出現させる敵のプレハブ")]
    GameObject popEnemyPrefab;
    [SerializeField, Tooltip("敵の出現位置")]
    Vector3[] popPoint;

    int nowPopEnemyNum = 0;
    float time = 0f;
    // Use this for initialization
    void Start()
    {
        time = Time.time;
    }

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

    // Update is called once per frame
    void Update()
    {
        //マスタークライアントのみ処理を行う
        if (PhotonNetwork.isMasterClient)
        {
            if (nowPopEnemyNum < maxEnemyNum)
            {
                if (Time.time - time >= rePopSec)
                {
                    GameObject enemy = PhotonNetwork.InstantiateSceneObject("Enemy/" + popEnemyPrefab.name, popPoint[Random.Range(0, popPoint.Length)], Quaternion.identity, 0, null);
                    nowPopEnemyNum++;
                    time = Time.time;
                    enemy.GetComponent<EnemyData>().myPopScriptRefarence = this;    //自分の参照を入れておく
                }
            }
        }
    }

    public void DeadEnemy()
    {
        nowPopEnemyNum--;
    }
}
