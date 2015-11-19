using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// When mouse over this collider and click it, dupricate a sprite.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]   // BoxCollider2Dをセットする
public class DuplicateSprite : MonoBehaviour {
    BoxCollider2D col;  // コライダー
    // キャンバスオブジェクト
    static Canvas mainCanvas;
    // スクロールのコンポーネント
    static ScrollOnMouseCheck scrollOnMouseCheck = null;

    [SerializeField, Tooltip("スキルのフラグ")]
    bool skillFlag = false;
    // Use this for initialization
    void Start () {
        col = gameObject.GetComponent<BoxCollider2D>();     // BoxCollider2Dコンポーネントを取得する
        // キャンバスが設定されていなければ
        if(!mainCanvas)
        // キャンバスを探して設定する
        mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        // スクロールのコンポーネントを取得する
        if (scrollOnMouseCheck == null && skillFlag)
        {
            Debug.Log("set");
            scrollOnMouseCheck = gameObject.transform.parent.parent.parent.parent.GetComponent<ScrollOnMouseCheck>();
        }
    }

    // Call this method once per every frame.
    void Update()
    {
        // スキルのフラグがオフならば
        if (!skillFlag)
        {
            // クリックされたら
            if (Input.GetMouseButtonDown(0))
            {
                // その画像の上にマウスの座標があるなら
                if (col.OverlapPoint(Input.mousePosition))
                {
                    GameObject obj = GameObject.Instantiate(this.gameObject);    // 自分を複製する
                    obj.AddComponent<MoveSprite>();     // 移動用スクリプトをアタッチする
                    obj.GetComponent<DuplicateSprite>().enabled = false;    // 複製用スクリプトをオフにする
                    obj.AddComponent<RemoveSprite>().enabled = false;       // 削除用スクリプトをアタッチし、オフにする
                    //obj.transform.SetParent(this.transform.parent); // 親を付ける
                    obj.transform.SetParent(mainCanvas.transform);
                    obj.transform.localScale = new Vector3(1f, 1f, 1f);
                    SetSkillIcon.moveImage = obj;
                }
            }
        }
        // スキルのフラグがオンならば
        else
        {
            // クリックされたら
            if (Input.GetMouseButtonDown(0))
            {
                if (scrollOnMouseCheck.GetMouseFlag())
                {
                    // その画像の上にマウスの座標があるなら
                    if (col.OverlapPoint(Input.mousePosition))
                    {
                        GameObject obj = GameObject.Instantiate(this.gameObject);    // 自分を複製する
                        obj.AddComponent<MoveSprite>();     // 移動用スクリプトをアタッチする
                        obj.GetComponent<DuplicateSprite>().enabled = false;    // 複製用スクリプトをオフにする
                        obj.AddComponent<RemoveSprite>().enabled = false;       // 削除用スクリプトをアタッチし、オフにする
                        //obj.transform.SetParent(this.transform.parent); // 親を付ける
                        obj.transform.SetParent(mainCanvas.transform);  // 親を付ける
                        obj.GetComponent<UseSkill>().skillID = transform.parent.GetComponent<OverLapPoint>().SkillCanvas.GetComponent<SkillText>().SkillID;
                        obj.transform.localScale = new Vector3(1f, 1f, 1f);
                        SetSkillIcon.moveImage = obj;
                    }
                }
            }
        }
    }
}