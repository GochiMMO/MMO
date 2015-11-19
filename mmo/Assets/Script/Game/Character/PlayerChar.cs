using UnityEngine;
using System.Collections;
// PhotonViewをアタッチする
[RequireComponent(typeof(PhotonView))]
// PhotonTransformViewをアタッチする
[RequireComponent(typeof(PhotonTransformView))]
abstract public class PlayerChar : Photon.MonoBehaviour {
    /// <summary>
    /// パーティーのシステム
    /// </summary>
    PartySystem partySystem;
    /// <summary>
    /// プレイヤーの攻撃コンポーネント
    /// </summary>
    PlayerAttack[] playerAttacks;
    /// <summary>
    /// プレイヤーの状態
    /// </summary>
    protected enum Status
    {
        /// <summary>
        /// 通常状態
        /// </summary>
        NORMAL = 0x01,
        /// <summary>
        /// 攻撃中
        /// </summary>
        ATTACK = 0x02,
        /// <summary>
        /// 被弾中
        /// </summary>
        DAMAGE = 0x04,
        /// <summary>
        /// 死亡中
        /// </summary>
        DEAD = 0x08,
        /// <summary>
        /// 復活中
        /// </summary>
        REVIVE = 0x10,
    }
    /// <summary>
    /// プレイヤーのステータス
    /// </summary>
    protected Status status = Status.NORMAL;

    /// <summary>
    /// 攻撃力アップのバフ
    /// </summary>
    /// <param name="percent">効果</param>
    /// <param name="time">時間</param>
    /// <returns></returns>
    protected IEnumerator StrBuff(float percent, float time)
    {
        // 開始の時間を設定する
        float firstTime = Time.time;
        // バフを掛ける
        strBuff *= percent;
        // 時間が来るまで
        while (firstTime + time > Time.time)
        {
            // 処理を戻す
            yield return null;
        }
        // バフを戻す
        strBuff /= percent;
        // 処理終了
        yield break;
    }

    /// <summary>
    /// 知力アップのバフ
    /// </summary>
    /// <param name="percent">効果</param>
    /// <param name="time">時間</param>
    /// <returns></returns>
    protected IEnumerator IntBuff(float percent, float time)
    {
        // 開始の時間を設定する
        float firstTime = Time.time;
        // バフを掛ける
        intBuff *= percent;
        // 時間が来るまで
        while (firstTime + time > Time.time)
        {
            // 処理を戻す
            yield return null;
        }
        // バフを戻す
        intBuff /= percent;
        // 処理終了
        yield break;
    }

    /// <summary>
    /// 防御力アップのバフ
    /// </summary>
    /// <param name="percent">効果</param>
    /// <param name="time">時間</param>
    /// <returns></returns>
    protected IEnumerator DefBuff(float percent, float time)
    {
        // 開始の時間を設定する
        float firstTime = Time.time;
        // バフを掛ける
        defBuff *= percent;
        // 時間が来るまで
        while (firstTime + time > Time.time)
        {
            // 処理を戻す
            yield return null;
        }
        // バフを戻す
        defBuff /= percent;
        // 処理終了
        yield break;
    }

    /// <summary>
    /// 魔法防御力アップのバフ
    /// </summary>
    /// <param name="percent">効果</param>
    /// <param name="time">時間</param>
    /// <returns></returns>
    protected IEnumerator MndBuff(float percent, float time)
    {
        // 開始の時間を設定する
        float firstTime = Time.time;
        // バフを掛ける
        mndBuff *= percent;
        // 時間が来るまで
        while (firstTime + time > Time.time)
        {
            // 処理を戻す
            yield return null;
        }
        // バフを戻す
        mndBuff /= percent;
        // 処理終了
        yield break;
    }

