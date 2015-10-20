using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class MoveWindow : MonoBehaviour {
    //[SerializeField, Tooltip("ウインドウを動かすためのコライダー")]
    BoxCollider2D titleBarCollider;

    bool catchFlag = false;
    Vector2 mouseDeltaPosition;
    Vector2 mousePastPosition;

    // Use this for initialization.
    void Start () {
        // アタッチしているコライダーを取得する
        titleBarCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update () {
        // そのコライダーがクリックされたら
        if (titleBarCollider.OverlapPoint(Input.mousePosition) && Input.GetMouseButtonDown(0))
        {
            // クリックされたフラグを立てる
            catchFlag = true;
        }
        // クリックされており、離されたら
        if(catchFlag && Input.GetMouseButtonUp(0)){
            // クリックされたフラグを折る
            catchFlag = false;
        }
        // クリックされていたら
        if (catchFlag)
        {
            // マウスの移動量を計算する
            mouseDeltaPosition.x = Input.mousePosition.x - mousePastPosition.x;
            mouseDeltaPosition.y = Input.mousePosition.y - mousePastPosition.y;
            // その移動量分移動させる
            gameObject.transform.Translate(mouseDeltaPosition, Space.World);
        }
        // マウスのポジションを取得しておく
        mousePastPosition = Input.mousePosition;
    }
}
