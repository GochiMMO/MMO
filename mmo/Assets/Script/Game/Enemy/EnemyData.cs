using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]          //PhotonViewを使う
[RequireComponent(typeof(CapsuleCollider))]     //カプセルコライダーを使う
public class EnemyData : Photon.MonoBehaviour {
    static Entity_Sahagin.Param enemyData = null;

    Animator anim;

    enum Status
    {
        NORMAL,     // 普通
        ATTACK,     // 攻撃
        DAMEGE,     // 被弾
        DISCOVER,   // プレイヤー発見
        DEAD        // 死亡
    };

    int HP;             // HP
    int attack;         // 攻撃力
    int defense;        // 防御力
    int magicAttack;    // 魔法攻撃力
    int magicDefense;   // 魔法防御力
    float moveSpeed;    // 移動速度


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

    Status enemyStatus = Status.NORMAL;     // 最初の状態は普通状態
    GameObject scripts;     // スクリプトコンポーネント

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
        // 既定時間が来たら
        if (Time.time - lastActionTime >= actionInterval)
        {
            // 最後にアクションした時間を登録する
            lastActionTime = Time.time;
            // 移動速度を登録する
            moveValue.z = moveSpeed;
            // 歩くモーションに切り替える
            anim.SetBool("walkFlag", true);
            // ランダムで移動する角度を変更する
            RotateDirectionForRandom();
        }
        // 移動させる
        transform.Translate(moveValue * Time.deltaTime);
        // 回転させる
        AttachRotation();
    }

    /// <summary>
    /// ランダムで移動方向を変更する
    /// </summary>
    void RotateDirectionForRandom(){
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
                case 8:
                    moveValue.z = 0f;
                    anim.SetBool("walkFlag", false);
                    break;
                default:
                    
                    break;
            }
    }

    /// <summary>
    /// ノーマル状態の向くべき方向
    /// </summary>
    void AttachRotation()
    {   
        // 回転度を計算する
        newRotation.x = 0f;
        newRotation.z = 0f;
        // 回転度を合わせる
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime*2);
    }

    /// <summary>
    /// プレイヤーを追いかける
    /// </summary>
    void Tracking()
    {
        // プレイヤーとの距離を計算する
        var kyori = (transform.position - haightMaxPlayer.transform.position).sqrMagnitude;

        // 指定した距離であれば,止まる
        if (kyori < Distance * Distance)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                anim.SetTrigger("attackFlag");
            }
            CheckView();
        }
        // 指定した距離に到達していない場合
        else
        {
            // 回転度を変更する(プレイヤーの方を向く)
            newRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(haightMaxPlayer.transform.position - transform.position), 1.0f).eulerAngles;
            // x,z方向の回転度を0に変更する
            newRotation.z = 0f;
            newRotation.x = 0f;
            // 回転させる
            transform.rotation = Quaternion.Euler(newRotation);
            // 移動速度を追いかける速度に変更する
            moveValue.z = trackingSpeed;
            // 走ってるアニメーションに変更する
            anim.SetBool("runFlag", true);
            // 移動させる
            transform.Translate(moveValue * Time.deltaTime);
        }
        /*
        // プレイヤーが視覚範囲にいなくなり、距離も離れた場合
        if (!((transform.position - haightMaxPlayer.transform.position).sqrMagnitude < angleDistance * angleDistance
            && Vector3.Angle(haightMaxPlayer.transform.position - transform.position, transform.forward) <= angle)
            && enemyStatus != Status.NORMAL)
        {
            // 走るフラグをオフにする
            anim.SetBool("runFlag", false);
            // 敵のステータスをノーマルに変更する
            enemyStatus = Status.NORMAL;
        }
        */
    }

    /// <summary>
    /// 特定の視野内でのプレイヤー検索用
    /// </summary>
    void CheckView()
    {
        // プレイヤーを発見したら
        if ((haightMaxPlayer.transform.position - transform.position).sqrMagnitude < angleDistance * angleDistance
            && Vector3.Angle(transform.position - haightMaxPlayer.transform.position, transform.forward) <= angle)
        {
            // 回転度を計算する
            newRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(haightMaxPlayer.transform.position - transform.position), 1.0f).eulerAngles;
            newRotation.x = 0f;
            newRotation.z = 0f;
            // 回転を変える
            transform.rotation = Quaternion.Euler(newRotation);
            // 敵のステータスを発見状態に変える
            enemyStatus = Status.DISCOVER;
        }
        // プレイヤーを発見していない時
        else {
            // ステータスをノーマルに変更する
            enemyStatus = Status.NORMAL;
            // 歩くアニメーションに変更する
            anim.SetBool("runFlag", false);
        }
    }

    /// <summary>
    /// ヘイトの高いプレイヤーを探す
    /// </summary>
    void GetPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        int playerHaight = -9999;

        foreach (var obj in players)    
        {
            //プレイヤー達の中で一番ヘイトが高い奴の情報をもらう
            if (obj.GetComponent<Haight>().GetHaight() > playerHaight)
            {
                playerHaight = obj.GetComponent<Haight>().GetHaight();
                haightMaxPlayer = obj;
            }
        }
    }

    /// <summary>
    /// 自分の頭上にダメージが出る
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="position"></param>
    [PunRPC]
    void DrawDamage(int damage, Vector3 position)
    {
        // ダメージを出すコンポーネントを取得し、ダメージを表示する
        scripts.GetComponent<CreateDamageBillboard>().DrawDamageBillboard(damage, position);
        // HPを減算する
        HP -= damage;
        // HPが0以下(つまり死亡したとき)
        if (HP <= 0)
        {
            // ステータスを志望状態に変更する
            enemyStatus = Status.DEAD;
            // 死亡時アニメーションを再生する
            anim.SetTrigger("dead");
            // 当り判定用オブジェクトをオフにする
            // gameObject.GetComponent<CapsuleCollider>().enabled = false;
            // 死んだので数を減算
            myPopScriptRefarence.DeadEnemy();
        }
    }

    /// <summary>
    /// 魔法によるダメージ
    /// </summary>
    /// <param name="damage"></param>
    [PunRPC]
    void HitCliantMagic(int damage)
    {
        // 全員にダメージを表示するRPCを飛ばす
        GetComponent<PhotonView>().RPC("DrawDamage", PhotonTargets.All, damage, transform.position);
    }

    // 何かとぶつかったとき
     void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Magic" && enemyStatus != Status.DEAD)  //当たった物体が魔法ならば
        {
            //マスタークライアントならば
            if (PhotonNetwork.isMasterClient)
            {
                int damage = col.gameObject.GetComponent<MagicBase>().GetAttack();  //攻撃力をゲットする
                GetComponent<PhotonView>().RPC("DrawDamage", PhotonTargets.All, damage, transform.position);    //ダメージを表示する

            }
            //他のクライアントならば
            else
            {
                // 攻撃力を取得する
                int damage = col.gameObject.GetComponent<MagicBase>().GetAttack();
                // ダメージを送信する(マスタークライアントに送信し、そこから全員に送信するようにする)
                GetComponent<PhotonView>().RPC("HitCliantMagic", PhotonTargets.MasterClient, damage);
            }
        }
    }

    void Start()
    {
        GetPlayer();
        // moveValue = new Vector3();  //ｉｎｓｔａｎｃｅ作成
        scripts = GameObject.FindGameObjectWithTag("Scripts");
        if (enemyData == null)
        {
            enemyData = Resources.Load<Entity_Sahagin>("Enemy/EnemyData/EnemyData").sheets[0].list[0];
        }
        anim = gameObject.GetComponent<Animator>();
        // ステータスの設定
        this.HP = enemyData.HP;
        this.magicAttack = enemyData.MagicAttack;
        this.magicDefense = enemyData.MagicDefense;
        this.moveSpeed = enemyData.MoveSpeed;
        this.attack = enemyData.Attack;
        this.defense = enemyData.Defense;
    }

    void Update()
    {
        // マスタークライアントなら
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log(enemyStatus);     // デバッグ用敵の状態表示
            Debug.Log(anim.GetCurrentAnimatorStateInfo(0));
            // 状態によって変化する
            switch (enemyStatus)
            {
                case Status.NORMAL:
                    Move();         // 移動処理
                    CheckView();    // プレイヤーを探す処理
                    break;
                case Status.ATTACK:
                    break;
                case Status.DAMEGE:
                    break;
                    // プレイヤーを発見した時
                case Status.DISCOVER:
                    Tracking(); // 追いかける処理
                    break;
                case Status.DEAD:
                    //何らかのアニメーション
                    //自分を削除
                    
                    //PhotonNetwork.Destroy(this.gameObject);
                    
                    break;
            }
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
