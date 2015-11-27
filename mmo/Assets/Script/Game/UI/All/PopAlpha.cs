using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class PopAlpha : MenuObjects {
    [SerializeField, Tooltip("何秒で透明度を変えるか")]
    float second = 0.2f;

    CanvasGroup canvasGroup;

    // Use this for initialization
    void Start () {
        // コンポーネントを取得する
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        // 秒数が0以下に設定されていたら
        if (second <= 0f)
        {
            // 透明度を1に設定する
            canvasGroup.alpha = 1f;
            // 処理から抜ける
            return;
        }
        // 透明度を0にする
        canvasGroup.alpha = 0f;
        // 透明度を1に変えるコルーチンを走らせる
        StartCoroutine(Substantiation());
    }

    /// <summary>
    /// 削除する関数
    /// </summary>
    public override void Destroy()
    {
        // 透明化する
        StartCoroutine(Transparency());
    }

    /// <summary>
    /// 透明→実体にする
    /// </summary>
    /// <returns></returns>
    IEnumerator Substantiation()
    {
        // 開始時刻を取得する
        float startTime = Time.time;
        // 既定時間が来るまで繰り返す
        while (startTime + second > Time.time)
        {
            // 透明度を加算する
            canvasGroup.alpha += (1f / second) * Time.deltaTime;
            // ループする
            yield return null;
        }
        // 透明度を1にする
        canvasGroup.alpha = 1f;
        // 処理から抜ける
        yield break;
    }

    /// <summary>
    /// 透明化する
    /// </summary>
    /// <returns></returns>
    IEnumerator Transparency()
    {
        // 開始時刻を取得する
        float startTime = Time.time;
        // 既定時間が来るまで繰り返す
        while (startTime + second > Time.time)
        {
            // 透明度を加算する
            canvasGroup.alpha -= (1f / second) * Time.deltaTime;
            // ループする
            yield return null;
        }
        // 透明度を0にする
        canvasGroup.alpha = 0f;
        // オブジェクトが存在するならば
        if (gameObject)
        {
            // 削除する
            GameObject.Destroy(gameObject);
        }
        // 処理から抜ける
        yield break;
    }
}
