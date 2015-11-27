using UnityEngine;
using System.Collections;

/// <summary>
/// 徘徊型モンスターの基礎クラス
/// </summary>
abstract public class LoiteringEnemyBase : EnemyData{
    // 視覚範囲内のヘイトが一番高いプレイヤー
    int inViewHatePlayer = 0;
    /// <summary>
    /// 攻撃中かのフラグ
    /// </summary>
    protected bool attackFlag = false;
    /// <summary>
    /// 移動方向を変更する処理
    /// </summary>
    abstract protected void RotateDirection();
    /// <summary>
    /// 既定距離に達した時の行動
    /// </summary>
    /// <param name="distance">距離</param>
    abstract protected void NearPlayerAction(float distance = -1f);

    /// <summary>
    /// 攻撃が終わった瞬間の処理
    /// </summary>
    public override void EndOfAttack()
    {
        attackFlag = false;
    }

    /// <summary>
    /// ダメージを食らった時の処理
    /// </summary>
    /// <param name="col">当たったコライダー</param>
    // abstract protected override void SufferDamageAction(Collider col, int damage);

    /// <summary>
    /// 敵の名前を設定する
    /// </summary>
    abstract override protected void SetName();

    /// <summary>
    /// 攻撃を受けている時の処理
    /// </summary>
    virtual protected void SufferingDamage() { }

    /// <summary>
    /// プレイヤーを発見した瞬間の処理
    /// </summary>
    virtual protected void SearchPlayer() { }

    /// <summary>
    /// インターバルで行う別の処理
    /// </summary>
    virtual protected void OtherIntervalAction() { }

    /// <summary>
    /// 走るモーションに切り替える処理
    /// </summary>
    /// <param name="flag">走るかどうかのフラグ</param>
    virtual protected void SetRunAnimationFlag(bool flag)
    {
        anim.SetBool("runFlag", flag);
    }

    /// <summary>
    /// 歩くアニメーションを切り替える処理
    /// </summary>
    /// <param name="flag">歩くかどうか</param>
    virtual protected void SetWalkAnimationFlag(bool flag)
    {
        anim.SetBool("walkFlag", flag);
    }

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
    virtual protected void Attacking() { }
    /// <summary>
    /// 敵がプレイヤーを見失った瞬間の処理
    /// </summary>
    virtual protected void MissingPlayer() { }
    /// <summary>
    /// 敵の状態がその他であるときに行う行動
    /// </summary>
    virtual protected void OtherAction() { }

    /// <summary>
    /// ノーマル状態の向くべき方向
    /// </summary>
    private void AttachRotation()
    {
        // x,z軸の回転を0にする
        newRotation.x = 0f;
        newRotation.z = 0f;
        // 回転させる
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * rotateSpeed);
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
            SetWalkAnimationFlag(true);
            // ヘイト値が最大のプレイヤーを登録する
            GetHaightHighestPlayer();
            // ランダムで移動する角度を変更する
            RotateDirection();
            // その他のインターバル上で行いたい処理を行う
            OtherIntervalAction();
            // マスタークライアントならば
            if (PhotonNetwork.isMasterClient)
            {
                // 移動速度、回転度を同期する
                photonTransformView.SetSynchronizedValues(moveValue, newRotation.y);
            }
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
        float distance = (transform.position - players[inViewHatePlayer].transform.position).sqrMagnitude;

        // 指定した距離に到達したかどうか
        if (distance < actionDistance)
        {
            // プレイヤーが近くに来た時の処理を行う
            NearPlayerAction(distance);
        }
        // 指定した距離に到達していない場合
        else
        {
            // 移動速度を追いかける速度に変更する
            moveValue.z = trackingSpeed;
            // 走ってるアニメーションに変更する
            SetRunAnimationFlag(true);
            // 移動させる
            transform.Translate(moveValue * Time.deltaTime);
        }
    }

    /// <summary>
    /// 特定の視野内でのプレイヤー検索用
    /// </summary>
    private bool CheckView()
    {
        // 視覚範囲にいるプレイヤーの番号を格納する変数
        int inViewPlayer = 0;
        // ヘイトの値を格納する
        int hate = -1;
        // プレイヤーの数だけ繰り返す
        for (int i = 0; i < players.Length; i++)
        {
            // プレイヤーが視覚範囲に存在していたら
            if ((players[i].transform.position - transform.position).sqrMagnitude < angleDistance                       // 距離計算
                && Vector3.Angle(players[i].transform.position - transform.position, transform.forward) <= angle)    // 角度計算
            {
                // 視覚範囲に存在したプレイヤーの番号を入れる
                inViewPlayer |= 0x00000001 << i;
                // ヘイトの値を取得する
                if (hate < playersHates[i])
                {
                    // ヘイトの値を設定する
                    hate = playersHates[i];
                    // ヘイトが一番高いプレイヤーを登録する
                    inViewHatePlayer = i;
                }
            }
        }
        // プレイヤーを発見していたら
        if ((inViewPlayer & 0xffffffff) != 0x00000000)
        {
            // ヘイトが一番高いプレイヤーに向う回転度を計算する
            newRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(players[inViewHatePlayer].transform.position - transform.position), 1.0f).eulerAngles;
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
        else
        {
            // ステータスをノーマルに変更する
            enemyStatus = Status.NORMAL;
            // 走るアニメーションをオフにする
            SetRunAnimationFlag(false);
            // 発見できなかったのでfalseを返す
            return false;
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    override sealed protected void Update()
    {
        // マスタークライアントなら
        if (PhotonNetwork.isMasterClient)
        {
            // 状態によって変化する
            switch (enemyStatus)
            {
                    // 通常時の処理
                case Status.NORMAL:
                    // 移動処理
                    Move();
                    // プレイヤーを探す処理
                    if (CheckView())
                    {
                        // プレイヤーを発見した瞬間の処理を行う
                        SearchPlayer();
                    }
                    break;

                    // 攻撃時の処理
                case Status.ATTACK:
                    // 攻撃が終了する処理
                    if (IsEndAttack())
                    {
                        // 攻撃当たり判定コンポーネントを無効化する
                        DisableAttackColliders();
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
                        // 敵がプレイヤーを見失った瞬間の処理
                        MissingPlayer();
                        break;
                    }
                    // 追いかける処理
                    Tracking();
                    break;

                // その他の処理
                case Status.OTHER:
                    OtherAction();
                    break;

                    // 敵が死亡したとき
                case Status.DEAD:
                    break;
                default:
                    break;
            }
            // 前フレーム移動速度と現在移動速度が変わっていたら
            if (pastMoveValue != moveValue)
            {
                // 移動速度を同期する
                photonTransformView.SetSynchronizedValues(moveValue, newRotation.y);
            }
            // 前フレーム移動速度を設定する
            pastMoveValue = moveValue;
        }
        // マスタークライアントでなければ
        else
        {
            // 移動させる
            gameObject.transform.Translate(moveValue * Time.deltaTime);
            // 回転させる
            gameObject.transform.Rotate(0f, newRotation.y * Time.deltaTime, 0f);
        }
    }
}