    /// <summary>
    /// HPを徐々に回復させる処理
    /// </summary>
    /// <param name="healValue">回復する量</param>
    /// <param name="skillTime">スキルの効果時間</param>
    /// <param name="healTime">回復する時間</param>
    /// <returns>反復子</returns>
    protected IEnumerator Regeneration(int healValue, float skillTime, float healTime)
    {
        
        // 開始時間を取得する
        float startTime = Time.time;
        // 回復した回数を設定する変数を定義
        int healCount = 1;
        // 時間が経つまで繰り返す
        while (startTime + skillTime > Time.time)
        {
            // 回復する時間になったかどうか
            if (startTime + healTime * healCount > Time.time)
            {
                // エフェクトを発生させる
                // GameObject.Instantiate(refeneEffect, gameObject.transform.position + Vector3.up * 3f, Quaternion.identity);
                // 回復させる
                Recover(healValue);
            }
            // 繰り返す
            yield return null;
        }
        // 処理が終了したら抜け出す
        yield break;
    }

    /// <summary>
    /// 移動速度、回転速度を同期するためのコンポーネント
    /// </summary>
    protected PhotonTransformView photonTransformView { private set; get; }
    /// <summary>
    /// アニメーションをコントロールするコンポーネント
    /// </summary>
    protected Animator anim { private set; get; }
    /// <summary>
    /// Rigidbodyコンポーネント
    /// </summary>
    protected Rigidbody rigbody;
    /// <summary>
    /// ステータスを送るフラグ
    /// </summary>
    private bool firstSyncFlag;
    /// <summary>
    /// プレイヤーのステータス
    /// </summary>
    protected PlayerData playerData = new PlayerData();
    /// <summary>
    /// 移動速度
    /// </summary>
    protected float moveSpeed { private set; get; }
    /// <summary>
    /// 移動ベクトル
    /// </summary>
    private Vector3 moveValue;
    /// <summary>
    /// 回転スピード
    /// </summary>
    private float rotateSpeed;
    /// <summary>
    /// 前方向ベクトル
    /// </summary>
    private Vector3 forwardVector;
    /// <summary>
    /// 右方向ベクトル
    /// </summary>
    private Vector3 rightVector;
    /// <summary>
    /// 攻撃力につくバフの値
    /// </summary>
    protected float strBuff { private set; get; }
    /// <summary>
    /// 知力につくバフの値
    /// </summary>
    protected float intBuff { private set; get; }
    /// <summary>
    /// 防御力につくバフの値
    /// </summary>
    protected float defBuff { private set; get; }
    /// <summary>
    /// 魔法防御力につくバフの値
    /// </summary>
    protected float mndBuff { private set; get; }
    /// <summary>
    /// 次に必要な経験値
    /// </summary>
    private int nextExp = 0;
    /// <summary>
    /// 攻撃中かどうか
    /// </summary>
    private bool isAttack = false;
    /// <summary>
    /// 被弾中かどうか
    /// </summary>
    private bool isDamage = false;

    /// <summary>
    /// プレイヤーのHP
    /// </summary>
    public int HP
    {
        set
        {
            // MAXHPを超えていたら
            if (value > playerData.MaxHP)
            {
                // MAXHPに調整する
                value = playerData.MaxHP;
            }
            // HPが0を下回っていたら
            else if (value < 0)
            {
                // 0にする
                value = 0;
            }
            // HPを設定する
            playerData.HP = value;
        }
        get
        {
            // プレイヤーのHPを返す
            return playerData.HP;
        }
    }

    /// <summary>
    /// プレイヤーのSP
    /// </summary>
    public int SP
    {
        set
        {
            // MAXSPを超えていたら
            if (value > playerData.MaxSP)
            {
                //MAXSPに調整する
                value = playerData.MaxSP;
            }
            // PlayerのSPを調整する
            playerData.SP = value;
        }
        get
        {
            // プレイヤーのSPを返す
            return playerData.SP;
        }
    }

