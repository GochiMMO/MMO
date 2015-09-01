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
    }

    //何かとぶつかったとき
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Magic")  //当たった物体が魔法ならば
        {
            //if (col.gameObject.GetComponent<PhotonView>().isMine)
            {
                int damage = col.gameObject.GetComponent<MagicBase>().GetAttack();
                GetComponent<PhotonView>().RPC("DrawDamage", PhotonTargets.All, damage, transform.position);
                HP -= damage;
                if (HP <= 0)
                {
                    enemyStates = States.DEAD;
                }
            }
        }
    }

    void Start()
    {
        scripts = GameObject.FindGameObjectWithTag("Scripts");
    }

    void Update()
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
