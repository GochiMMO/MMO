using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class SetSkillIcon : MonoBehaviour {
    public static GameObject moveImage;
    public static BoxCollider2D[] col { private set; get; }     // コライダーの配列
    public static GameObject[] skills { private set; get; }     // セットされたスキル

    // Use this for initialization
    void Start () {
        col = gameObject.GetComponents<BoxCollider2D>(); // コライダーを取得
        skills = new GameObject[col.Length];    // コライダーの数だけスキルをセットできる
    }
    
    // Update is called once per frame
    void Update () {
        // イメージ画像が存在していれば
        if (moveImage)
        {
            // ボタンを離した瞬間
            if (Input.GetMouseButtonUp(0))
            {
                // 配列分繰り返す
                for (int i = 0; i < col.Length; i++)
                {
                    // マウスのポジションがコライダーの上にあるとき
                    if (col[i].OverlapPoint(Input.mousePosition))
                    {
                        // もし先に何かスキルがセットされていたら
                        if (skills[i])
                        {
                            // そのスキルを削除する
                            GameObject.Destroy(skills[i]);
                        }
                        
                        // スキル配列にスキルの画像をセットする
                        skills[i] = GameObject.Instantiate(moveImage);
                        GameObject.Destroy(moveImage);
                        
                        // 親を自分に設定する
                        skills[i].transform.SetParent(this.transform);

                        // 位置を調整し、サイズを変更する
                        skills[i].transform.position = col[i].bounds.center;
                        skills[i].transform.localScale = new Vector3(1f, 1f, 1f);

                        // 動かすコンポーネントと複製するコンポーネントをオフにする
                        skills[i].GetComponent<MoveSprite>().enabled = false;

                        // 離れるコンポーネントをオンにする
                        RemoveSprite skillComponent = skills[i].GetComponent<RemoveSprite>();
                        skillComponent.enabled = true;
                        skillComponent.parentColliders = col[i];

                        // 参照を解放する
                        moveImage = null;
                        break;
                    }
                }
            }
        }
    }
}