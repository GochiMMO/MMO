using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]          //PhotonViewを使う
[RequireComponent(typeof(CapsuleCollider))]     //カプセルコライダーを使う

public class EnemyData : Photon.MonoBehaviour {
    enum States
    {
        NORMAL,
        ATTACK,
        DAMEGE,
        DEAD
    };

    enum testStates //デバック用ステータス
    {
        tNormal,
        tAttack,
        tDamege,
        tDiscover,
        tDead
    };

    [SerializeField, Tooltip("HP")]
    int HP;
    [SerializeField, Tooltip("攻撃力")]
    int attack;
    [SerializeField, Tooltip("物理防御力")]
    int defense;
    [SerializeField, Tooltip("魔法攻撃力")]
    int magicAttack;
    [SerializeField, Tooltip("魔法防御力")]
    int magicDefense;
    [SerializeField, Tooltip("移動速度")]
    float moveSpeed;
    [SerializeField, Tooltip("追跡用速度")]
    float trackingSpeed;
    [SerializeField, Tooltip("敵行動間隔(秒)")]
    float actionInterval;
    [SerializeField, Tooltip("敵視野（℃）")]
    float angle;
    [SerializeField, Tooltip("視野距離")]
    float angleDistance;
    [SerializeField, Tooltip("接近距離")]
    float Distance;

    float lastActionTime; //行動間隔用
    Vector3 moveValue;    //移動用
    Vector3 newRotation;  //回転用
    Vector3 pastPosition;  //過去位置

    GameObject[] players; //Player情報取得用
    GameObject haightMaxPlayer; //ヘイト用

    //States enemyStates = States.NORMAL;
    testStates testEnmStates = testStates.tNormal;
    GameObject scripts;

    public PopEnemy myPopScriptRefarence;  //（敵にとって）自分が出現管理されているスクリプトの参照

    void Action()
    {
        //ここに攻撃パターンを書く
    }

    /// <summary>
    /// ノーマル状態の移動処理
    /// </summary>
    void Move()
    {
        if (Time.time - lastActionTime >= actionInterval)
        {
            lastActionTime = Time.time;
            moveValue.z = moveSpeed;
            switch (Random.Range(0, 28))
            {
                case 0:
                    newRotation.y = 0f;
                    break;
                case 1:
                    newRotation.y = 180f;
                    break;
                case 2:
                    newRotation.y = 90f;
                    break;
                case 3:
                    newRotation.y = 270f;
                    break;
                case 4:
                    newRotation.y = 135f;
                    break;
                case 5:
                    newRotation.y = 225f;
                    break;
                case 6:
                    newRotation.y = 45f;
                    break;
                case 7:
                    newRotation.y = 315f;
                    break;
                default:
                    break;
            }
            
            //Debug.Log(newRotation);
        }
        transform.Translate(moveValue * Time.deltaTime);
        AttachRotation();
    }

