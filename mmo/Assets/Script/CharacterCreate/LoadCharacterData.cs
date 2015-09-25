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
    [SerializeField, Tooltip("開始と削除ボタンがあるキャンバス")]
    GameObject startAndDeleteButtonCanvas;
    [SerializeField, Tooltip("キャラクター作成のボタン")]
    GameObject characterCreateButton;


    GameObject deleteButtonCanvasInstance;
    bool buttonsInstantiateFlag = false;
    PlayerData[] playerData;    //キャラクターのデータ
    // Use this for initialization
    void Start () {
        Debug.Log(PlayerStates.environmentalSaveData.saveDataNum);

        //セーブデータの読み込み
        if (PlayerStates.environmentalSaveData.saveDataNum > 0)
        {
            playerData = new PlayerData[PlayerStates.environmentalSaveData.saveDataNum];    //セーブデータの数だけプレイヤーがいる
            playerData[0] = SaveManager.Load<PlayerData>(playerData[0], PlayerStates.environmentalSaveData.oneSaveDataName);
            GameObject obj = GameObject.Instantiate(button);
            obj.transform.SetParent(canvas.transform);
            obj.transform.GetChild(0).GetComponent<Text>().text = playerData[0].name;
            obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener( ()=>pushPlayerButton(0) );

            Debug.Log(playerData[0].name);

            //キャラクターの数が２人以上
            if (PlayerStates.environmentalSaveData.saveDataNum > 1)
            {
                playerData[1] = SaveManager.Load<PlayerData>(playerData[1], PlayerStates.environmentalSaveData.twoSaveDataName);
                Debug.Log(playerData[1].name);
                GameObject obj1 = GameObject.Instantiate(button);
                obj1.GetComponent<RectTransform>().Translate(Vector3.up * -obj1.GetComponent<RectTransform>().rect.height * obj1.transform.localScale.y, Space.Self);
                obj1.transform.SetParent(canvas.transform);
                obj1.transform.GetChild(0).GetComponent<Text>().text = playerData[1].name;
                obj1.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => pushPlayerButton(1));

                //キャラクターの数が３人
                if (PlayerStates.environmentalSaveData.saveDataNum > 2)
                {
                    playerData[2] = SaveManager.Load<PlayerData>(playerData[2], PlayerStates.environmentalSaveData.threeSaveDataName);
                    Debug.Log(playerData[2].name);
                    GameObject obj2 = GameObject.Instantiate(button);
                    obj2.transform.Translate(Vector3.up * -obj2.GetComponent<RectTransform>().rect.height * 2 * obj2.transform.localScale.y, Space.Self);
                    obj2.transform.SetParent(canvas.transform);
                    obj2.transform.GetChild(0).GetComponent<Text>().text = playerData[2].name;
                    obj2.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => pushPlayerButton(2));
                }
                else
                {
                    GameObject obj2 = GameObject.Instantiate(characterCreateButton);
                    obj2.transform.Translate(Vector3.up * -obj2.GetComponent<RectTransform>().rect.height * 2 * obj2.transform.localScale.y, Space.Self);
                    obj2.transform.SetParent(canvas.transform);
                }
            }
            else
            {
                GameObject obj2 = GameObject.Instantiate(characterCreateButton);
                obj2.transform.Translate(Vector3.up * -obj2.GetComponent<RectTransform>().rect.height * obj2.transform.localScale.y, Space.Self);
                obj2.transform.SetParent(canvas.transform);
            }
        }
        else
        {
            GameObject obj2 = GameObject.Instantiate(characterCreateButton);
            //obj2.transform.Translate(Vector3.up * -obj2.GetComponent<RectTransform>().rect.height * obj2.transform.localScale.y, Space.Self);
            obj2.transform.SetParent(canvas.transform);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerNumber"></param>
    public void pushPlayerButton(int playerNumber)
    {
        //モデルの表示処理


        //テキストの表示処理
        lvText.text = "Lv " + playerData[playerNumber].Lv.ToString();
        playerNameText.text = "名前 " + playerData[playerNumber].name;
        string job = "";
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

        //ボタンキャンバスがインスタンス化していなければ
        if (!buttonsInstantiateFlag)
        {
            deleteButtonCanvasInstance = GameObject.Instantiate(startAndDeleteButtonCanvas);
            buttonsInstantiateFlag = true;
            deleteButtonCanvasInstance.transform.GetChild(2).GetComponent<Text>().text = playerNumber.ToString();//.GetComponentInChildren<Text>().text = playerNumber.ToString();
        }
        else
        {
            //deleteButtonCanvasInstance.transform.GetComponentInChildren<Text>().text = playerNumber.ToString();
            deleteButtonCanvasInstance.transform.GetChild(2).GetComponent<Text>().text = playerNumber.ToString();
        }
    }
    
    // Update is called once per frame
    void Update () {
    
    }
}
