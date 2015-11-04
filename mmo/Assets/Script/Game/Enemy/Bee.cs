using UnityEngine;
using System.Collections;

sealed public class Bee : LoiteringEnemyBase {
    /// <summary>
    /// 行う行動の種類
    /// </summary>
    enum Action
    {
        MOVE,
        STOP
    }
    /// <summary>
    /// 現在行っているアクション
    /// </summary>
    Action nowAction = Action.STOP;
    /// <summary>
    /// 遠距離攻撃時に飛ばす弾のプレハブ
    /// </summary>
    static private GameObject shotObjectPrefab;
    /// <summary>
    /// 仰け反る最低のダメージ
    /// </summary>
    const int ACTION_MIN_DAMAGE_VAL = 200;
    /// <summary>
    /// 仰け反る確率
    /// </summary>
    const int DAMAGE_ACTION_PERCENT = 40;
    /// <summary>
    /// ホバーリングを行う確率
    /// </summary>
    const int HOVERING_PERCENT = 70;
    /// <summary>
    /// 遠距離攻撃をする確率
    /// </summary>
    const int LONG_RANGE_ATTACK = 30;
    /// <summary>
    /// ２連撃する確率
    /// </summary>
    const int SPECIAL_ATTACK_PERCENT = 20;
    /// <summary>
    /// 行動をしているカウント
    /// </summary>
    int count = 0;
    /// <summary>
    /// 次の行動に移る秒数
    /// </summary>
    int nextMoveValue = 0;
    /// <summary>
    /// 停止中もしくは移動中かのフラグ
    /// </summary>
    bool moveOrStopFlag = false;


    /// <summary>
    /// ステータスを読み込むための名前を設定する
    /// </summary>
    protected override void SetName()
    {
        // 名前を設定する
        enemyName = "ビー";
    }

    /// <summary>
    /// 回転方向の決定など
    /// </summary>
    protected override void RotateDirection()
    {
        // 移動中でなく停止中でもないとき
        if (!moveOrStopFlag)
        {
            switch (nowAction)
            {
                // 現在の行動が停止中ならば
                case Action.STOP:
                    // 移動にする
                    nowAction = Action.MOVE;
                    // 次の行動に移るためのタイムをいれる
                    nextMoveValue = Random.Range(2, 5);
                    // 行動中のフラグを入れる
                    moveOrStopFlag = true;
                    // 移動方向を決定する
                    newRotation.y = Random.Range(-359f, 359f);
                    break;
                case Action.MOVE:
                    // ストップにする
                    nowAction = Action.STOP;
                    // 次の行動に移るためのタイムを入れる
                    nextMoveValue = Random.Range(1, 3);
                    // 行動中のフラグを入れる
                    moveOrStopFlag = true;
                    break;
            }
            // カウンターをリセットする
            count = 0;
        }
    }

    /// <summary>
    /// プレイヤーを見つけた瞬間の処理
    /// </summary>
    protected override void SearchPlayer()
    {
        // 基底クラスのメソッドを行う
        base.SearchPlayer();
        // カウンターを初期化する
        count = 0;
        // 移動、ストップのフラグをオフにする
        moveOrStopFlag = false;
        // 次の行動に移る時間を初期化する
        nextMoveValue = 0;
    }

    /// <summary>
    /// ダメージを食らった瞬間の処理
    /// </summary>
    /// <param name="col">当たってきたコライダー</param>
    /// <param name="damage">ダメージ</param>
    protected override void SufferDamageAction(Collider col, int damage)
    {
        // 基底クラスの処理を行う
        base.SufferDamageAction(col, damage);
        // ダメージが一定以上超えていたら
        if (damage > ACTION_MIN_DAMAGE_VAL)
        {
            // 一定の確立で
            if (Random.Range(0, 100) < DAMAGE_ACTION_PERCENT - 1)
            {
                // ダメージモーションに変更する
                anim.SetTrigger("damage");
                // 状態を被ダメに変更する
                enemyStatus = Status.DAMEGE;
            }
        }
    }

    /// <summary>
    /// インターバル中に行う処理
    /// </summary>
    protected override void OtherIntervalAction()
    {
        // 次の行動に移るカウントになったら
        if (nextMoveValue <= count++)
        {
            // 移動中、停止中フラグをオフにする
            moveOrStopFlag = false;
        }
        // 現在の行動が停止状態ならば
        if (nowAction == Action.STOP)
        {
            // 移動速度を0にする
            moveValue.z = 0f;
        }
    }

    /// <summary>
    /// 攻撃中の処理
    /// </summary>
    protected override void Attacking()
    {
    }

    /// <summary>
    /// プレイヤーが既定距離まで近づいてきた時の処理
    /// </summary>
    protected override void NearPlayerAction(float distance)
    {
        // 距離が遠距離攻撃を行う距離ならば
        if (distance > actionDistance / 2f)
        {

            // ランダムで行動するための変数定義
            int random = Random.Range(1, 101);
            // ホバリングするなら
            if (random < HOVERING_PERCENT)
            {
                // ホバリングする
                anim.SetTrigger("hovering");
                // 攻撃状態にする
                enemyStatus = Status.ATTACK;
            }
            // ホバリングをしない場合
            else
            {
                // 遠距離攻撃をする
                // PhotonNetwork.Instantiate("Enemy/Attack/" + shotObjectPrefab.name, this.transform.position, Quaternion.identity, 0);
                // モーションを変更する
                anim.SetTrigger("shotAttack");
                // ステータスを攻撃に変更する
                enemyStatus = Status.ATTACK;

            }
        }
        // 距離が近距離攻撃を行うほど近づいていたら
        else
        {
            // 攻撃方法をランダムで決定する
            int attackRandom = Random.Range(0, 100);
            // ２連撃を行うかどうか
            if (attackRandom <= SPECIAL_ATTACK_PERCENT)
            {
                // ２連撃アニメーションを再生する
                anim.SetTrigger("specialAttack");
                // ステータスを攻撃に遷移する
                enemyStatus = Status.ATTACK;
            }
            else
            {
                // 通常攻撃アニメーションを再生する
                anim.SetTrigger("attackFlag");
                // ステータスを攻撃に遷移する
                enemyStatus = Status.ATTACK;
            }
            // 空中に巻き上げられるのを防止するため重力を使う
            rigBody.useGravity = true;
        }
    }
    

    /// <summary>
    /// 攻撃が終わったかどうかの処理
    /// </summary>
    /// <returns>true : 終了 , false : 継続</returns>
    protected override bool IsEndAttack()
    {
        // 攻撃中のフラグが立っていれば
        if (attackFlag)
        {
            // falseを返す
            return false;
        }
        // 攻撃終了処理
        return true;
    }

    /// <summary>
    /// 攻撃が終わった瞬間の処理(アニメーションコントロールから呼ばれる)
    /// </summary>
    public override void EndOfAttack()
    {
        // 基底クラスの処理を行う
        base.EndOfAttack();
        // ステータスをノーマルに戻す
        enemyStatus = Status.NORMAL;
        // 重力をオフにする
        rigBody.useGravity = false;
    }

    /// <summary>
    /// ダメージを食らっている処理が終了したかどうか
    /// </summary>
    /// <returns>true : 終了 , false : 継続</returns>
    protected override bool IsEndDamage()
    {
        // ダメージを食らっているアニメーション中なら
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Damage"))
        {
            // false を返す
            return false;
        }
        // ステータスをノーマルにする
        enemyStatus = Status.NORMAL;
        // trueを返す
        return true;
    }
}
