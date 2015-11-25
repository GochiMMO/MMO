using UnityEngine;
using System.Collections;

public class TranslateWindow : MonoBehaviour {
    [SerializeField, Tooltip("移動させたい座標値")]
    Vector3 targetPosition;
    [SerializeField, Tooltip("移動にかける秒数")]
    float secound = 0.25f;

    Vector3 moveVector;
    Vector3 firstPosition;

    // Use this for initialization
    void Start () {
        // 現在座標を取得しておく
        firstPosition = gameObject.transform.localPosition;
        // 秒数が0以下ならば
        if (secound <= 0)
        {
            // その位置に即座に移動させる
            this.transform.localPosition = targetPosition;
        }
        // 移動量を計算する
        moveVector = targetPosition - gameObject.transform.localPosition;
        // コルーチンを走らせる
        StartCoroutine(Translate());
    }

    /// <summary>
    /// 元の位置に戻る処理
    /// </summary>
    public void Return()
    {
        // 秒数が0以下ならば
        if (secound <= 0)
        {
            // 開始位置に戻す
            this.transform.localPosition = firstPosition;
        }
        // 元の位置と今の座標の差分を計算する
        moveVector = firstPosition - gameObject.transform.localPosition;
        // 移動コルーチンを起動する
        StartCoroutine(Translate());
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Translate()
    {
        // 開始時刻を入れる
        float startTime = Time.time;
        // 終了まで繰り返す
        while (startTime + secound > Time.time)
        {
            // 現在の座標を取得する
            Vector3 position = gameObject.transform.localPosition;
            // 次に移動すべき座標を計算する
            position += (1f / secound) * Time.deltaTime * moveVector;
            // 移動値を計算して加算する
            gameObject.transform.localPosition = position;
            // 繰り返す
            yield return null;
        }
        // 座標を会わせる
        gameObject.transform.localPosition = targetPosition;
        // 処理終了
        yield break;
    }

    
}
