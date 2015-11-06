using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]   //当たり判定オブジェクトを自動でアタッチ
public class ScrollOnMouseCheck : MonoBehaviour {
    // 重なり判定に用いる当り判定コンポーネントの参照
    BoxCollider2D boxCol;

    // マウスが当り判定コンポーネントの上に乗っているかどうか
    bool mouseFlag = false;

    /// <summary>
    /// マウスが当り判定オブジェクトの上に乗っているかどうかを返す関数
    /// </summary>
    /// <returns>true : 乗っている , false : 乗っていない</returns>
    public bool GetMouseFlag()
    {
        return mouseFlag;
    }

    // Use this for initialization
    void Start()
    {
        // 当り判定オブジェクトの参照を取得する
        boxCol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 当り判定コンポーネントの上にマウスの座標があるかどうか
        if (boxCol.OverlapPoint(Input.mousePosition))
        {
            // マウスが乗っているフラグを立てる
            mouseFlag = true;
        }
        else
        {
            // マウスが乗っていないことにする
            mouseFlag = false;
        }
    }
}
