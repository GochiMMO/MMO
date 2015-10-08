using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// BoxCollider2Dを自動アタッチする
[RequireComponent(typeof(BoxCollider2D))]
public class OnOffComponent : MonoBehaviour {
    [SerializeField, Tooltip("オンの時の画像")]
    Sprite onImageSprite;
    [SerializeField, Tooltip("オフの時の画像")]
    Sprite offImageSprite;
    [SerializeField, Tooltip("切り替える画像をのっけるイメージ")]
    Image image;
    [SerializeField, Tooltip("ツールチップのゲームオブジェクト")]
    GameObject tooltipObjectPrefab;
    [SerializeField, Tooltip("オンオフしたいコンポーネントがあるオブジェクト")]
    GameObject componentHaveObject;
    [SerializeField, Tooltip("オンオフしたいコンポーネントのクラスの型")]
    string type;

    Behaviour component;
    GameObject tooltipObjectInstance;
    const int SHOW_TOOLTIP_TIME_SEC = 1;    // ツールチップを出すまでの時間
    float firstTime = 0;
    BoxCollider2D col;
    bool onFlag;
    // Use this for initialization
    void Start () {
        col = GetComponent<BoxCollider2D>();
        // 画像を切り替える
        component = componentHaveObject.GetComponent(type) as Behaviour;
        ChangeOnOffImage();
    }

    void ChangeOnOffImage()
    {
        // コンポーネントがアクティブなら
        if (component.enabled)
        {
            // オンの時の画像に切り替える
            image.sprite = onImageSprite;
        }
        // コンポーネントがノンアクティブなら
        else
        {
            // オフの時の画像に切り替える
            image.sprite = offImageSprite;
        }
    }
    
    // Update is called once per frame
    void Update () {
        // コライダーの上にマウスカーソルが乗ったら
        if (col.OverlapPoint(Input.mousePosition))
        {
            // オンのフラグが立っていなかったら
            if (!onFlag)
            {
                onFlag = true;
                firstTime = Time.time;
            }
            // コライダーの上でクリックされたら
            if (Input.GetMouseButtonDown(0))
            {
                // コンポーネントのオンオフを切り替える
                component.enabled = !component.enabled;
                // 画像を切り替える
                ChangeOnOffImage();
                // オフにする
                onFlag = false;
            }
        }
        else
        {
            // オンのフラグをfalseにする
            onFlag = false;
        }
        // オンにするフラグが立っていたら
        if (onFlag)
        {
            // 時間が経っており、インスタンスが作成されていなければ
            if (Time.time - firstTime > SHOW_TOOLTIP_TIME_SEC && !tooltipObjectInstance)
            {
                // ツールチップを表示する
                tooltipObjectInstance = GameObject.Instantiate(tooltipObjectPrefab);
                // ツールチップを移動する
                tooltipObjectInstance.transform.GetChild(0).Translate(Input.mousePosition);
            }
        }
        // オフにするフラグが立っており、オブジェクトが生成されていたら
        else if (tooltipObjectInstance)
        {
            GameObject.Destroy(tooltipObjectInstance);
        }
    }
}
