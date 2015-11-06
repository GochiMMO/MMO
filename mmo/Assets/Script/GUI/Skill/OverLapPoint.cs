using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]   //当たり判定オブジェクトを自動でアタッチ
public class OverLapPoint : MonoBehaviour {
    [SerializeField, Tooltip("出すスキルのメニュー")]
    GameObject SkillCanvas;

    // スクロール位置にマウスが存在するかどうか判定するコンポーネントの参照
    static ScrollOnMouseCheck scrollOnMouseCheck = null;
    // 暗くするスプライト画像のコンポーネント
    Image blackSprite;
    // ポップアップしたオブジェクトのインスタンスの参照
    GameObject objectInstance = null;
    // 当り判定用コンポーネントを取得する
    BoxCollider2D boxCol;
    
    // Call once before first Update method.
    void Start () {
        // 当り判定用コンポーネントを取得する
        boxCol = GetComponent<BoxCollider2D>();

        // ☆探す処理は重い、一度なら仕方ないけど何度もさせる予定なら変数に参照を設定する方がよい
        // テクスチャ―（テクスチャ―ってなに？）を表示する
        // transform.Find("dummy").GetComponent<Image>().enabled = true;

        // 暗くするイメージコンポーネントを取得する
        blackSprite = transform.GetChild(1).GetComponent<Image>();

        // 暗くするイメージをオンにする
        blackSprite.enabled = true;

        // スクロール位置にマウスがあるか判定するコンポーネントの参照が設定されていなければ
        if (scrollOnMouseCheck == null)
        {
            // 参照を設定する
            scrollOnMouseCheck = gameObject.transform.parent.parent.parent.gameObject.GetComponent<ScrollOnMouseCheck>();
        }
    }
    
    void Update () {
        // ☆毎フレームこのスクリプト分GetComponentは重い処理、一度設定して使いまわすが吉
        // ScrollOnMouseCheck a = gameObject.transform.parent.parent.parent.gameObject.GetComponent<ScrollOnMouseCheck>();

        // ☆この順番で処理を行うよりは最初に大きい方の当り判定を取り、次に細かい方の当り判定を取るのが正解
        // if (boxCol.OverlapPoint(Input.mousePosition) && scrollOnMouseCheck.GetMouseFlag())

        // スクロールする位置にマウスカーソルが存在するかどうか
        if (scrollOnMouseCheck.GetMouseFlag())
        {
            // スキル画像の上にマウスカーソルが存在するかどうか
            if (boxCol.OverlapPoint(Input.mousePosition))
            {
                // オブジェクト存在しない時
                if (!objectInstance)
                {
                    // オブジェクトのインスタンスを作成する
                    objectInstance = GameObject.Instantiate(SkillCanvas);
                }

                // ☆探す処理は重い処理、変数化したものを呼び出した方が遥かに効率的
                // テクスチャー？（テクスチャ―ってなに？）を非表示
                // transform.Find("dummy").GetComponent<Image>().enabled = false;

                // 暗くするイメージ画像を非表示にする
                blackSprite.enabled = false;
            }
            // スキル画像の上にマウスカーソルが存在しない場合
            else
            {
                // オブジェクトのインスタンスが存在するならば
                if (objectInstance)
                {
                    // オブジェクトを破壊する
                    GameObject.Destroy(objectInstance);

                    // ☆重い処理
                    // テクスチャー（テクスチャ―ってなに？）を表示
                    // transform.Find("dummy").GetComponent<Image>().enabled = true;

                    // 暗くするイメージ画像を表示する
                    blackSprite.enabled = true;
                }
            }
        }
        // スクロール画像の上にマウスカーソルが無ければ
        else
        {
            // オブジェクトのインスタンスが存在するならば
            if (objectInstance)
            {
                // オブジェクトを破壊する
                GameObject.Destroy(objectInstance);

                // ☆重い処理
                // テクスチャー（テクスチャ―ってなに？）を表示
                // transform.Find("dummy").GetComponent<Image>().enabled = true;

                // 暗くするイメージ画像を表示する
                blackSprite.enabled = true;
            }
        }
    }
}
