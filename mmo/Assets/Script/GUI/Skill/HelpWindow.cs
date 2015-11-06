using UnityEngine;
using System.Collections;

// BoxCollider2Dを自動でアタッチする
[RequireComponent(typeof(BoxCollider2D))]
public class HelpWindow : MonoBehaviour {
    [SerializeField, Tooltip("出すスキルのメニュー")]
    GameObject SkillCanvas;

    // メニューのインスタンスの参照
    GameObject objectInstance = null;

    // 当り判定コンポーネントの参照
    BoxCollider2D boxCol;

    // Use this for initialization
    void Start()
    {
        // 当り判定コンポーネントの参照を取得する
        boxCol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 当り判定オブジェクトの上にマウスの座標があったら
        if (boxCol.OverlapPoint(Input.mousePosition))
        {
            // オブジェクトのインスタンスが作成されていなければ
            if (!objectInstance)
            {
                // インスタンスを作成する
                objectInstance = GameObject.Instantiate(SkillCanvas);
            }
        }
        // 当り判定オブジェクトの上にマウスの座標が無かったら
        else
        {
            // オブジェクトのインスタンスが存在する場合
            if (objectInstance)
            {
                // 削除する
                GameObject.Destroy(objectInstance);
            }
        }
    }
}
