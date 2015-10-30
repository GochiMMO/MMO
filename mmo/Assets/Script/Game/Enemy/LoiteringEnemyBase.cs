using UnityEngine;
using System.Collections;

/// <summary>
/// 徘徊型モンスターの基礎クラス
/// </summary>
abstract public class LoiteringEnemyBase : EnemyData{
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
    abstract protected void NearPlayerAction();

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
        if (kyori < actionDistance)
        {
            // プレイヤーが近くに来た時の処理を行う
            NearPlayerAction();
            Debug.Log("プレイヤー接近");
        }
        // 指定した距離に到達していない場合
        else
        {
            Debug.Log("プレイヤーから離れました");
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
        Debug.Log(Vector3.Angle(transform.position - haightMaxPlayer.transform.position, transform.forward).ToString());

        // プレイヤーを発見したら
        if ((haightMaxPlayer.transform.position - transform.position).sqrMagnitude < angleDistance                    // 距離計算
            && Vector3.Angle(haightMaxPlayer.transform.position - transform.position, transform.forward) <= angle)    // 角度計算
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
                case Status.NORMAL:
                    Move();         // 移動処理
                    // プレイヤーを探す処理
                    if (CheckView())
                    {
                        // プレイヤーを発見した瞬間の処理を行う
                        SearchPlayer();
                    }
                    break;
                case Status.ATTACK:
                    // 攻撃が終了する処理
                    if (IsEndAttack())
                    {
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
                default:
                    break;
            }
        }
    }
}
