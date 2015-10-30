using UnityEngine;
using System.Collections;

public class Cokatris : LoiteringEnemyBase {
    /// <summary>
    /// その場に止まる確率
    /// </summary>
    const int STOP_ESTABLISHMENT = 30;
    /// <summary>
    /// 止まった後その場で左右確認する確率
    /// </summary>
    const int CONFIRMATION_ESTABLISHMENT = 30;
    /// <summary>
    /// その場で威嚇する確率
    /// </summary>
    const int INTIMIDATION_ESTABLISHMENT = 10;
    /// <summary>
    /// その場で確認を行うフラグ
    /// </summary>
    bool confirmationFlag = false;
    /// <summary>
    /// 確認する方向を設定する
    /// </summary>
    int confirmationValue = 0;
    /// <summary>
    /// ジャンプアタックを行う確立
    /// </summary>
    private const int SPECIAL_ATTACK_PERCENT = 20;
    /// <summary>
    /// 攻撃が終わった時の処理
    /// </summary>
    /// <returns></returns>
    protected override bool IsEndAttack()
    {
        // 攻撃状態かどうか
        if (!attackFlag)
        {
            // 通常の状態に戻す
            enemyStatus = Status.NORMAL;
            // 終わったからtrueを返す
            return true;
        }
        // 攻撃中なのでfalseを返す
        return false;
    }

    /// <summary>
    /// プレイヤーが近くに寄ってきたときに行う処理
    /// </summary>
    protected override void NearPlayerAction()
    {
        // 攻撃アニメーションでない時
        if (!attackFlag)
        {
            // ステータスを攻撃に変更する
            enemyStatus = Status.ATTACK;
            // 移動速度を0にする
            moveValue.z = 0f;
            // 回転度を0にする
            newRotation.y = 0f;
            // ランダム行動用変数定義
            int random = Random.Range(0, 101);

            // 一定の割合で普通の攻撃
            if (random >= SPECIAL_ATTACK_PERCENT)
            {
                // 普通の攻撃アニメーション
                anim.SetTrigger("attackFlag");
                Debug.Log("Normal Attack");
            }
            // 一定の割合でスペシャル攻撃
            else
            {
                // ジャンプ攻撃のアニメーション
                anim.SetTrigger("jumpAttack");
                Debug.Log("JumpAttack");
            }
            // 攻撃中のフラグを立てる
            attackFlag = true;
        }
    }

    /// <summary>
    /// ダメージ食らう処理が終了したかどうか
    /// </summary>
    /// <returns></returns>
    protected override bool IsEndDamage()
    {
        // ダメージを受けている状態なら
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Damage"))
        {
            // falseを返す
            return false;
        }
        // ステータスを通常状態に戻す
        enemyStatus = Status.NORMAL;
        // 終わったことを返す
        return true;
    }

    /// <summary>
    /// 威嚇を行うコルーチン
    /// </summary>
    /// <returns>繰り返すかどうか</returns>
    IEnumerator Intimidation()
    {
        // ちょっと待つ
        yield return new WaitForSeconds(0.5f);
        // 威嚇中は処理を継続して行う
        while (anim.GetCurrentAnimatorStateInfo(0).IsTag("Intimidation"))
        {
            yield return null;
        }
        // 威嚇が終了したらステータスを通常状態に遷移させる
        enemyStatus = Status.NORMAL;
        // コルーチンを終了する
        yield break;
    }

    /// <summary>
    /// 回転する処理
    /// </summary>
    protected override void RotateDirection()
    {
        // 周りを確認するフラグが立っていたら
        if (!confirmationFlag)
        {
            // ランダム行動用変数定義
            int random = Random.Range(0, 100);
            // ランダムの値が30を下回っていたら
            if (random <= STOP_ESTABLISHMENT)
            {
                // 歩くのをやめる
                moveValue.z = 0f;
                // アイドル状態にする
                SetRunAnimationFlag(false);
                SetWalkAnimationFlag(false);
                // 分母調整のため10で掛ける
                random *= 100;
                // その場で確認する確立
                if (random < STOP_ESTABLISHMENT * CONFIRMATION_ESTABLISHMENT)
                {
                    // その場で確認を行うフラグをオンにする
                    confirmationFlag = true;
                    // 確認する方向に使う変数を初期化する
                    confirmationValue = 0;
                }
            }
            // その場で威嚇を行う確率
            else if (random - STOP_ESTABLISHMENT < INTIMIDATION_ESTABLISHMENT)
            {
                // 移動、回転の値を無しにする
                newRotation.y = 0f;
                moveValue.z = 0f;
                // ステータスを変更する
                enemyStatus = Status.OTHER;
                // 威嚇アニメーションを再生する
                anim.SetTrigger("intimidation");
                // 威嚇を開始する
                StartCoroutine("Intimidation");
            }
            // その他の処理
            else
            {
                // 止まる確率と威嚇する確率を減算する
                random -= STOP_ESTABLISHMENT + INTIMIDATION_ESTABLISHMENT;
                // 角度を変更する
                newRotation.y = (360f / (100 - (STOP_ESTABLISHMENT + INTIMIDATION_ESTABLISHMENT))) * random;
            }
        }
    }

    /// <summary>
    /// その場で確認する処理
    /// </summary>
    protected override void OtherIntervalAction()
    {
        // 確認する処理フラグが立っていれば
        if (confirmationFlag)
        {
            // 移動しないようにする
            moveValue.z = 0f;
            // 現在の状態によって処理を分ける
            switch (confirmationValue)
            {
                // 一番最初の処理
                case 0:
                    // 直角方向を見る
                    newRotation.y = 90f;
                    break;
                case 1:
                    // 反対方向を見る
                    newRotation.y = -180f;
                    break;
                case 2:
                    // 最初の方向を見る
                    newRotation.y = 90f;
                    break;
                case 3:
                    // フラグを落とし、初期化する
                    confirmationValue = 0;
                    confirmationFlag = false;
                    return;
            }
            // 次の行動のために値を加算する
            confirmationValue++;
        }
    }

    /// <summary>
    /// 名前を設定する
    /// </summary>
    protected override void SetName()
    {
        // コカトリスになる
        enemyName = "コカトリス";
    }

    /// <summary>
    ///  ダメージを食らった瞬間の処理
    /// </summary>
    /// <param name="col">当たってきたコライダー</param>
    /// <param name="damage">ダメージの値</param>
    protected override void SufferDamageAction(Collider col, int damage)
    {
        // 当たってきた物体が魔法ならば
        if (col.tag == "Magic")
        {
            // 一定のダメージ以下ならば
            if (damage >= 100)
            {
                // 一定の確立で
                if (Random.Range(0, 100) < 50)
                {
                    // ステータスを被ダメに変更する
                    enemyStatus = Status.DAMEGE;
                    // ダメージを受けたアニメーションを再生する
                    anim.SetTrigger("damage");
                }
            }
        }
    }

    /// <summary>
    /// 通常状態からプレイヤーを発見した瞬間の処理
    /// </summary>
    protected override void SearchPlayer()
    {
        // その場で確認するフラグをオフにする
        confirmationFlag = false;
        // その場で確認する方向の変数を初期化する
        confirmationValue = 0;
    }

    /// <summary>
    /// 開始処理
    /// </summary>
    protected sealed override void Start()
    {
        // 基底クラスのスタートを行う
        base.Start();
        // 回転するスピードを設定する
        // rotateSpeed = 0.5f;
    }
}
