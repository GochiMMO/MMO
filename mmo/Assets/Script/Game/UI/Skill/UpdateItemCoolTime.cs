using UnityEngine;
using System.Collections;

public class UpdateItemCoolTime : MonoBehaviour {
    [SerializeField, Tooltip("回転させる画像の右側")]
    Transform maskImage1;
    [SerializeField, Tooltip("回転させる画像の左側")]
    Transform maskImage2;

    // クールタイムかどうかのフラグ
    static bool coolTimeFlag = false;

    // クールタイム
    public static float coolTime = 0f;

    // クールタイムがスタートした時間
    static float nowTime = 0f;
    // クールタイムの割合
    float timePercentage = 0f;

    /// <summary>
    /// Call this method, when this component is disabled.
    /// </summary>
    void OnDisable()
    {
        // 回転を元に戻す
        maskImage1.rotation = Quaternion.Euler(0f, 0f, 0f);
        maskImage2.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // クールタイムのフラグが立っていたら
        if (UpdateItemCoolTime.coolTimeFlag)
        {
            // 現在時間が登録されていなければ
            if (nowTime == 0f)
            {
                // 現在時間を登録する
                nowTime = Time.time;
            }
            // クールタイムの割合を計算する
            timePercentage = (Time.time - nowTime) / coolTime;

            // 画像の右回転
            RotateRightImage();

            // 画像の左回転
            RotateLeftImage();

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
                Debug.Log("アイテムのクールタイムのフラグはオフ！");
                // 非アクティブにする
                gameObject.SetActive(false);
            }
        }
    }
    
    /// <summary>
    /// Set rotation of right black image.
    /// </summary>
    void RotateRightImage()
    {
        // 半分に到達する前
        if (timePercentage < 0.5f)
        {
            // 回転させる
            maskImage1.rotation = Quaternion.Euler(0f, 0f, 360f * -timePercentage);
        }
    }

    /// <summary>
    /// Set rotation of left black image.
    /// </summary>
    void RotateLeftImage()
    {
        // クールタイムの割合が半分を超えている場合
        if (timePercentage >= 0.5f)
        {
            // 左側の画像を回転させる
            maskImage2.rotation = Quaternion.Euler(0f, 0f, 360f * (-timePercentage + 0.5f));
            // 右側の画像を半回転状態にしておく
            maskImage1.rotation = Quaternion.Euler(0f, 0f, -180f);
        }
    }

    /// <summary>
    /// Initialization.
    /// </summary>
    void Initialization()
    {
        Debug.Log("Call instantiate");
        // クールタイムを解除する
        UpdateItemCoolTime.coolTime = 0f;
        UpdateItemCoolTime.nowTime = 0f;
        UpdateItemCoolTime.coolTimeFlag = false;
        UseItem.itemCoolTimeFlag = false;
        // 非アクティブにする
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Set cool time and run cool time.
    /// </summary>
    /// <param name="coolTime">Cool time(sec)</param>
    public void SetCoolTime(float coolTime)
    {
        // クールタイムを行うフラグが立っていなければ
        if (!UpdateItemCoolTime.coolTimeFlag)
        {

            Debug.Log("アイテムのクールタイム設定" + coolTime + "秒");
            // クールタイムを設定する
            UpdateItemCoolTime.coolTime = coolTime;
            // クールタイムを行うフラグを立てる
            UpdateItemCoolTime.coolTimeFlag = true;
        }
    }

    
}
