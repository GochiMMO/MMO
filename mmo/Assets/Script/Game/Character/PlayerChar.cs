using UnityEngine;
using System.Collections;
// PhotonViewをアタッチする
[RequireComponent(typeof(PhotonView))]
// PhotonTransformViewをアタッチする
[RequireComponent(typeof(PhotonTransformView))]
abstract public class PlayerChar : Photon.MonoBehaviour {
    /// <summary>
    /// 重力
    /// </summary>
    const float GRAVITY = 9.8f;
    /// <summary>
    /// 重力計算用構造体W
    /// </summary>
    Vector3 gravity = Vector3.zero;
    /// <summary>
    /// キャラクターのコントローラーコンポーネント
    /// </summary>
    CharacterController character;
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
    /// ヘイトの値
    /// </summary>
    private int hate = 0;
    /// <summary>
    /// ヘイトを設定するプロパティ
    /// </summary>
    public int Hate
    {
        set { hate = value < 0 ? 0 : value; }
        get { return hate; }
    }
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
                // GameObject.Instantiate(regeneEffect, gameObject.transform.position + Vector3.up * 3f, Quaternion.identity);
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
    /// 移動速度のバフ
    /// </summary>
    /// <param name="percent">上がる割合</param>
    /// <param name="time">時間</param>
    /// <returns></returns>
    protected IEnumerator SpeedBuf(float percent, float time)
    {
        // バフの値を乗算する
        speedBuf *= percent;
        // 時間まで待つ
        yield return new WaitForSeconds(time);
        // バフの値を元に戻す
        speedBuf /= percent;
        // コルーチンから抜ける
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
    protected float moveSpeed = 7f;
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
    /// 移動速度につくバフ
    /// </summary>
    private float speedBuf = 1f;
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
    /// ステータスを同期するタイミング(秒)
    /// </summary>
    private static readonly float UpdateStatusRate = 0.2f;

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
    /// キャラクターコントローラーにぶつかってきたとき
    /// </summary>
    /// <param name="hitCollider"></param>
    private void OnControllerColliderHit(ControllerColliderHit hitCollider)
    {
        // 当たってきたコライダーの名前がEnemyAttack(敵の攻撃)だった場合
        if (hitCollider.gameObject.tag == "EnemyAttack" && (status & (Status.DEAD | Status.REVIVE)) == 0)
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
                SetTrigger("Dead");
                
            }
        }
    }

    /// <summary>
    /// 何か当たり判定用コライダーが当たってきた時の処理
    /// </summary>
    /// <param name="hitCol">当たってきたコライダー</param>
    private void OnTriggerEnter(Collider hitCollider)
    {
        // 当たってきたコライダーの名前がEnemyAttack(敵の攻撃)だった場合
        if (hitCollider.gameObject.tag == "EnemyAttack" && (status & (Status.DEAD | Status.REVIVE)) == 0)
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
                SetTrigger("Dead");
                // デスペナルティーとして最大経験値の10%を減らす
                playerData.nowExp -= PlayerStatus.nextLevelExp / 10;
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
        // 通信同期のためのコンポーネントを取得する
        photonTransformView = gameObject.GetComponent<PhotonTransformView>();
        // アニメーションのためのコンポーネントを取得する
        anim = gameObject.GetComponent<Animator>();
        if (photonView.isMine)
        {
            // 次に必要な経験値を計算し、格納する
            nextExp = PlayerStatus.nextLevelExp;
            // プレイヤーのステータスを取得する
            playerData = PlayerStatus.playerData;
            // バフの値を初期化する
            strBuff = intBuff = defBuff = mndBuff = 1f;
            // 回転スピードを変更する
            rotateSpeed = 5f;
            // プレイヤーの攻撃コンポーネントを取得する
            playerAttacks = GetComponentsInChildren<PlayerAttack>();
            // パーティーシステムを取得する
            partySystem = GameObject.Find("Scripts").GetComponent<PartySystem>();
            // rigbodyを取得する
            rigbody = gameObject.GetComponent<Rigidbody>();
            // キャラクターコントローラーを取得する
            character = gameObject.GetComponent<CharacterController>();
            // サブカメラを探し出し、その数だけ繰り返す
            foreach (GameObject subCamera in GameObject.FindGameObjectsWithTag("SubCamera"))
            {
                // 自分を入れる
                subCamera.GetComponent<MiniMapCamera>().player = this.gameObject;
            }
            // プレイヤーのステータスを送信する
            StartCoroutine(UpdateStatusOtherPlayers());
        }
        else
        {
            // 職業と名前を送らせる
            photonView.RPC("SendNameAndJob", photonView.owner);
        }
    }

    /// <summary>
    /// プレイヤーに対し経験値を加算させる
    /// </summary>
    /// <param name="exp"></param>
    public void CallAddExp(int exp)
    {
        // このキャラクターを出したプレイヤーに経験値加算命令を出す
        photonView.RPC("AddExp", photonView.owner, exp);
    }

    /// <summary>
    /// 経験値の加算
    /// </summary>
    /// <param name="exp">経験値</param>
    [PunRPC]
    public void AddExp(int exp)
    {
        // パーティーメンバーを格納する変数
        GameObject[] partyMembers = partySystem.GetPartyMember();
        // パーティーメンバーが２人以上のとき
        if (partyMembers != null && partyMembers.Length > 1)
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
            AddExpRpc(exp);
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
        // 前方向ベクトルを正規化する
        forwardVector.Normalize();
        // 右方向ベクトルを正規化する
        rightVector.Normalize();
    }

    /// <summary>
    /// レベルアップしてるかチェックする関数
    /// </summary>
    private void LevelUp()
    {
        // 現在経験値がレベルアップする経験値を超えていたら
        while(playerData.nowExp > nextExp)
        {
            // エフェクトを表示する
            // PhotonNetwork.Instantiate(levelUpEffect, gameObject.transform.position + Vector3.up * 3f, Quaternion.identity);
            // 現在経験値を消費する
            playerData.nowExp -= nextExp;
            // レベルアップする
            playerData.Lv++;
            // ステータスポイントを加算する
            playerData.statusPoint += 5;
            // スキルポイントを加算する
            playerData.skillPoint += PlayerStatus.addSkillPoint;
            // 次に必要な経験値を計算する
            nextExp = PlayerStatus.nextLevelExp;
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
        moveValue = (Input.GetAxis("Horizontal") * rightVector + Input.GetAxis("Vertical") * forwardVector) * moveSpeed * speedBuf;
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
            gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveValue), Time.deltaTime * rotateSpeed);

            // 移動させる
            // gameObject.transform.Translate(moveValue, Space.World);
            // 移動させる
            character.Move(moveValue * Time.deltaTime);

            // 走っているフラグをオンにする
            anim.SetBool("RunFlag", true);
        }
        // ネットワーク同期処理(速度と回転)
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
    virtual protected void SetAttack(int attack, int attackNumber = -1, float damageRate = 0.1f, int SP)
    {
        // 攻撃コンポーネントの番号が指定されていない時
        if (attackNumber == -1)
        {
            // 攻撃コンポーネントの数だけ繰り返す
            for (int i = 0; i < playerAttacks.Length; i++)
            {
                // 攻撃の各種パラメータを設定する
                playerAttacks[i].SetProperties(attack, damageRate, PlayerAttack.AttackKind.PHYSICS, this, SP);
            }
        }
        // 攻撃コンポーネントの番号に指定があった場合、範囲をチェックしその範囲内であったとき
        else if (attackNumber >= 0 && attackNumber < playerAttacks.Length)
        {
            // 攻撃の各種パラメータを設定する
            playerAttacks[attackNumber].SetProperties(attack, damageRate, PlayerAttack.AttackKind.PHYSICS, this, SP);
        }
    }

    /// <summary>
    /// 攻撃力を設定する(魔法)
    /// </summary>
    /// <param name="attackNumber">攻撃コンポーネントの番号</param>
    virtual protected void SetMagicAttack(int attack, int attackNumber = -1, float damageRate = 0.1f, int SP)
    {
        // 攻撃コンポーネントの番号が指定されていない時
        if (attackNumber == -1)
        {
            // 攻撃コンポーネントの数だけ繰り返す
            for (int i = 0; i < playerAttacks.Length; i++)
            {
                // 攻撃の各種パラメータを設定する
                playerAttacks[i].SetProperties(attack, damageRate, PlayerAttack.AttackKind.MAGIC, this, SP);
            }
        }
        // 攻撃コンポーネントの番号に指定があった場合、範囲をチェックしその範囲内であったとき
        else if (attackNumber >= 0 && attackNumber < playerAttacks.Length)
        {
            // 攻撃の各種パラメータを設定する
            playerAttacks[attackNumber].SetProperties(attack, damageRate, PlayerAttack.AttackKind.MAGIC, this, SP);
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
    /*
    /// <summary>
    /// 通常状態時の処理
    /// </summary>
    virtual protected void Normal() { }
    */
    /// <summary>
    /// 被弾中の処理
    /// </summary>
    virtual protected void Damage() { }

    /// <summary>
    /// 死亡中の処理
    /// </summary>
    virtual protected void Dead() {

    }

    /// <summary>
    /// 通常攻撃
    /// </summary>
    virtual protected void NormalAttack() {
    }

    /// <summary>
    /// 通常状態の処理
    /// </summary>
    private void OnNormal()
    {
        // 左クリックされたら
        if (IsClick())
        {
            // 通常攻撃を行う
            NormalAttack();
        }
    }

    /// <summary>
    /// 左クリックされ、かつUIの上にカーソルが無ければtrue、その他はfalse
    /// </summary>
    /// <returns></returns>
    protected bool IsClick()
    {
        // 左クリックされたら
        if (Input.GetMouseButtonDown(0))
        {
            // レイを飛ばし、ヒットするかどうかチェックする
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
            // ヒットしていなければ
            if (!hit.collider)
            {
                return true;
            }
        }
        // クリックされてないか、コライダーにヒットしたか
        return false;
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
        if (photonView.isMine)
        {
            // プレイヤーのステータスによって処理分けを行う
            switch (status)
            {
                case Status.NORMAL:
                    // 通常状態の処理を行う
                    OnNormal();
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
            // レベルアップ監視
            LevelUp();
            // rigidbodyを使えるようにしておく
            rigbody.WakeUp();
        }
        // 他のプレイヤーのキャラクターだった場合
        else
        {
            // 移動させる
            Transform();
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
        // コルーチンをストップする
        StopAllCoroutines();
        // セーブする
        SavePlayerCharData();
    }

    /// <summary>
    /// プレイヤーの状態を同期する
    /// </summary>
    /// <param name="stream">同期用変数</param>
    /// <param name="info">送ってきたプレイヤーのデータ</param>
    protected void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*
        if (stream.isWriting)
        {
            Debug.Log("プレイヤーのステータス送信");
            stream.SendNext(playerData.HP);
            stream.SendNext(playerData.MaxHP);
            stream.SendNext(playerData.job);
            stream.SendNext(playerData.Lv);
        }
        else
        {
            Debug.Log("プレイヤーのステータス受信  " + info.sender.name);
            playerData.HP = (int)stream.ReceiveNext();
            playerData.MaxHP = (int)stream.ReceiveNext();
            playerData.job = (int)stream.ReceiveNext();
            playerData.Lv = (int)stream.ReceiveNext();
            playerData.name = info.sender.name;
        }
        */
    }

    /// <summary>
    /// 自分のステータスを送る処理
    /// </summary>
    private void SendStatus()
    {
        // 自分のステータスを送信する
        photonView.RPC("ReciveStatus", PhotonTargets.All, HP, playerData.MaxHP, playerData.Lv);
    }

    /// <summary>
    /// 名前と職業を送らせる関数
    /// </summary>
    [PunRPC]
    private void SendNameAndJob(PhotonMessageInfo info)
    {
        // RPCで名前と職業を送る
        photonView.RPC("ReciveNameAndJob", info.sender, playerData.name, playerData.job);
    }

    /// <summary>
    /// 名前と職業を受け取る関数
    /// </summary>
    /// <param name="name">名前</param>
    /// <param name="job">職業の番号</param>
    [PunRPC]
    public void ReciveNameAndJob(string name, int job)
    {
        // 名前を設定する
        playerData.name = name;
        // 職業を設定する
        playerData.job = job;
    }

    /// <summary>
    /// ステータスを受け取る関数
    /// </summary>
    /// <param name="hp">現在HP</param>
    /// <param name="maxHP">最大HP</param>
    /// <param name="level">現在レベル</param>
    [PunRPC]
    public void ReciveStatus(int hp, int maxHP, int level, PhotonMessageInfo info)
    {
        // ステータスを反映する
        playerData.HP = hp;
        playerData.MaxHP = maxHP;
        playerData.Lv = level;
        playerData.name = info.sender.name;
    }

    /// <summary>
    /// 他のプレイヤーにステータスを送信する処理
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateStatusOtherPlayers()
    {
        // 無限ループ
        while (true)
        {
            // ステータスを送信する
            SendStatus();
            // ステータスを送信する時間を待つ
            yield return new WaitForSeconds(UpdateStatusRate);
        }
    }

    /// <summary>
    /// ローカルプレイヤーのアニメーションを変更し、他のＰＣの自分も変更させる
    /// </summary>
    /// <param name="trigger"></param>
    protected void SetTrigger(string trigger)
    {
        // 自分のアニメーションを変更する
        anim.SetTrigger(trigger);
        // 他のプレイヤーのアニメーションも変更させる
        photonView.RPC("SetTriggerForRPC", PhotonTargets.All, trigger);
    }

    /// <summary>
    /// RPCで飛んできた
    /// </summary>
    /// <param name="trigger"></param>
    [PunRPC]
    public void SetTriggerForRPC(string trigger)
    {
        // アニメーションを変更する
        anim.SetTrigger(trigger);
    }

    /// <summary>
    /// 物理に沿った動きを計算するアップデート
    /// </summary>
    void FixedUpdate()
    {
        // 落下
        // gravity.y += Physics.gravity.y * Time.fixedDeltaTime;
        gravity.y += Physics.gravity.y;
        character.Move(gravity * Time.fixedDeltaTime);

        // 着地していたら速度を0にする
        if (character.isGrounded)
        {
            gravity.y = 0f;
        }
    }
}