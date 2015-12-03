using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnMouseAndTransparent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField, Tooltip("透過にかかる時間")]
    float second = 0.2f;
    [SerializeField, Tooltip("透過の値")]
    float alpha = 0.4f;

    RawImage transparentImage;

    float firstAlpha;

    /// <summary>
    /// Updateを行う前の最初のフレームに呼び出される関数
    /// </summary>
    void Start()
    {
        // 画像のスクリプトを取得する
        transparentImage = GetComponent<RawImage>();
        // 最初の透過度を取得しておく
        firstAlpha = transparentImage.color.a;
    }

    /// <summary>
    /// ポインターが入ってきたら呼び出されるメソッド
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerEnter(PointerEventData data)
    {
        // 透明化させるコルーチンをスタートする
        StartCoroutine(Transparent());
    }

    /// <summary>
    /// マウスポインタ―が外れた時に呼び出されるメソッド
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerExit(PointerEventData data)
    {
        // 実体化させるコルーチンをスタートする
        StartCoroutine(Substantiation());
    }

    /// <summary>
    /// 透明なものを元の透過度に戻す処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Substantiation()
    {
        // 開始時刻を取得する
        float startTime = Time.time;
        // 色を取得しておく構造体
        Color color;
        // 時間が来るまで繰り返す
        while (startTime + second > Time.time)
        {
            // カラーを作成する
            color = transparentImage.color;
            // 透過度を引く(透明に近づける
            color.a += (1f / second) * (firstAlpha - alpha) * Time.deltaTime;
            // 透明度を反映させる
            transparentImage.color = color;
            // 繰り返す
            yield return null;
        }
        // 透明度を合わせる
        color = transparentImage.color;
        color.a = firstAlpha;
        transparentImage.color = color;
        // コルーチンから抜ける
        yield break;
    }

    /// <summary>
    /// 透明化させるスクリプト
    /// </summary>
    /// <returns></returns>
    IEnumerator Transparent()
    {
        // 開始時刻を取得する
        float startTime = Time.time;
        // 色を取得しておく構造体
        Color color;
        // 時間が来るまで繰り返す
        while (startTime + second > Time.time)
        {
            // カラーを作成する
            color = transparentImage.color;
            // 透過度を引く(透明に近づける
            color.a -= (1f / second) * (firstAlpha - alpha) * Time.deltaTime;
            // 透明度を反映させる
            transparentImage.color = color;
            // 繰り返す
            yield return null;
        }
        // 透明度を合わせる
        color = transparentImage.color;
        color.a = alpha;
        transparentImage.color = color;
        // コルーチンから抜ける
        yield break;
    }
}
