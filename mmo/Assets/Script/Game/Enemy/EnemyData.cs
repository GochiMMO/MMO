using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]          //PhotonViewを使う
[RequireComponent(typeof(CapsuleCollider))]     //カプセルコライダーを使う
[RequireComponent(typeof(Rigidbody))]           // rigidbodyを使う
abstract public class EnemyData : Photon.MonoBehaviour {
    /// <summary>
    /// 通信同期のためのコンポーネント
    /// </summary>
    protected PhotonTransformView photonTransformView;
    /// <summary>
    /// 物理演算を行うためのコンポーネント
    /// </summary>
    protected Rigidbody rigBody;
    /// <summary>
    /// 敵の元データの参照
    /// </summary>
    protected static Entity_Sahagin enemyResourceData = null;
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
    /// 回転するスピード(基本１秒で回転する)
    /// </summary>
    protected float rotateSpeed = 1f;
    /// <summary>
    /// 敵のステータス判別用列挙体
    /// </summary>
    protected enum Status
    {
        NORMAL,     // 普通
        ATTACK,     // 攻撃
        DAMEGE,     // 被弾
        DISCOVER,   // プレイヤー発見
        INTIMIDATION,   // 威嚇
        DEAD,        // 死亡
        OTHER           // その他の処理
    };
    /// <summary>
    /// 体力
    /// </summary>
    protected int HP;
    /// <summary>
    /// 攻撃力
    /// </summary>
    protected int attack { private set; get; }
    /// <summary>
    /// 防御力
    /// </summary>
    protected int defense { private set; get; }
    /// <summary>
    /// 魔法攻撃力
    /// </summary>
    protected int magicAttack { private set; get; }
    /// <summary>
    /// 魔法防御力
    /// </summary>
    protected int magicDefense { private set; get; }
    /// <summary>
    /// 移動速度
    /// </summary>
    protected float moveSpeed { private set; get; }
    /// <summary>
    /// プレイヤー発見時、追跡する速度
    /// </summary>
    protected float trackingSpeed{ private set; get; }
    /// <summary>
    /// 敵が行動を変更する間隔（秒）
    /// </summary>
    protected float actionInterval { private set; get; }
    /// <summary>
    /// 敵がプレイヤーを発見できる視野角（度）
    /// </summary>
    protected float angle { private set; get; }
    /// <summary>
    /// 敵がプレイヤーを発見できる距離（メートル）
    /// </summary>
    protected float angleDistance { private set; get; }
    /// <summary>
    /// 敵が何らかのアクションを行う距離
    /// </summary>
    protected float actionDistance { private set; get; }
    /// <summary>
    /// 最後に行動が変わった時間
    /// </summary>
    protected float lastActionTime;
    /// <summary>
    /// 実際に移動する速度
    /// </summary>
    protected Vector3 moveValue;
    /// <summary>
    /// 前フレーム移動速度
    /// </summary>
    protected Vector3 pastMoveValue;
    /// <summary>
    /// 回転する角度
    /// </summary>
    protected Vector3 newRotation;
    /// <summary>
    /// 前フレームの回転角度
    /// </summary>
    protected Vector3 pastRotation;
    /// <summary>
    /// プレイヤーの配列
    /// </summary>
    protected GameObject[] players { private set; get; }
    /// <summary>
    /// ヘイトを一番稼いでるプレイヤーのオブジェクトを格納する変数
    /// </summary>
    protected GameObject haightMaxPlayer { private set; get; }
    /// <summary>
    /// 敵のステータス(状態遷移用)
    /// </summary>
    protected Status enemyStatus = Status.NORMAL;     // 最初の状態は普通状態
    /// <summary>
    /// スクリプト用コンポーネントを格納しておくオブジェクト
    /// </summary>
    private GameObject scripts;
    /// <summary>
    /// （敵にとって）自分が出現管理されているスクリプトの参照、インスペクターには表示しない
    /// </summary>
    [HideInInspector]
    public PopEnemy myPopScriptRefarence;

    /// <summary>
    /// 攻撃を受けた時の処理
    /// </summary>
    /// <param name="col">当たってきたオブジェクトのコライダー</param>
    /// <param name="damage">ダメージの値</param>
    virtual protected void SufferDamageAction(Collider col, int damage){}

    /// <summary>
    /// 名前を設定する
    /// </summary>
    abstract protected void SetName();

    /// <summary>
    /// ヘイトの高いプレイヤーを探す
    /// </summary>
    protected void GetHaightHighestPlayer()
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
    virtual protected void Start()
    {
        // 敵の元データが無いならば
        if (enemyResourceData == null)
        {
            // 敵の元データを読み込む
            enemyResourceData = Resources.Load<Entity_Sahagin>("Enemy/EnemyData/EnemyData");
        }
        // 敵の名前を設定する(継承先で設定する)
        SetName();
        // 敵のデータを読み込む
        LoadEnemyData();
        // ステータスを設定する
        if (!SetStatus())
        {
            // エラー文を表示
            Debug.LogError(enemyName + "のステータスは設定されていません。");
            // ネットワークに接続されていたら
            if (PhotonNetwork.connected)
            {
                // ネットワーク上から削除する
                PhotonNetwork.Destroy(this.gameObject);
            }
            // ネットワークに接続されていなければ
            else
            {
                // ローカル上で削除する
                GameObject.Destroy(this.gameObject);
            }
            // 処理から抜ける
            return;
        }
        // スクリプトコンポーネントのあるオブジェクトを格納する
        scripts = GameObject.FindGameObjectWithTag("Scripts");
        // アニメーションコンポーネントを取得する
        anim = gameObject.GetComponent<Animator>();
        // 通信同期のためのコンポーネントを取得する
        photonTransformView = gameObject.GetComponent<PhotonTransformView>();
        // 物理演算の為のコンポーネントを取得する
        rigBody = gameObject.GetComponent<Rigidbody>();
        // マスタークライアントならば
        if (PhotonNetwork.isMasterClient)
        {
            // 移動速度を設定する
            moveValue.z = moveSpeed;
            // 通信同期のための移動速度を送る
            photonTransformView.SetSynchronizedValues(moveValue, 0f);
        }
        // ヘイトが一番高いプレイヤーを探す
        GetHaightHighestPlayer();
    }

    /// <summary>
    /// 攻撃の終了(アニメーションからコールされる)
    /// </summary>
    virtual public void EndOfAttack() { }

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
            this.angle = enemyData.FieldOfView / 2;
            this.angleDistance = enemyData.ViewDistance * enemyData.ViewDistance;
            this.actionDistance = enemyData.ActionDistance * enemyData.ActionDistance;
            // 設定されたことを返す
            return true;
        }
        // 設定できなければfalseを返す
        return false;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    abstract protected void Update();
}