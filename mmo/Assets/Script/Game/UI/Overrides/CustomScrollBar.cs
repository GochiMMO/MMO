using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomScrollBar : MonoBehaviour{
    [SerializeField, Tooltip("スクロールさせるScrollRectクラス")]
    ScrollRect scrollRect;

    /// <summary>
    /// 上でマウスホイールをコロコロする処理
    /// </summary>
    /// <param name="eventData"></param>
    public void OnScroll(PointerEventData eventData)
    {
        // スクロールする処理
        scrollRect.OnScroll(eventData);
    }

}
