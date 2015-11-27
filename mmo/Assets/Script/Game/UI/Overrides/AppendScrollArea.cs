using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AppendScrollArea : MonoBehaviour, IScrollHandler {
    [SerializeField, Tooltip("スクロールさせるScrollRectクラス")]
    ScrollRect scrollRect;

    /// <summary>
    /// スクロールされた瞬間呼ばれる
    /// </summary>
    /// <param name="data">データクラス</param>
    public void OnScroll(PointerEventData data)
    {
        // スクロールする処理
        scrollRect.OnScroll(data);
    }

}
