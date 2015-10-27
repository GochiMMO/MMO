using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]          //PhotonViewを使う
[RequireComponent(typeof(CapsuleCollider))]     //カプセルコライダーを使う
abstract public class EnemyData : Photon.MonoBehaviour {
    /// <summary>
    /// 敵の元データの参照
    /// </summary>
    private static Entity_Sahagin enemyResourceData = null;
    /// <summary>
    /// 敵のステータスデータ
    /// </summary>
    protected Entity_Sahagin.Param enemyData = null;
    /// <summary>
    /// 敵の名前
    /// </summary>
    protected string enemyName;
    /// <summary>
    /// アニメーションコンポーネント
    /// </summary>
    protected Animator anim;
    /// <summary>
    /// 敵のステータス判別用列挙体
    /// </summary>
    protected enum Status
    {
        NORMAL,     // 普通
        ATTACK,     // 攻撃
        DAMEGE,     // 被弾
        DISCOVER,   // プレイヤー発見
        DEAD        // 死亡
    };
    /// <summary>
    /// 体力
    /// </summary>
    private int HP;
    /// <summary>
    /// 攻撃力
    /// </summary>
    private int attack;
    /// <summary>
    /// 防御力
    /// </summary>
    private int defense;
    /// <summary>
    /// 魔法攻撃力
    /// </summary>
    private int magicAttack;
    /// <summary>
    /// 魔法防御力
    /// </summary>
    private int magicDefense;
    /// <summary>
    /// 移動速度
    /// </summary>
    private float moveSpeed;
    /// <summary>
    /// プレイヤー発見時、追跡する速度
    /// </summary>
    private float trackingSpeed;
    /// <summary>
    /// 敵が行動を変更する間隔（秒）
    /// </summary>
    private float actionInterval;
    /// <summary>
    /// 敵がプレイヤーを発見できる視野角（度）
    /// </summary>
    private float angle;
    /// <summary>
    /// 敵がプレイヤーを発見できる距離（メートル）
    /// </summary>
    private float angleDistance;
    /// <summary>
    /// 敵が何らかのアクションを行う距離
    /// </summary>
    private float actionDistance;
    /// <summary>
    /// 最後に行動が変わった時間
    /// </summary>
    private float lastActionTime;
    /// <summary>
    /// 実際に移動する速度
    /// </summary>
    protected Vector3 moveValue;
    /// <summary>
    /// 回転する角度
    /// </summary>
    protected Vector3 newRotation;
    /// <summary>
    /// １つ前のフレームの位置
    /// </summary>
    protected Vector3 pastPosition;  //過去位置
    /// <summary>
    /// プレイヤーの配列
    /// </summary>
    private GameObject[] players;
    /// <summary>
    /// ヘイトを一番稼いでるプレイヤーのオブジェクトを格納する変数
    /// </summary>
    protected GameObject haightMaxPlayer;
    /// <summary>
    /// 敵のステータス(状態遷移用)
    /// </summary>
    protected Status enemyStatus = Status.NORMAL;     // 最初の状態は普通状態
    /// <summary>
    /// スクリプト用コンポーネントを格納しておくオブジェクト
    /// </summary>
    private GameObject scripts;     // スクリプトコンポーネント
    /// <summary>
    /// //（敵にとって）自分が出現管理されているスクリプトの参照、インスペクターには表示しない
    /// </summary>
    [HideInInspector]
    public PopEnemy myPopScriptRefarence;

    

    /// <summary>
    /// 移動方向を変更する処理（仮想関数）
    /// </summary>
    abstract protected void RotateDirection();

    /// <summary>
    /// 既定距離に達した時の行動
    /// </summary>
    abstract protected void NearPlayerAction();

    /// <summary>
    /// ダメージを食らった時の処理
    /// </summary>
    /// <param name="col">当たったコライダー</param>
    abstract protected void SufferDamageAction(Collider col, int damage);

    /// <summary>
    /// 敵の名前を設定する
    /// </summary>
    abstract protected void SetEnemyName();

    /// <summary>
    /// 攻撃を受けている時の処理
    /// </summary>
    abstract protected void SufferingDamage();

    /// <summary>
    /// ダメージを食らっている処理が終わったかどうか
    /// </summary>
    /// <returns>終わったらtrue、終わっていなければfalse</returns>
    abstract protected bool IsEndDamage();

    /// <summary>
    /// 攻撃が終了する処理
    /// </summary>
    /// <returns>終わったらtrue、終わってなければfalse</returns>
    abstract protected bool IsEndAttack();

