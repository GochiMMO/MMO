using UnityEngine;
using System.Collections;

public class UpdateSkillCoolTime : MonoBehaviour {
    [SerializeField, Tooltip("回転させる画像の右側")]
    Transform maskImage1;
    [SerializeField, Tooltip("回転させる画像の左側")]
    Transform maskImage2;

    /// <summary>
    /// スキルを使うスクリプトを登録しておく
    /// </summary>
    public UseSkill useSkill;

    // クールタイムを行っているフラグ
    bool coolTimeFlag = false;

    // クールタイム
    public float coolTime = 0f;

    // 現在時間
    float startTime = 0f;

    // どのくらいの割合回転させているか
    float timePercentage = 0f;

    // 回転角
    float angle = 0f;

    // このスクリプトが有効化されたら
    void OnEnable()
    {
        // 画像を非表示にする
        maskImage1.gameObject.SetActive(false);
        maskImage2.gameObject.SetActive(false);
        // クールタイムを同期する
        SyncCoolTime();
    }

    // スクリプトが無効化されたら
    void OnDisable()
    {
        // クールタイムを行うフラグが立っていたら
        if (coolTimeFlag)
        {
            // 初期化を行う
            Initialization();
        }
    }

    /// <summary>
    /// クールタイムを同期する関数
    /// </summary>
    void SyncCoolTime()
    {
        // 設定されたスキルがクールタイム中であり、自分がクールタイム中でなければ
        if (SyncSkillCoolTime.IsCoolTime(useSkill.skillID) && !coolTimeFlag || !SyncSkillCoolTime.IsSameCoolTime(useSkill.skillID, coolTime))
        {
            // 初期化処理を行う
            Initialization();
            // クールタイムをセットする
            this.coolTime = SyncSkillCoolTime.GetCoolTime(useSkill.skillID);
            // 始まった時間を取得する
            this.startTime = SyncSkillCoolTime.GetStartTime(useSkill.skillID);
            // クールタイムを行うフラグをオンにする
            this.coolTimeFlag = true;
            // スキルの方にもクールタイムを行うフラグを立てる
            useSkill.skillCoolTimeFlag = true;
            // 画像を表示する
            maskImage1.gameObject.SetActive(true);
            maskImage2.gameObject.SetActive(true);
        }
        // スキルが外れた時
        if (!useSkill)
        {
            // クールタイムを行うフラグが立っていたら
            if (coolTimeFlag)
            {
                // 初期化処理
                Initialization();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // クールタイムを同期する
        SyncCoolTime();
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
            if (startTime == 0f)
            {
                // 現在時間を登録する
                startTime = Time.time;
            }
            // クールタイムの割合を計算する
            timePercentage = (Time.time - startTime) / coolTime;

            // 画像の右回転
            RotateRightImage();

            // 画像の左回転
            RotateLeftImage();

            // クールタイムに達したら
            if (coolTime < Time.time - startTime)
            {
                // 初期化
                Initialization();
            }
        }
    }
    
    /// <summary>
    /// 右側の画像を回転させる処理
    /// </summary>
    void RotateRightImage()
    {
        // 半分に到達する前
        if (timePercentage < 0.5f)
        {
            // 回転させる
            maskImage1.rotation = Quaternion.Euler(0f, 0f, -angle * (Time.time - startTime));
        }
    }

    /// <summary>
    /// 左側の画像を回転させる処理
    /// </summary>
    void RotateLeftImage()
    {
        if (timePercentage >= 0.5f)
        {
            // 左側の画像を回転させる
            maskImage2.rotation = Quaternion.Euler(0f, 0f, -angle * (Time.time - startTime - coolTime / 2));
            // 右側の画像を半回転状態にしておく
            maskImage1.rotation = Quaternion.Euler(0f, 0f, 180);
        }
    }

    void Initialization()
    {
        // パラメータの初期化
        angle = 0f;
        // 回転を元に戻す
        maskImage1.rotation = Quaternion.Euler(Vector3.zero);
        maskImage2.rotation = Quaternion.Euler(Vector3.zero);
        // クールタイムを解除する
        coolTime = 0f;
        startTime = 0f;
        coolTimeFlag = false;
        // クールタイムのフラグを折る
        useSkill.skillCoolTimeFlag = false;

        // 画像のオブジェクトを非アクティブにする
        maskImage1.gameObject.SetActive(false);
        maskImage2.gameObject.SetActive(false);

        // 非アクティブにする
        // this.gameObject.SetActive(false);

    }
}