using UnityEngine;
using System.Collections;

public class UpdateItemCoolTime : MonoBehaviour {
    [SerializeField, Tooltip("回転させる画像の右側")]
    Transform maskImage1;
    [SerializeField, Tooltip("回転させる画像の左側")]
    Transform maskImage2;

    static bool coolTimeFlag = false;

    public static float coolTime = 0f;

    static float nowTime = 0f;

    float timePercentage = 0f;
    float angle = 0f;

    void Start()
    {
    }

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
    
    void RotateRightImage()
    {
        // 半分に到達する前
        if (timePercentage < 0.5f)
        {
            // 回転させる
            maskImage1.rotation = Quaternion.Euler(0f, 0f, 360f * -timePercentage);
        }
    }

    void RotateLeftImage()
    {
        if (timePercentage >= 0.5f)
        {
            // 左側の画像を回転させる
            maskImage2.rotation = Quaternion.Euler(0f, 0f, 360f * (-timePercentage + 0.5f));
            // 右側の画像を半回転状態にしておく
            maskImage1.rotation = Quaternion.Euler(0f, 0f, -180f);
        }
    }

    void Initialization()
    {
        Debug.Log("Call instantiate");
        // パラメータの初期化
        angle = 0f;
        // クールタイムを解除する
        UpdateItemCoolTime.coolTime = 0f;
        UpdateItemCoolTime.nowTime = 0f;
        UpdateItemCoolTime.coolTimeFlag = false;
        UseItem.itemCoolTimeFlag = false;
        // 非アクティブにする
        this.gameObject.SetActive(false);
    }

    public void SetCoolTime(float coolTime)
    {
        if (!UpdateItemCoolTime.coolTimeFlag)
        {
            Debug.Log("アイテムのクールタイム設定" + coolTime + "秒");
            UpdateItemCoolTime.coolTime = coolTime;
            UpdateItemCoolTime.coolTimeFlag = true;
        }
    }

    
}
