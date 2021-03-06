﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]          // PhotonViewを使う
[RequireComponent(typeof(CapsuleCollider))]     // カプセルコライダーを使う
[RequireComponent(typeof(Rigidbody))]           // rigidbodyを使う
abstract public class EnemyData : Photon.MonoBehaviour {
    /// <summary>
    /// 攻撃の当たり判定を行うコンポーネント
    /// </summary>
    [SerializeField, Tooltip("攻撃用当り判定コンポーネント")]
    EnemyAttack[] enemyAttacks;
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
    /// レベル
    /// </summary>
    protected int level;
    /// <summary>
    /// レベル設定用プロパティ
    /// </summary>
    public int Level
    {
        // レベルを調整してメンバ変数に設定する
        set
        {
            // 100を超えていたときは
            if (value > 100)
            {
                // 100に調整する
                value = 100;
            }
            // 1を下回っていたときは
            else if (value < 1)
            {
                // 1に調整する
                value = 1;
            }
            // レベルを設定する
            level = value;
        }
        // レベルを返す
        get { return level; }
    }

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
    /// 獲得経験値
    /// </summary>
    protected int exp;
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
    /// ヘイトを格納する配列
    /// </summary>
    protected int[] playersHates;
    /// <summary>
    /// ヘイトを一番稼いでるプレイヤーのオブジェクトを格納する変数
    /// </summary>
    protected GameObject hateMaxPlayer { private set; get; }
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
        int playerHate = -9999;
        // ヘイトの配列を設定する
        playersHates = new int[players.Length];

