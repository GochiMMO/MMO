using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopStatusMenu : MonoBehaviour {
    /// <summary>
    /// 表示する画像
    /// </summary>
    [SerializeField, Tooltip("徐々に表示させたい画像")]
    Image image;
    /// <summary>
    /// 表示するまでにかかる時間
    /// </summary>
    [SerializeField, Tooltip("表示するまでにかかる時間")]
    float showSec = 1f;

    // Use this for initialization
    void Start () {
        // 画像を非表示の状態にする
        image.fillAmount = 0f;
        // 時間が0以下ならば
        if (showSec <= 0)
        {
            // 画像を表示する
            image.fillAmount = 1f;
            // 処理から抜ける
            return;
        }
        // 表示するコルーチンを走らせる
        StartCoroutine(Showing());
    }

    /// <summary>
    /// 画像を徐々に表示するコルーチン
    /// </summary>
    /// <returns>反復子</returns>
    IEnumerator Showing()
    {
        // 開始時間を取得する
        float startTime = Time.time;
        // 指定した時間になるまで繰り返す
        while (startTime + showSec > Time.time)
        {
            // 画像の表示を少し進める
            image.fillAmount += (1f / showSec) * Time.deltaTime;
            // 一度処理から抜ける
            yield return null;
        }
        // 画像の表示を完全に出す
        image.fillAmount = 1f;
        // 処理から抜ける
        yield break;
    }

    /// <summary>
    /// 閉じる処理
    /// </summary>
    public void Close()
    {
        // 閉じる処理を開始する
        StartCoroutine(Closing());
    }

    /// <summary>
    /// 閉じる処理
    /// </summary>
    /// <returns>反復子</returns>
    IEnumerator Closing()
    {
        // 開始時間を取得する
        float startTime = Time.time;
        // 指定した時間になるまで繰り返す
        while (startTime + showSec > Time.time)
        {
            // 画像の表示を少し進める
            image.fillAmount -= (1f / showSec) * Time.deltaTime;
            // 一度処理から抜ける
            yield return null;
        }
        // 画像の表示を完全に出す
        image.fillAmount = 0f;
        // 自身を削除する
        GameObject.Destroy(this.gameObject.transform.root.gameObject);
        // 処理から抜ける
        yield break;
    }
}
