using UnityEngine;
using System.Collections;

public class CharacterSelect : MonoBehaviour {
    [SerializeField, Tooltip("開始するかどうか聞くウインドウ")]
    GameObject ynWindow;
    [SerializeField, Tooltip("キャラクタークリエイト画面のシーンの名前")]
    string characterCreateSceneName;
    [SerializeField, Tooltip("削除するかどうか聞くウインドウ")]
    GameObject askDeleteWindow;

    // Use this for initialization
    void Start () {
        // コンフィグファイルを読み込む
        PlayerStatus.Init();
    }

    /// <summary>
    /// Popup "ask Delete Window" and entry a character destroy method on the window's button.
    /// </summary>
    public void PushDeleteButton()
    {
        // 本当に削除してもいいですか？のウィンドウを表示する
        var obj = GameObject.Instantiate(askDeleteWindow);
        // 子オブジェクトの数だけ繰り返す
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            // 子オブジェクトを取得する
            GameObject childObj = obj.transform.GetChild(i).gameObject;
            // タグがYesButtonに設定されていたら
            if (childObj.tag == "YesButton")
            {
                // そのボタンのコンポーネントを取得する
                var btn = childObj.GetComponent<UnityEngine.UI.Button>();
                // プレイヤーの番号を取得する
                int playerNumber = int.Parse(gameObject.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text);
                // セーブデータを削除する関数を登録
                btn.onClick.AddListener(() => { StaticMethods.DeletePlayerSaveData(playerNumber); });
                // シーンを再読み込みする
                btn.onClick.AddListener(() => { PhotonNetwork.LoadLevel(Application.loadedLevel); });
            }
        }
    }

    /// <summary>
    /// Load a level, name is "CharacterCreate".
    /// </summary>
    public void LoadCharacterCreate()
    {
        PhotonNetwork.LoadLevel(characterCreateSceneName);
    }

    /// <summary>
    /// Create instance of yes no window.
    /// </summary>
    public void EnabledYNWindow()
    {
        Instantiate(ynWindow, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {
    
    }
}