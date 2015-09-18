using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]          //PhotonViewを使う
[RequireComponent(typeof(CapsuleCollider))]     //カプセルコライダーを使う

public class EnemyData : Photon.MonoBehaviour {
    enum States
    {
        NORMAL,
        DEAD
    };
    

    [SerializeField, Tooltip("HP")]
    int HP;
    [SerializeField, Tooltip("攻撃力")]
    int attack;

    States enemyStates = States.NORMAL;
    GameObject scripts;

    public PopEnemy myPopScriptRefarence;  //（敵にとって）自分が出現管理されているスクリプトの参照

    [PunRPC]
    void DrawDamage(int damage, Vector3 position)
    {
        scripts.GetComponent<CreateDamageBillboard>().DrawDamageBillboard(damage, position);
        HP -= damage;
    }

    [PunRPC]
    void HitCliantMagic(int damage)
    {
        GetComponent<PhotonView>().RPC("DrawDamage", PhotonTargets.All, damage, transform.position);
    }

    //何かとぶつかったとき
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Magic")  //当たった物体が魔法ならば
        {
            //マスタークライアントならば
            if (PhotonNetwork.isMasterClient)
            {
                int damage = col.gameObject.GetComponent<MagicBase>().GetAttack();  //攻撃力をゲットする
                GetComponent<PhotonView>().RPC("DrawDamage", PhotonTargets.All, damage, transform.position);    //ダメージを表示する
                if (HP <= 0)
                {
                    enemyStates = States.DEAD;
                }
            }
            //他のクライアントならば
            else
            {
                int damage = col.gameObject.GetComponent<MagicBase>().GetAttack();
                //GetComponent<PhotonView>().RPC("HitCliantMagic", PhotonTargets.MasterClient, damage);
                GetComponent<PhotonView>().RPC("DrawDamage", PhotonTargets.All, damage, transform.position);
            }
        }
    }

    void Start()
    {
        scripts = GameObject.FindGameObjectWithTag("Scripts");
    }

    void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            switch (enemyStates)
            {
                case States.DEAD:
                    //何らかのアニメーション

                    //自分を削除
                    PhotonNetwork.Destroy(this.gameObject);
                    myPopScriptRefarence.DeadEnemy();   //死んだので数を減算
                    break;
            }
        }
    }

    //状態の同期
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(enemyStates);   //ステータスを送信
        }
        else
        {
            enemyStates = (States)stream.ReceiveNext(); //ステータスを受信
        }
    }
}
