using UnityEngine;
using System.Collections;

public class UpdateCoolTime : MonoBehaviour {
    [SerializeField, Tooltip("回転させる画像の右側")]
    Transform maskImage1;
    [SerializeField, Tooltip("回転させる画像の左側")]
    Transform maskImage2;

    public float coolTime;
    float nowTime;

    float timePercentage;
    float angle;

    // Use this for initialization
    void Start () {
        // １秒間に回転させる角度を計算する
        angle = 180f / (coolTime / 2);
    }
    
    // Update is called once per frame
    void Update () {
        // クールタイムが設定されていれば
        if (coolTime != 0f)
        {
            // 現在時間が登録されていなければ
            if (nowTime != 0f)
            {
                // 現在時間を登録する
                nowTime = Time.time;
            }
            // クールタイムの割合を計算する
            timePercentage = (Time.time - nowTime) / coolTime;

            // 半分に到達する前
            if (timePercentage < 0.5f)
            {
                // 回転させる
                maskImage1.Rotate(Vector3.back * (angle * Time.deltaTime));
            }
            else
            {
                // 左側の画像を回転させる
                maskImage2.Rotate(Vector3.back * (angle * Time.deltaTime));
                // 右側の画像がアクティブなら
                if (maskImage1.gameObject.activeInHierarchy)
                {
                    // 非アクティブにする
                    maskImage1.gameObject.SetActive(false);
                }
            }

            // クールタイムに達したら
            if (coolTime < Time.time - nowTime)
            {
                // オブジェクトを削除する
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}