    /// <summary>
    /// 攻撃中の処理
    /// </summary>
    abstract protected void Attacking();

    /// <summary>
    /// ノーマル状態の向くべき方向
    /// </summary>
    private void AttachRotation()
    {   
        // x,z軸の回転を0にする
        newRotation.x = 0f;
        newRotation.z = 0f;
        // 回転させる(既定角に0.5秒で回転する)
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime*2);
    }

    /// <summary>
    /// ノーマル状態の移動処理
    /// </summary>
    private void Move()
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
            RotateDirection();
            // ヘイト値が最大のプレイヤーを登録する
            GetHaightHighestPlayer();
        }
        // 移動させる
        transform.Translate(moveValue * Time.deltaTime);
        // 回転させる
        AttachRotation();
    }

    /// <summary>
    /// プレイヤーを追いかける
    /// </summary>
    private void Tracking()
    {
        // プレイヤーとの距離を計算する
        float kyori = (transform.position - haightMaxPlayer.transform.position).sqrMagnitude;

        // 指定した距離に到達したかどうか
        if (kyori < actionDistance * actionDistance)
        {
            // プレイヤーが近くに来た時の処理を行う
            NearPlayerAction();
        }
        // 指定した距離に到達していない場合
        else
        {
            // 移動速度を追いかける速度に変更する
            moveValue.z = trackingSpeed;
            // 走ってるアニメーションに変更する
            anim.SetBool("runFlag", true);
            // 移動させる
            transform.Translate(moveValue * Time.deltaTime);
        }
    }

    /// <summary>
    /// 特定の視野内でのプレイヤー検索用
    /// </summary>
    private bool CheckView()
    {
        // プレイヤーを発見したら
        if ((haightMaxPlayer.transform.position - transform.position).sqrMagnitude < angleDistance * angleDistance      // 距離計算
            && Vector3.Angle(transform.position - haightMaxPlayer.transform.position, transform.forward) <= angle)      // 角度計算
        {
            // 回転度を計算する
            newRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(haightMaxPlayer.transform.position - transform.position), 1.0f).eulerAngles;
            newRotation.x = 0f;
            newRotation.z = 0f;
            // 回転を変える
            transform.rotation = Quaternion.Euler(newRotation);
            // 敵のステータスを発見状態に変える
            enemyStatus = Status.DISCOVER;
            // 発見できているのでtrueを返す
            return true;
        }
        // プレイヤーを発見していない時
        else {
            // ステータスをノーマルに変更する
            enemyStatus = Status.NORMAL;
            // 歩くアニメーションに変更する
            anim.SetBool("runFlag", false);
            // 発見できなかったのでfalseを返す
            return false;
        }
    }

    /// <summary>
    /// ヘイトの高いプレイヤーを探す
    /// </summary>
    private void GetHaightHighestPlayer()
    {
        // "Player"とタグ付けされているオブジェクトを取得する
        players = GameObject.FindGameObjectsWithTag("Player");
        // プレイヤーのヘイトの値を格納しておく
        int playerHaight = -9999;
        // プレイヤーの分だけ繰り返す
        foreach (var obj in players) 
        {
            // プレイヤーからヘイトを取得する
            int haight = obj.GetComponent<Haight>().GetHaight();
            // 格納したヘイトが一時変数のヘイトを上回ったら
            if (haight > playerHaight)
            {
                // ヘイトの値を格納する
                playerHaight = haight;
                // プレイヤーの参照を格納する
                haightMaxPlayer = obj;
            }
        }
    }

    /// <summary>
    /// 自分の頭上にダメージが出る
    /// </summary>
    /// <param name="damage">出すダメージの値</param>
    [PunRPC]
    public void DrawDamage(int damage)
    {
        // ダメージを出すコンポーネントを取得し、ダメージを表示する
        scripts.GetComponent<CreateDamageBillboard>().DrawDamageBillboard(damage, transform.position);
        // HPを減算する
        HP -= damage;
        // HPが0以下(つまり死亡したとき)
        if (HP <= 0)
        {
            // ステータスを死亡状態に変更する
            enemyStatus = Status.DEAD;
            // 死亡時アニメーションを再生する
            anim.SetTrigger("dead");
            // 死んだので数を減算
            myPopScriptRefarence.DeadEnemy();
        }
    }

    /// <summary>
    /// 何かのオブジェクトとぶつかったとき
    /// </summary>
    /// <param name="col">当たってきたコライダー</param>
    private void OnTriggerEnter(Collider col)
    {
        // 敵が死亡状態でないとき
        if (enemyStatus != Status.DEAD)
        {
            // そのコライダーが自分が出したものならば
            if (col.gameObject.GetComponent<PhotonView>().isMine)
            {
                // 当たった物体が魔法ならば
                if (col.gameObject.tag == "Magic")
                {
                    // 攻撃力をゲットする
                    int damage = col.gameObject.GetComponent<MagicBase>().GetAttack();
                    // 防御力を加味して計算する
                    damage -= magicDefense;
                    // もしダメージが0を下回ったら
                    if (damage < 0)
                    {
                        // ノーダメージにする
                        damage = 0;
                    }
                    // ダメージを表示させる、減算させるなどの処理を行う
                    GetComponent<PhotonView>().RPC("DrawDamage", PhotonTargets.All, damage);
                    // 攻撃を食らった時の処理を行う
                    SufferDamageAction(col, damage);
                }
            }
        }
    }

    /// <summary>
    /// スクリプトが開始した時の処理
    /// </summary>
    private void Start()
    {
        // 敵の元データが無いならば
        if (enemyResourceData == null)
        {
            // 敵の元データを読み込む
            enemyResourceData = Resources.Load<Entity_Sahagin>("Enemy/EnemyData/EnemyData");
        }
        // 敵の名前を設定する(継承先で設定する)
        SetEnemyName();
        // 敵のデータを読み込む
        LoadEnemyData();
        // ステータスを設定する
        if (!SetStatus())
        {
            // ステータスを設定できなければ自身を削除する
            GameObject.Destroy(this.gameObject);
            return;
        }
        // スクリプトコンポーネントのあるオブジェクトを格納する
        scripts = GameObject.FindGameObjectWithTag("Scripts");
        // アニメーションコンポーネントを取得する
        anim = gameObject.GetComponent<Animator>();
        // ヘイトが一番高いプレイヤーを探す
        GetHaightHighestPlayer();
    }

    /// <summary>
    /// 敵の名前からデータを設定する
    /// </summary>
    private void LoadEnemyData()
    {
        // シートを読み込む
        foreach (Entity_Sahagin.Sheet sheets in enemyResourceData.sheets)
        {
            // 敵の配列を読み込む
            foreach (Entity_Sahagin.Param param in sheets.list)
            {
                // 設定された名前の敵のデータを呼び出す
                if (param.Name == enemyName)
                {
                    // 敵のデータを登録する
                    this.enemyData = param;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// ステータスの設定
    /// </summary>
    private bool SetStatus()
    {
        // 敵のデータが登録されていれば
        if (enemyData != null)
        {
            // ステータスの設定
            this.HP = enemyData.HP;
            this.magicAttack = enemyData.MagicAttack;
            this.magicDefense = enemyData.MagicDefense;
            this.moveSpeed = enemyData.MoveSpeed;
            this.attack = enemyData.Attack;
            this.defense = enemyData.Defense;
            this.trackingSpeed = enemyData.TrackingSpeed;
            this.actionInterval = enemyData.ActionInterval;
            this.angle = enemyData.FieldOfView;
            this.angleDistance = enemyData.ViewDistance;
            this.actionDistance = enemyData.ActionDistance;
            // 設定されたことを返す
            return true;
        }
        // 設定できなければfalseを返す
        return false;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        // マスタークライアントなら
        if (PhotonNetwork.isMasterClient)
        {
            // 状態によって変化する
            switch (enemyStatus)
            {
                case Status.NORMAL:
                    Move();         // 移動処理
                    CheckView();    // プレイヤーを探す処理
                    break;
                case Status.ATTACK:
                    // 攻撃が終了する処理
                    if (IsEndAttack())
                    {
                        Debug.Log("攻撃終了");
                        // 攻撃する処理を行わない
                        break;
                    }
                    // 攻撃中の処理
                    Attacking();
                    break;
                    // ダメージを食らった時の処理
                case Status.DAMEGE:
                    if (IsEndDamage())
                    {
                        // ダメージを食らう処理が終わったら
                        break;
                    }
                    // ダメージを食らっている時の処理
                    SufferingDamage();
                    break;
                    // プレイヤーを発見した時
                case Status.DISCOVER:
                    // プレイヤーを探し、発見したらtrue、発見できなかったらfalse
                    if (!CheckView())
                    {
                        break;
                    }
                    // 追いかける処理
                    Tracking();
                    break;
                case Status.DEAD:
                    break;
            }
        }
    }
}