    /// <summary>
    /// 何か当たり判定用コライダーが当たってきた時の処理
    /// </summary>
    /// <param name="hitCol">当たってきたコライダー</param>
    private void OnTriggerEnter(Collider hitCollider)
    {
        // 当たってきたコライダーの名前がEnemyAttack(敵の攻撃)だった場合
        if (hitCollider.gameObject.tag == "EnemyAttack" && (status & (Status.DEAD | Status.REVIVE)) != 0)
        {
            // 敵の攻撃用コンポーネントを取得する
            EnemyAttack enmAttack = hitCollider.gameObject.GetComponent<EnemyAttack>();
            // 敵の攻撃力を設定する
            int attack = enmAttack.GetDamage();
            // 敵の攻撃の種類によって処理分け
            switch (enmAttack.attackKind)
            {
                case EnemyAttack.AttackKind.PHYSICS:
                    // 防御力を適用する
                    attack -= playerData.defense;
                    break;
                case EnemyAttack.AttackKind.MAGIC:
                    // 魔法防御力を適用する
                    attack -= playerData.magicDefence;
                    break;
            }
            // 攻撃力が0を下回ったとき0とする
            if (attack < 0) attack = 0;
            // HPを減算する
            HP -= attack;
            // HPが0になったら
            if (HP == 0)
            {
                // 死亡状態にする
                status = Status.DEAD;
                // アニメーションを再生する
                anim.SetTrigger("Dead");
            }
        }
    }

    /// <summary>
    /// 初期化関数
    /// </summary>
    public void Initialize()
    {
        Start();
    }

