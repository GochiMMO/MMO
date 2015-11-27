using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadCharacterData : MonoBehaviour {
    [SerializeField, Tooltip("キャンバス")]
    GameObject canvas;
    [SerializeField, Tooltip("自分のセーブデータからキャラクターを呼び出した時のボタン")]
    GameObject button;
    [SerializeField, Tooltip("レベルを入力するテキスト")]
    Text lvText;
    [SerializeField, Tooltip("名前を入力するテキスト")]
    Text playerNameText;
    [SerializeField, Tooltip("職業を入力するテキスト")]
    Text jobName;
    [SerializeField, Tooltip("STRを入力するテキスト")]
    Text strText;
    [SerializeField, Tooltip("VITを入力するテキスト")]
    Text vitText;
    [SerializeField, Tooltip("INTを入力するテキスト")]
    Text intText;
    [SerializeField, Tooltip("MNDを入力するテキスト")]
    Text mndText;

    [SerializeField, Tooltip("開始と削除ボタンがあるキャンバス")]
    GameObject startAndDeleteButtonCanvas;
    [SerializeField, Tooltip("キャラクター作成のボタン")]
    GameObject characterCreateButton;

    GameObject backImage;
    RectTransform backImageRect;
    GameObject deleteButtonCanvasInstance;
    bool buttonsInstantiateFlag = false;
    PlayerData[] playerData;    //キャラクターのデータ

    // Use this for initialization
    void Start () {
        // 背景イメージのオブジェクトを取得する
        backImage = canvas.transform.GetChild(0).gameObject;
        // 背景イメージのRectTransformを取得する
        backImageRect = backImage.GetComponent<RectTransform>();
        // ボタンを表示する１つの枠を計算する
        float buttonHeight = (backImageRect.anchorMax.y - backImageRect.anchorMin.y) / 3;
        // セーブデータの数を取得する
        int saveDataNum = PlayerStatus.environmentalSaveData.saveDataNum;
        // セーブデータの名前を格納する変数を定義する
        string[] saveDataName = new string[3];
        // セーブデータの名前を取得する
        saveDataName = PlayerStatus.environmentalSaveData.playerName;

        // セーブデータの数が0でない時
        if (saveDataNum > 0)
        {
            // セーブデータの数だけ領域を確保する
            playerData = new PlayerData[saveDataNum];

            for (int i = 0; i < saveDataNum; i++)
            {
                // セーブデータを読み込む
                playerData[i] = SaveManager.Load<PlayerData>(playerData[i], saveDataName[i]);
                // ボタンをインスタンス化する
                GameObject obj = GameObject.Instantiate(button);
                // ボタンのRectTransformを取得する
                RectTransform buttonRect = obj.GetComponent<RectTransform>();
                // 位置を変更する
                buttonRect.Translate(Vector3.up * buttonRect.rect.height * obj.transform.localScale.y * i, Space.Self);
                // キャンバスの子にする
                obj.transform.SetParent(canvas.transform);
                // サイズを1:1:1に変更する
                buttonRect.localScale = new Vector3(1f, 1f, 1f);
                // 上下のサイズ比を合わせる
                buttonRect.sizeDelta = new Vector2(0, 0);
                // アンカー位置を変更する
                // 上アンカー位置の調整
                buttonRect.anchorMax = new Vector2(buttonRect.anchorMax.x, backImageRect.anchorMax.y - buttonHeight * i);
                // 下アンカー位置の調整
                buttonRect.anchorMin = new Vector2(buttonRect.anchorMin.x, backImageRect.anchorMax.y - buttonHeight * (i + 1));
                // アンカーからのボタンの位置の調整(ぴったりアンカーに合わせるので0,0を入れる)
                buttonRect.anchoredPosition = new Vector2(0f, 0f);

                // テキストをプレイヤーの名前に変更する
                obj.transform.GetChild(0).GetComponent<Text>().text = playerData[i].name;

                // プレイヤーの番号を設定する
                int playerNumber = i;

                // ボタンに押された時の処理を行うようにメソッドを登録する
                obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => pushPlayerButton(playerNumber));
            }
        }

        // セーブデータの数が３人分無ければ
        if (saveDataNum != 3)
        {
            // キャラクター作成ボタンをインスタンス化する
            GameObject charCreateButton = GameObject.Instantiate(characterCreateButton);
            // ボタンのRectTransformを取得する
            RectTransform buttonRect = charCreateButton.GetComponent<RectTransform>();
            // 位置を変更する
            buttonRect.Translate(Vector3.up * buttonRect.rect.height * charCreateButton.transform.localScale.y * saveDataNum, Space.Self);
            // 親子関係を作る
            charCreateButton.transform.SetParent(canvas.transform);
            
            // スケールを調整する
            buttonRect.localScale = new Vector3(1f, 1f, 1f);
            buttonRect.sizeDelta = new Vector2(0, 0);

            // 上アンカー位置の変更
            buttonRect.anchorMax = new Vector2(buttonRect.anchorMax.x, backImageRect.anchorMax.y - buttonHeight * (saveDataNum));
            // 下アンカー位置の変更
            buttonRect.anchorMin = new Vector2(buttonRect.anchorMin.x, backImageRect.anchorMax.y - buttonHeight * (saveDataNum + 1));
            // アンカーにピッタリ合わせるようにする
            buttonRect.anchoredPosition = new Vector2(0f, 0f);
        }
    }


    /// <summary>
    /// キャラクターの名前のボタンを押した時の処理
    /// </summary>
    /// <param name="playerNumber">Show player number.</param>
    public void pushPlayerButton(int playerNumber)
    {
        // モデルの表示処理
        

        // テキストの表示処理
        lvText.text = "Lv " + playerData[playerNumber].Lv.ToString();
        playerNameText.text = "名前 " + playerData[playerNumber].name;
        string job = "";

        // ジョブによって処理分け
        switch (playerData[playerNumber].job)
        {
            case 0:
                job = "アーチャー";
                break;
            case 1:
                job = "ウォーリア";
                break;
            case 2:
                job = "ソーサラー";
                break;
            case 3:
                job = "モンク";
                break;
        }
        jobName.text = "職業 " + job;
        strText.text = "STR " + playerData[playerNumber].str;
        vitText.text = "VIT " + playerData[playerNumber].vit;
        intText.text = "INT " + playerData[playerNumber].intelligence;
        mndText.text = "MND " + playerData[playerNumber].mnd;

        //ボタンキャンバスがインスタンス化していなければ
        if (!buttonsInstantiateFlag)
        {
            // 削除ボタンを表示する
            deleteButtonCanvasInstance = GameObject.Instantiate(startAndDeleteButtonCanvas);
            // ボタンがインスタンス化されたフラグを立てる
            buttonsInstantiateFlag = true;
            // テキストに名前を表示する
            deleteButtonCanvasInstance.transform.GetChild(2).GetComponent<Text>().text = playerNumber.ToString();
        }
        // ボタンがインスタンス化されていたら
        else
        {
            // 削除ボタンのテキストに名前を表示する
            deleteButtonCanvasInstance.transform.GetChild(2).GetComponent<Text>().text = playerNumber.ToString();
        }
        // プレイヤーのステータスを登録する
        PlayerStatus.playerData = playerData[playerNumber];
        // プレイヤーの名前を設定する
        PhotonNetwork.playerName = PlayerStatus.playerData.name;
    }
}