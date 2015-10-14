using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// When mouse over this collider and click it, dupricate a sprite.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]   // BoxCollider2Dをセットする
public class DuplicateSprite : MonoBehaviour {
    BoxCollider2D col;  // コライダー
    // Use this for initialization
    void Start () {
        col = gameObject.GetComponent<BoxCollider2D>();     // BoxCollider2Dコンポーネントを取得する
    }
    
    // Update is called once per frame
    void Update () {
        // コライダーがクリックされたら
        if (col.OverlapPoint(Input.mousePosition) && Input.GetMouseButtonDown(0))
        {
            GameObject obj = GameObject.Instantiate(this.gameObject);    // 自分を複製する
            obj.AddComponent<MoveSprite>();     // 移動用スクリプトをアタッチする
            obj.transform.SetParent(this.transform.parent); // 親を付ける
            obj.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