    // Use this for initialization
    protected virtual void Start () {
        // プレイヤーのステータスを取得する
        playerData = PlayerStatus.playerData;
        // 通信同期のためのコンポーネントを取得する
        photonTransformView = gameObject.GetComponent<PhotonTransformView>();
        // アニメーションのためのコンポーネントを取得する
        anim = gameObject.GetComponent<Animator>();
        // 移動速度を設定する
        moveSpeed = 10f;
        // バフの値を初期化する
        strBuff = intBuff = defBuff = mndBuff = 1f;
        // 回転スピードを変更する
        rotateSpeed = 3f;
        // プレイヤーの攻撃コンポーネントを取得する
        playerAttacks = GetComponentsInChildren<PlayerAttack>();
        // パーティーシステムを取得する
        partySystem = GameObject.Find("Scripts").GetComponent<PartySystem>();
        // rigbodyを取得する
        rigbody = gameObject.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 経験値の加算
    /// </summary>
    /// <param name="exp">経験値</param>
    public void AddExp(int exp)
    {
        // パーティーメンバーを格納する変数
        GameObject[] partyMembers;
        // パーティーメンバーが２人以上のとき
        if ((partyMembers = partySystem.GetPartyMember()).Length > 1)
        {
            // expを変更する(パーティーメンバーの数だけ経験値を少し減らす
            exp = (int)((float)exp * (1f - (float)partyMembers.Length / 10f));
            // パーティーメンバー分繰り返す
            foreach (GameObject partyMember in partyMembers)
            {
                // RPCを送信し、経験値を加算させる
                photonView.RPC("AddExpRpc", partyMember.GetPhotonView().owner, exp);
            }
        }
        // パーティーがいなければ
        else
        {
            // EXPをそのまま加算する
            playerData.nowExp += exp;
        }
    }

    /// <summary>
    /// 経験値を加算する関数
    /// </summary>
    /// <param name="exp">経験値</param>
    [PunRPC]
    public void AddExpRpc(int exp)
    {
        // 死亡状態でない時
        if (status != Status.DEAD)
        {
            // 経験値を加算する
            playerData.nowExp += exp;
            // レベルアップチェックを行う
            LevelUp();
        }
    }

    /// <summary>
    /// 前、右方向ベクトルを設定する
    /// </summary>
    private void UpdateVector()
    {
        // 前方向ベクトルを設定する
        forwardVector = Camera.main.transform.TransformDirection(Vector3.forward);
        // Y軸方向は無いので0とする
        forwardVector.y = 0f;
        // 右方向ベクトルを設定する
        rightVector = Camera.main.transform.TransformDirection(Vector3.right);
        // Y軸を0にする
        rightVector.y = 0f;
    }

    /// <summary>
    /// レベルアップしてるかチェックする関数
    /// </summary>
    private void LevelUp()
    {
        // 現在経験値がレベルアップする経験値を超えていたら
        if (playerData.nowExp > nextExp)
        {
            // エフェクトを表示する
            // GameObject.Instantiate(levelUpEffect, gameObject.transform.position + Vector3.up * 3f, Quaternion.identity);
            // 現在経験値を消費する
            playerData.nowExp -= nextExp;
            // レベルアップする
            playerData.Lv++;
            // ステータスポイントを加算する
            playerData.statusPoint += 5;
            // 加算するスキルポイントを格納する変数定義
            int addSkillPoint = 0;
            // 10Lvを下回っている間
            if (playerData.Lv > 10)
            {
                // レベル自体を加算するポイントとする(1~9)
                addSkillPoint = playerData.Lv;
            }
            // 10Lvを超えている時
            else
            {
                // レベル / １０ * １０
                addSkillPoint = (playerData.Lv / 10) * 10;
            }
            // スキルポイントを加算する
            playerData.skillPoint += addSkillPoint;
            // 次に必要な経験値を計算する
            nextExp = 50 * playerData.Lv * playerData.Lv * playerData.Lv;
        }
    }

    /// <summary>
    /// 移動関数
    /// </summary>
    protected void Move()
    {
        // 前方向ベクトルを更新する
        UpdateVector();
        // moveValueを初期化する
        moveValue = Vector3.zero;
        // 移動ベクトルを計算する
        moveValue = (Input.GetAxis("Horizontal") * rightVector + Input.GetAxis("Vertical") * forwardVector) * Time.deltaTime * moveSpeed;
        // 移動ベクトルが0ならば
        if (moveValue == Vector3.zero)
        {
            // 走っているフラグをオフにする
            anim.SetBool("RunFlag", false);
        }
        // 移動ベクトルがあるならば
        else
        {
            // 回転させる
            gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveValue), Time.deltaTime * rotateSpeed);
            // 移動させる
            gameObject.transform.Translate(moveValue, Space.World);
            // 走っているフラグをオンにする
            anim.SetBool("RunFlag", true);
        }
        // ネットワーク同期処理
        photonTransformView.SetSynchronizedValues(moveValue, rotateSpeed);
    }

    /// <summary>
    /// Player data getter.
    /// </summary>
    /// <returns>player data.</returns>
    public PlayerData GetPlayerData()
    {
        return this.playerData;
    }

    /// <summary>
    /// Synchronized player data.
    /// </summary>
    /// <param name="stream">PhotonStream</param>
    /// <param name="info">PhotonMessageInfo</param>
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // まだステータスを送っていない時
            if (!firstSyncFlag)
            {
                // プレイヤーのステータスを送る
                stream.SendNext(playerData);
                // ステータスを送ったフラグを立てる
                firstSyncFlag = true;
            }
        }
        else
        {
            // プレイヤーのステータスをもらう
            playerData = (PlayerData)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// 攻撃用コンポーネントを有効化する
    /// </summary>
    /// <param name="attackNumber">コンポーネントの番号</param>
    protected virtual void EnablePlayerAttack(int attackNumber = -1)
    {
        // 攻撃用コンポーネントの番号に指定が無ければ
        if (attackNumber == -1)
        {
            // 攻撃用コンポーネントの数だけ繰り返す
            for (int i = 0; i < playerAttacks.Length; i++)
            {
                // 攻撃用コンポーネントを有効化する
                playerAttacks[i].col.enabled = true;
            }
        }
        // 番号に指定があり、有効範囲内である場合
        else if (attackNumber >= 0 && attackNumber < playerAttacks.Length)
        {
            // 指定番号のコンポ―ネントを有効化する
            playerAttacks[attackNumber].col.enabled = true;
        }
    }

    /// <summary>
    /// 攻撃用コンポーネントを無効化する
    /// </summary>
    /// <param name="attackNumber">コンポーネントの番号</param>
    protected virtual void DisablePlayerAttack(int attackNumber = -1)
    {
        // 攻撃用コンポーネントの番号に指定が無ければ
        if (attackNumber == -1)
        {
            // 攻撃用コンポーネントの数だけ繰り返す
            for (int i = 0; i < playerAttacks.Length; i++)
            {
                // 攻撃用コンポーネントを無効化する
                playerAttacks[i].col.enabled = false;
            }
        }
        // 番号に指定があり、有効範囲内である場合
        else if (attackNumber >= 0 && attackNumber < playerAttacks.Length)
        {
            // 指定番号のコンポ―ネントを無効化する
            playerAttacks[attackNumber].col.enabled = false;
        }
    }

    /// <summary>
    /// 攻撃力を設定する(物理)
    /// </summary>
    /// <param name="attackNumber">攻撃コンポーネントの番号</param>
    virtual protected void SetAttack(int attack, int attackNumber = -1)
    {
        // 攻撃コンポーネントの番号が指定されていない時
        if (attackNumber == -1)
        {
            // 攻撃コンポーネントの数だけ繰り返す
            for (int i = 0; i < playerAttacks.Length; i++)
            {
                // 攻撃力を設定する
                playerAttacks[i].attack = attack;
                // 攻撃の種類を変更する
                playerAttacks[i].attackKind = PlayerAttack.AttackKind.PHYSICS;
            }
        }
        // 攻撃コンポーネントの番号に指定があった場合、範囲をチェックしその範囲内であったとき
        else if (attackNumber >= 0 && attackNumber < playerAttacks.Length)
        {
            // 攻撃力を設定する
            playerAttacks[attackNumber].attack = attack;
            // 攻撃の種類を変更する
            playerAttacks[attackNumber].attackKind = PlayerAttack.AttackKind.PHYSICS;
        }
    }

    /// <summary>
    /// 攻撃力を設定する(魔法)
    /// </summary>
    /// <param name="attackNumber">攻撃コンポーネントの番号</param>
    virtual protected void SetMagicAttack(int attack, int attackNumber = -1)
    {
        // 攻撃コンポーネントの番号が指定されていない時
        if (attackNumber == -1)
        {
            // 攻撃コンポーネントの数だけ繰り返す
            for (int i = 0; i < playerAttacks.Length; i++)
            {
                // 攻撃力を設定する
                playerAttacks[i].attack = attack;
                // 攻撃の種類を変更する
                playerAttacks[i].attackKind = PlayerAttack.AttackKind.MAGIC;
            }
        }
        // 攻撃コンポーネントの番号に指定があった場合、範囲をチェックしその範囲内であったとき
        else if (attackNumber >= 0 && attackNumber < playerAttacks.Length)
        {
            // 攻撃力を設定する
            playerAttacks[attackNumber].attack = attack;
            // 攻撃の種類を変更する
            playerAttacks[attackNumber].attackKind = PlayerAttack.AttackKind.MAGIC;
        }
    }

    /// <summary>
    /// リジェネを発動する関数
    /// </summary>
    /// <param name="healValue">回復する量</param>
    /// <param name="skillTime">スキルの効果時間</param>
    /// <param name="healTime">回復する時間</param>
    [PunRPC]
    public void GenerationRegeneration(int healValue, float skillTime, float healTime)
    {
        // リジェネを発生させる
        StartCoroutine(Regeneration(healValue, skillTime, healTime));
    }

    /// <summary>
    /// 攻撃をする関数
    /// </summary>
    protected virtual void Attack()
    {
        // 攻撃中フラグを立てる
        isAttack = true;
        // ステータスを変更する
        status = Status.ATTACK;
    }

    /// <summary>
    /// アニメーションの方から呼ばれる攻撃モーション終了の関数
    /// </summary>
    public virtual void EndAttackAnimation(){
        isAttack = false;
    }

    /// <summary>
    /// アニメーションの方から呼ばれる被弾モーションの終了
    /// </summary>
    public void EndDamageAnimation()
    {
        isDamage = false;
    }

    /// <summary>
    /// 他のプレイヤーの同期
    /// </summary>
    private void Transform()
    {
        // 移動させる
        gameObject.transform.Translate(moveValue);
    }

    /// <summary>
    /// 攻撃中の処理
    /// </summary>
    virtual protected void Attacking() { }

    /// <summary>
    /// 攻撃が終了した瞬間の処理
    /// </summary>
    virtual protected void EndOfAttack()
    {
        // 攻撃用当り判定コンポーネントを無効化する
        DisablePlayerAttack();
        // ステータスをNormalに戻す
        status = Status.NORMAL;
    }

    /// <summary>
    /// 通常状態時の処理
    /// </summary>
    virtual protected void Normal() { }

    /// <summary>
    /// 被弾中の処理
    /// </summary>
    virtual protected void Damage() { }

    /// <summary>
    /// 死亡中の処理
    /// </summary>
    virtual protected void Dead() {
        // rigidbodyが働いてるときは
        if (!rigbody.isKinematic)
        {
            // オフにする
            rigbody.isKinematic = true;
        }
    }

    /// <summary>
    /// 生き返っている時の瞬間
    /// </summary>
    virtual protected void Revive() { }

    /// <summary>
    /// スキルを使う処理
    /// </summary>
    /// <param name="skillNumber">使うスキルの番号</param>
    abstract public bool UseSkill(int skillNumber, SkillBase skill);

    // Update is called once per frame
    private void Update () {
        // プレイヤーがローカル処理下に置いてあるか
        //if (photonView.isMine)
        {
            // プレイヤーのステータスによって処理分けを行う
            switch (status)
            {
                case Status.NORMAL:
                    // 通常状態の処理を行う
                    Normal();
                    // ステータスが通常状態でなくなったら
                    if (Status.NORMAL != status)
                    {
                        // 処理を外れる
                        break;
                    }
                    // 移動関数
                    Move();
                    break;
                case Status.ATTACK:
                    // 攻撃が終了したら
                    if (!isAttack)
                    {
                        // 攻撃が終了した瞬間の処理を行う
                        EndOfAttack();
                        // 処理を外れる
                        break;
                    }
                    // 攻撃中の処理を行う
                    Attacking();
                    break;
                case Status.DAMAGE:
                    // ダメージを受ける処理が終了したら
                    if (!isDamage)
                    {
                        // 処理を抜ける
                        break;
                    }
                    // 被弾中の処理を行う
                    Damage();
                    break;
                case Status.DEAD:
                    // 死亡時の処理を行う
                    Dead();
                    break;
                case Status.REVIVE:
                    // 生き返っている時の処理を行う
                    Revive();
                    break;
            }
        }
        // 他のプレイヤーのキャラクターだった場合
        //else
        {
            // 移動させる
            // Transform();
        }
    }

    /// <summary>
    /// 回復するスキル
    /// </summary>
    /// <param name="addHp">回復する値</param>
    [PunRPC]
    public void Recover(int addHp)
    {
        // 死んでいなければ
        if (status != Status.DEAD)
        {
            // プレイヤーのHPを足す
            HP += addHp;
        }
    }

    /// <summary>
    /// Save player data.
    /// </summary>
    private void SavePlayerCharData()
    {
        // 座標を設定
        playerData.x = gameObject.transform.position.x;
        playerData.y = gameObject.transform.position.y;
        playerData.z = gameObject.transform.position.z;
        // ログアウトしたシーンを保存
        playerData.logoutScene = Application.loadedLevel;
        // セーブする
        SaveManager.Save<PlayerData>(playerData, playerData.name);
    }

    /// <summary>
    /// If destroy this game object, call this method once.
    /// </summary>
    protected virtual void OnDestroy()
    {
        // セーブする
        SavePlayerCharData();
    }
}