        // プレイヤーの分だけ繰り返す
        for (int i = 0; i < players.Length; i++ )
        {
            // プレイヤーからヘイトを取得する
            int hate = players[i].GetComponent<PlayerChar>().Hate;
            // ヘイトの値を登録する
            playersHates[i] = hate;
            // 格納したヘイトが一時変数のヘイトを上回ったら
            if (hate > playerHate)
            {
                // ヘイトの値を格納する
                playerHate = hate;
                // プレイヤーの参照を格納する
                hateMaxPlayer = players[i];
            }
        }
    }

    /// <summary>
    /// 自分の頭上にダメージが出る
    /// </summary>
    /// <param name="damage">出すダメージの値</param>
    [PunRPC]
    public void DrawDamage(int damage, PhotonMessageInfo info)
    {
        // ダメージを出すコンポーネントを取得し、ダメージを表示する
        scripts.GetComponent<CreateDamageBillboard>().DrawDamageBillboard(damage, transform.position);
        // マスタークライアントならば
        if (PhotonNetwork.isMasterClient)
        {
            // HPを減算する
            HP -= damage;
            // HPを他のプレイヤーに飛ばして同期させる
            // photonView.RPC("SetHP", PhotonTargets.Others, HP);
            // HPが0以下(つまり死亡したとき)
            if (HP <= 0)
            {
                // 倒したプレイヤーに経験値を加算させる処理を行わせる
                // info.photonView.gameObject.GetComponent<PlayerChar>().CallAddExp(this.exp);
                // ステータスを死亡状態に変更する
                enemyStatus = Status.DEAD;
                // 死亡時アニメーションを再生する
                SetTrigger("dead");
                // 死んだので数を減算
                myPopScriptRefarence.DeadEnemy();
            }
        }
    }

    /// <summary>
    /// HPを設定する関数
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="info"></param>
    [PunRPC]
    public void SetHP(int hp, PhotonMessageInfo info)
    {
        // HPを設定する
        this.HP = hp;
    }

    /// <summary>
    /// 攻撃用あたり判定コンポーネントを有効化する
    /// </summary>
    /// <param name="colliderNumber">当り判定コンポーネントの番号</param>
    public void EnableAttackColliders(int colliderNumber = -1)
    {
        // コライダーに指定が無ければ
        if (colliderNumber == -1)
        {
            // コライダーの数だけ繰り返す
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                // 攻撃用コライダーコンポーネントを有効化する
                enemyAttacks[i].col.enabled = true;
            }
        }
        // コライダーの番号に指定があるとき範囲チェックを行い、チェック内だったら
        else if(colliderNumber >= 0 && colliderNumber <= enemyAttacks.Length)
        {
            // 指定番号の攻撃用コンポーネントを有効化する
            enemyAttacks[colliderNumber].col.enabled = true;
        }
    }

    /// <summary>
    /// 攻撃用あたり判定コンポーネントを無効化する
    /// </summary>
    /// <param name="colliderNumber">当り判定コンポーネントの番号</param>
    public void DisableAttackColliders(int colliderNumber = -1)
    {
        // コライダー番号に指定が無ければ
        if (colliderNumber == -1)
        {
            // コライダーの数だけ繰り返す
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                // 攻撃用コライダーコンポーネントを無効化する
                enemyAttacks[i].col.enabled = false;
            }
        }
        // コライダーの番号に指定があるとき範囲チェックを行い、チェック内だったら
        else if (colliderNumber >= 0 && colliderNumber <= enemyAttacks.Length)
        {
            // 指定番号の攻撃用コンポーネントを無効化する
            enemyAttacks[colliderNumber].col.enabled = false;
        }
    }

    /// <summary>
    /// 攻撃力の設定
    /// </summary>
    /// <param name="enemyAttackNumber">コンポーネントの番号(指定無しで全部)</param>
    protected void SetAttack(int enemyAttackNumber = -1)
    {
        // 攻撃用コンポーネント番号の指定が無い場合
        if (enemyAttackNumber == -1)
        {
            // 攻撃用コンポーネントの登録された数だけ繰り返す
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                // 攻撃力を設定する
                enemyAttacks[i].attack = this.attack;
                // 敵の攻撃を物理にする
                enemyAttacks[i].attackKind = EnemyAttack.AttackKind.PHYSICS;
            }
        }
        // コンポーネント番号の指定があり、範囲内ならば
        else if (enemyAttackNumber >= 0 && enemyAttackNumber < enemyAttacks.Length)
        {
            // 攻撃力を設定する
            enemyAttacks[enemyAttackNumber].attack = this.attack;
            // 敵の攻撃を物理にする
            enemyAttacks[enemyAttackNumber].attackKind = EnemyAttack.AttackKind.PHYSICS;
        }
    }

    /// <summary>
    /// 魔法攻撃力の設定
    /// </summary>
    /// <param name="enemyAttackNumber">コンポーネントの番号(指定無しで全部)</param>
    protected void SetMagicAttack(int enemyAttackNumber = -1)
    {
        // 攻撃用コンポーネント番号の指定が無い場合
        if (enemyAttackNumber == -1)
        {
            // 攻撃用コンポーネントの登録された数だけ繰り返す
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                // 攻撃力を設定する
                enemyAttacks[i].attack = this.magicAttack;
                // 攻撃の種類を魔法攻撃とする
                enemyAttacks[i].attackKind = EnemyAttack.AttackKind.MAGIC;
            }
        }
        // コンポーネント番号の指定があり、範囲内ならば
        else if (enemyAttackNumber >= 0 && enemyAttackNumber < enemyAttacks.Length)
        {
            // 攻撃力を設定する
            enemyAttacks[enemyAttackNumber].attack = this.magicAttack;
            // 攻撃の種類を魔法攻撃とする
            enemyAttacks[enemyAttackNumber].attackKind = EnemyAttack.AttackKind.MAGIC;
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
            // PhotonViewを取得する
            PhotonView photon = col.gameObject.GetComponent<PhotonView>();
            // そのコライダーが自分が出したものならば
            if (photon && photon.isMine)
            {
                // 当たった物体がプレイヤーの攻撃ならば
                if (col.gameObject.tag == "PlayerAttack")
                {
                    // プレイヤーの攻撃コンポーネントを取得する
                    PlayerAttack playerAttack = col.gameObject.GetComponent<PlayerAttack>();
                    // 攻撃力をゲットする
                    int damage = playerAttack.GetDamage();
                    // 物理、魔法によって処理分けを行う
                    switch (playerAttack.attackKind)
                    {
                        // 魔法攻撃の場合
                        case PlayerAttack.AttackKind.MAGIC:
                            damage -= magicDefense;
                            break;
                        // 物理攻撃の場合
                        case PlayerAttack.AttackKind.PHYSICS:
                            damage -= defense;
                            break;
                    }

                    // もしダメージが0を下回ったら
                    if (damage < 0)
                    {
                        // ノーダメージにする
                        damage = 0;
                    }
                    
                    // HPが0を下回るならば
                    if (HP - damage < 0)
                    {
                        // そのコライダーを出したプレイヤーに経験値を加算させる
                        playerAttack.parentPlayer.AddExp(exp);
                    }
                    // SPを加算する
                    playerAttack.parentPlayer.SP += playerAttack.sp;
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
        if (!SetStatus() && PhotonNetwork.isMasterClient)
        {
            // エラー文を表示
            Debug.LogError(enemyName + "のステータスが設定されていません。");
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
        // マスタークライアントでなければ
        else if (!PhotonNetwork.isMasterClient)
        {
            // マスタークライアントにレベルと名前を送ってもらう
            RequireLevelAndNameForMaster();
        }
        // スクリプトコンポーネントのあるオブジェクトを格納する
        scripts = GameObject.FindGameObjectWithTag("Scripts");
        // アニメーションコンポーネントを取得する
        anim = gameObject.GetComponent<Animator>();
        // 通信同期のためのコンポーネントを取得する
        photonTransformView = gameObject.GetComponent<PhotonTransformView>();
        // 物理演算の為のコンポーネントを取得する
        rigBody = gameObject.GetComponent<Rigidbody>();
        // 攻撃用あたり判定コンポーネントをオフにする
        DisableAttackColliders();
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
        if (enemyData != null && level > 0)
        {
            // ステータスの設定
            this.HP = enemyData.HP + enemyData.HpRate * level;
            this.magicAttack = enemyData.MagicAttack + enemyData.MAtkRate * level;
            this.magicDefense = enemyData.MagicDefense + enemyData.MDefRate * level;
            this.attack = enemyData.Attack + enemyData.AttackRate * level;
            this.defense = enemyData.Defense + enemyData.DefenseRate * level;
            this.exp = enemyData.BaseExp + enemyData.ExpRate * level;
            this.moveSpeed = enemyData.MoveSpeed;
            this.trackingSpeed = enemyData.TrackingSpeed;
            this.actionInterval = enemyData.ActionInterval;
            this.angle = enemyData.FieldOfView / 2;
            this.angleDistance = enemyData.ViewDistance * enemyData.ViewDistance;
            this.actionDistance = enemyData.ActionDistance * enemyData.ActionDistance;
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                // ダメージの振れ幅を設定する
                enemyAttacks[i].damageRate = enemyData.DamageRate;
            }
                // 設定されたことを返す
                return true;
        }
        // 設定できなければfalseを返す
        return false;
    }

    /// <summary>
    /// マスタークライアントにレベルと名前を送ってもらうよう依頼する関数
    /// </summary>
    void RequireLevelAndNameForMaster()
    {
        // マスタークライアントにレベルと名前の請求を行う
        photonView.RPC("SendLevelAndName", PhotonTargets.MasterClient);
    }

    /// <summary>
    /// レベルと名前をすべてのプレイヤーに送信する処理
    /// </summary>
    [PunRPC]
    public void SendLevelAndName()
    {
        // レベルと敵の名前を送る
        photonView.RPC("ReciveLevelAndName", PhotonTargets.All, level, enemyName);
    }

    /// <summary>
    /// 送られてきたレベルと名前を受ける関数
    /// </summary>
    /// <param name="level">レベル</param>
    /// <param name="name">名前</param>
    [PunRPC]
    public void ReciveLevelAndName(int level, string name)
    {
        // 変数に登録する
        this.level = level;
        this.enemyName = name;
        // ステータスの設定を行う
        SetStatus();
    }

    /// <summary>
    /// Photonの力でネットワーク同期！のコールバック関数
    /// </summary>
    /// <param name="stream">ビスコだよ！</param>
    /// <param name="info">送ってきた相手の情報</param>
    virtual protected void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(HP);
        }
        else
        {
            HP = (int)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    abstract protected void Update();

    public override string ToString()
    {
        // 敵の名前とレベルを返す
        return this.enemyName + " " + level.ToString() + "Lv";
    }

    /// <summary>
    /// アニメーションを変更する
    /// </summary>
    /// <param name="trigger">変更するアニメーション</param>
    protected void SetTrigger(string trigger)
    {
        // アニメーションを変更する
        anim.SetTrigger(trigger);
        // 他のプレイヤーの敵もアニメーションを変更する
        photonView.RPC("SetTriggerForRPC", PhotonTargets.All, trigger);
    }

    /// <summary>
    /// 飛んできたTriggerにアニメーションを変更する
    /// </summary>
    /// <param name="trigger">変更するアニメーション</param>
    [PunRPC]
    public void SetTriggerForRPC(string trigger)
    {
        // アニメーションを変更する
        anim.SetTrigger(trigger);
    }
}