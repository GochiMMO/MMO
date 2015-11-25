using UnityEngine;
using System.Collections;

public class ScaleStatusMenu : MenuObjects {
    [SerializeField, Tooltip("拡縮にかかる時間")]
    float second = 0.25f;
    [SerializeField, Tooltip("サイズはいくつにするか")]
    float size = 1f;

    // Use this for initialization
    void Start () {
        // 大きさを0に設定しておく
        gameObject.transform.localScale = Vector3.zero;
        // 時間が0以下ならば
        if (second <= 0)
        {
            // スケールを１に合わせる
            gameObject.transform.localScale = Vector3.one * size;
            // 処理から抜ける
            return;
        }
        // 拡大メソッドをスタートする
        StartCoroutine(Expansion());
    }

    /// <summary>
    /// 削除する関数
    /// </summary>
    public override void Destroy()
    {
        // 縮小開始
        StartCoroutine(Reduction());
        // 移動するコンポーネントを取得する
        TranslateWindow translateWindow = gameObject.GetComponent<TranslateWindow>();
        // もし移動するコンポーネントも付随していたら
        if (translateWindow)
        {
            // 元の一に戻る処理も行う
            translateWindow.Return();
        }
    }

    /// <summary>
    /// 拡大させる
    /// </summary>
    /// <returns></returns>
    IEnumerator Expansion()
    {
        // 開始時間を設定する
        float startTime = Time.time;
        // 既定時間まで繰り返す
        while (startTime + second > Time.time)
        {
            // スケールを取得する
            Vector3 scale = gameObject.transform.localScale;
            // 拡大させる
            scale.x += (1f / second) * Time.deltaTime * size;
            // 拡大させる
            scale.y = scale.x;
            // 適用させる
            gameObject.transform.localScale = scale;
            // 処理を繰り返す
            yield return null;
        }
        // 拡大値を合わせる
        gameObject.transform.localScale = Vector3.one * size;
        // 処理から抜ける
        yield break;
    }

    /// <summary>
    /// 縮小スタート
    /// </summary>
    public void Reduct()
    {
        // コルーチンを起動する
        StartCoroutine(Reduction());
    }

    /// <summary>
    /// 縮小するコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator Reduction()
    {
        // 開始時間を設定する
        float startTime = Time.time;
        // 既定時間まで繰り返す
        while (startTime + second > Time.time)
        {
            // スケールを取得する
            Vector3 scale = gameObject.transform.localScale;
            // 縮小させる
            scale.x -= (1f / second) * Time.deltaTime * size;
            // xの値に合わせる
            scale.y = scale.x;
            // 適用させる
            gameObject.transform.localScale = scale;
            // 処理を繰り返す
            yield return null;
        }
        // 拡大値を合わせる
        gameObject.transform.localScale = Vector3.zero;
        // オブジェクトを削除
        GameObject.Destroy(gameObject.transform.root.gameObject);
        // 処理から抜ける
        yield break;
    }
}
