using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

// BoxCollider2Dを自動的にアタッチする
[RequireComponent(typeof(BoxCollider2D))]
public class MoveWindow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    //[SerializeField, Tooltip("ウインドウを動かすためのコライダー")]
    BoxCollider2D titleBarCollider;

    // ウィンドウをつかんでるかのフラグ
    bool catchFlag = false;
    // マウスの前からの移動量
    Vector2 mouseDeltaPosition;
    // マウスの前フレーム移動位置
    Vector2 mousePastPosition;

    // Use this for initialization.
    void Start () {
        // アタッチしているコライダーを取得する
        titleBarCollider = GetComponent<BoxCollider2D>();
        // 現在のフレームのマウス座標を入れておく
        mousePastPosition = Input.mousePosition;
    }

    /// <summary>
    /// ボタンが離された時に呼ばれるイベント
    /// </summary>
    /// <param name="eventData">イベントのデータ</param>
    public void OnEndDrag (PointerEventData eventData)
    {
        // 掴まれているフラグにfalseを入れる
        catchFlag = false;
        Debug.Log("mouseUp");
    }

    /// <summary>
    /// ドラッグの開始処理
    /// </summary>
    /// <param name="eventData">データ</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // コライダー上で左クリックされたら
        if (eventData.button == PointerEventData.InputButton.Left && titleBarCollider.OverlapPoint(eventData.position))
        {
            // マウスの座標を取得する
            mousePastPosition = eventData.position;
            // クリックされたフラグを格納する
            catchFlag = true;
            Debug.Log("mouseDown");
        }
    }

    /// <summary>
    /// ドラッグ中の処理
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        // コライダーが掴まれていたら
        if (catchFlag)
        {
            // マウスの移動量を計算する
            mouseDeltaPosition.x = Input.mousePosition.x - mousePastPosition.x;
            mouseDeltaPosition.y = Input.mousePosition.y - mousePastPosition.y;
            // その移動量分移動させる
            gameObject.transform.Translate(mouseDeltaPosition, Space.World);
            // マウスのポジションを取得しておく
            mousePastPosition = Input.mousePosition;
        }
    }
}
