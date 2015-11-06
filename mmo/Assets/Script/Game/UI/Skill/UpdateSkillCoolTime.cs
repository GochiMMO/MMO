using UnityEngine;
using System.Collections;

public class UpdateSkillCoolTime : MonoBehaviour {
    [SerializeField, Tooltip("回転させる画像の右側")]
    Transform maskImage1;
    [SerializeField, Tooltip("回転させる画像の左側")]
    Transform maskImage2;

    // クールタイムを行っているフラグ
    bool coolTimeFlag = false;

    // クールタイム
    public float coolTime = 0f;

    // 現在時間
    float nowTime = 0f;

    // どのくらいの割合回転させているか
    float timePercentage = 0f;

    // 回転角
    float angle = 0f;

    // Update is called once per frame
    void Update()
    {
        // クールタイムのフラグが立っていたら
        if (coolTimeFlag)
        {
            // 角度が設定されていなければ
            if (angle == 0f)
            {
                // １秒間に回転させる角度を計算する
                angle = 180f / (coolTime / 2);
            }

            // 現在時間が登録されていなければ
            if (nowTime == 0f)
            {
                // 現在時間を登録する
                nowTime = Time.time;
            }
            // クールタイムの割合を計算する
            timePercentage = (Time.time - nowTime) / coolTime;

            // 画像の右回転
            RotateRightImage(nowTime);

            // 画像の左回転
            RotateLeftImage(coolTime, nowTime);

            // クールタイムに達したら
            if (coolTime < Time.time - nowTime)
            {
                // 初期化
                Initialization();
            }
        }
        else
        {
            // アクティブならば
            if (this.gameObject.activeInHierarchy)
            {
                Debug.Log("スキルのクールタイムのフラグはオフ！");
                // 非アクティブにする
                gameObject.SetActive(false);
            }
        }
    }
    
    void RotateRightImage(float nowTime)
    {
        // 半分に到達する前
        if (timePercentage < 0.5f)
        {
            // 回転させる
            maskImage1.rotation = Quaternion.Euler(0f, 0f, -angle * (Time.time - nowTime));
        }
    }

    void RotateLeftImage(float coolTime, float nowTime)
    {
        if (timePercentage >= 0.5f)
        {
            // 左側の画像を回転させる
            maskImage2.rotation = Quaternion.Euler(0f, 0f, -angle * (Time.time - nowTime - coolTime / 2));
            // 右側の画像を半回転状態にしておく
            maskImage1.rotation = Quaternion.Euler(0f, 0f, 180);
        }
    }

    void Initialization()
    {
        Debug.Log("Call instantiate");
        // パラメータの初期化
        angle = 0f;
        // 回転を元に戻す
        maskImage1.rotation = Quaternion.Euler(Vector3.zero);
        maskImage2.rotation = Quaternion.Euler(Vector3.zero);
        // クールタイムを解除する
        coolTime = 0f;
        nowTime = 0f;
        coolTimeFlag = false;
        // 非アクティブにする
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// クールタイムを設定する関数
    /// </summary>
    /// <param name="coolTime">クールタイム(秒)</param>
    public void SetCoolTime(float coolTime)
    {
        // クールタイムを行っているかどうか
        if (!coolTimeFlag)
        {
            Debug.Log("スキルのクールタイム設定" + coolTime + "秒");
            // クールタイムを設定する
            this.coolTime = coolTime;
            // クールタイムを行うフラグを立てる
            coolTimeFlag = true;
        }
    }
}