    /// <summary>
    /// ノーマル状態の向くべき方向
    /// </summary>
    void AttachRotation()
    {   
        newRotation.x = 0f;
        newRotation.z = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime*2);
    }

    /// <summary>
    /// プレイヤーを追いかける
    /// </summary>
    void Tracking()
    {
        var kyori = Vector3.Distance(haightMaxPlayer.transform.position, transform.position);

        if (kyori < Distance) //指定した距離であれば,止まる
        {
            CheckView();
            //testEnmStates = testStates.tAttack;
        }
        else
        {
            newRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(haightMaxPlayer.transform.position - transform.position), 1.0f).eulerAngles;
            newRotation.x = 0f;
            transform.rotation = Quaternion.Euler(newRotation);
            moveValue.z = trackingSpeed;
            transform.Translate(moveValue * Time.deltaTime);
        }

        if ((transform.position - haightMaxPlayer.transform.position).sqrMagnitude < angleDistance
            && Vector3.Angle(haightMaxPlayer.transform.position - transform.position, transform.forward) <= angle){}
        else
        {
            if(testEnmStates != testStates.tNormal ) { testEnmStates = testStates.tNormal; }
        }
    }

    /// <summary>
    /// 特定の視野内でのプレイヤー検索用
    /// </summary>
    void CheckView()
    {
        if ((transform.position - haightMaxPlayer.transform.position).sqrMagnitude < angleDistance && Vector3.Angle(haightMaxPlayer.transform.position - transform.position, transform.forward) <= angle)
        {
            if (testEnmStates == testStates.tNormal) { testEnmStates = testStates.tDiscover; } //デバッグ用 ステータスがNormalである時だけ
            newRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(haightMaxPlayer.transform.position - transform.position), 1.0f).eulerAngles;
            newRotation.x = 0f;
            transform.rotation = Quaternion.Euler(newRotation);
        }
        else {
            testEnmStates = testStates.tNormal;
        }
    }

    /// <summary>
    /// ヘイトの高いプレイヤーを探す
    /// </summary>
    void GetPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        int playerHaight = -9999;

        foreach (var obj in players)    //プレイヤー達の中で一番ヘイトが高い奴の情報をもらう
        {
            if (obj.GetComponent<Haight>().GetHaight() > playerHaight)
            {
                playerHaight = obj.GetComponent<Haight>().GetHaight();
                haightMaxPlayer = obj;
            }
        }
    }

    /// <summary>
    /// 自分の頭上にダメージが出ウ？
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="position"></param>
    [PunRPC]
    void DrawDamage(int damage, Vector3 position)
    {
        scripts.GetComponent<CreateDamageBillboard>().DrawDamageBillboard(damage, position);
        HP -= damage;
    }

    /// <summary>
    /// 魔法によるダメージ
    /// </summary>
    /// <param name="damage"></param>
    [PunRPC]
    void HitCliantMagic(int damage)
    {
        GetComponent<PhotonView>().RPC("DrawDamage", PhotonTargets.All, damage, transform.position);
    }

    //何かとぶつかったとき
    // void OnTriggerEnter(Collider col)
    //{
    //    if (col.gameObject.tag == "Magic")  //当たった物体が魔法ならば
    //    {
    //        //マスタークライアントならば
    //        if (PhotonNetwork.isMasterClient)
    //        {
    //            int damage = col.gameObject.GetComponent<MagicBase>().GetAttack();  //攻撃力をゲットする
    //            GetComponent<PhotonView>().RPC("DrawDamage", PhotonTargets.All, damage, transform.position);    //ダメージを表示する
    //            if (HP <= 0)
    //            {
    //                enemyStates = States.DEAD;
    //            }
    //        }
    //        //他のクライアントならば
    //        else
    //        {
    //            int damage = col.gameObject.GetComponent<MagicBase>().GetAttack();
    //            GetComponent<PhotonView>().RPC("HitCliantMagic", PhotonTargets.MasterClient, damage);
    //        }
    //    }
    //}

    void Start()
    {
        GetPlayer();
        moveValue = new Vector3();  //ｉｎｓｔａｎｃｅ作成
        scripts = GameObject.FindGameObjectWithTag("Scripts");
    }

    void Update()
    {
        //if (PhotonNetwork.isMasterClient)
        //{
        //    switch (enemyStates)
        //    {
        //        case States.NORMAL:
        //            Move();
        //            break;
        //        case States.ATTACK:
        //            break;
        //        case States.DAMEGE:
        //            break;
        //        case States.DEAD:
        //            //何らかのアニメーション
        //            //自分を削除
        //            PhotonNetwork.Destroy(this.gameObject);
        //            myPopScriptRefarence.DeadEnemy();   //死んだので数を減算
        //            break;
        //    }
        //}

        switch (testEnmStates)  //ネットを使わないデバック用
        {
            case testStates.tNormal:
                Move();
                CheckView();
                break;
            case testStates.tAttack:
                break;
            case testStates.tDamege:
                break;
            case testStates.tDiscover:
                Tracking();
                break;
            case testStates.tDead:
                break;
            default:
                break;
        }
    }

//    //状態の同期
//    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        if (stream.isWriting)
//        {
//            stream.SendNext(enemyStates);   //ステータスを送信
//        }
//        else
//        {
//            enemyStates = (States)stream.ReceiveNext(); //ステータスを受信
//        }
//    }
}
