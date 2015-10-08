using UnityEngine;
using UnityEngine.UI;

public class StatusPartitionSystem : MonoBehaviour {
    [SerializeField, Tooltip("ステータスを表示するテキスト群")]
    GameObject texts;
    [SerializeField, Tooltip("スキルポイントを表示しておくテキスト")]
    Text statusText;

    Text[] statusPartitionTexts;    //ステータスを表示するテキスト

    int[] statusPoint;      //ステータスを表示させておくテキスト
    int restStatusPoint;    //残りステータスポイント

    // Use this for initialization
    void Start () {
        statusPartitionTexts = new Text[4];     
        statusPoint = new int[4];       //メモリ領域を確保

        //ステータスポイントを表示させるテキストを格納する
        for (int i = 0; i < 4; i++)
        {
            statusPartitionTexts[i] = texts.transform.GetChild(i).gameObject.GetComponent<Text>();
        }

        //それぞれ格納しておく
        statusPoint[0] = PlayerStates.playerData.str;
        statusPoint[1] = PlayerStates.playerData.vit;
        statusPoint[2] = PlayerStates.playerData.intelligence;
        statusPoint[3] = PlayerStates.playerData.mnd;
        restStatusPoint = PlayerStates.playerData.statusPoint;
    }
    
    // Update is called once per frame
    void Update () {
        // 表示処理
        for (int i = 0; i < 4; i++)
        {
            statusPartitionTexts[i].text = statusPoint[i].ToString();
        }
        statusText.text = restStatusPoint.ToString();
    }

    /// <summary>
    /// Push a plus button method of status point.
    /// </summary>
    /// <param name="statusNumber">Kind of status number.</param>
    public void PushPlusButton(int statusNumber)
    {
        if (restStatusPoint > 0)    //ステータスポイントが余っていたら
        {
            statusPoint[statusNumber]++;    //ステータスを増やす
            restStatusPoint--;              //残りステータスポイントを減らす
            statusText.color = Color.red;
            statusPartitionTexts[statusNumber].color = Color.green;
        }
    }

    /// <summary>
    /// Push a minus button method of status point.
    /// </summary>
    /// <param name="statusNumber">Kind of status number.</param>
    public void PushMinusButton(int statusNumber)
    {
        if (restStatusPoint < PlayerStates.playerData.statusPoint)  //残りステータスポイントがセーブデータのステータスポイントより少なければ（振っていれば）
        {
            int playerStatusPoint = 0;  //プレイヤーのステータスポイント
            
            //どのステータスを変動させるのか
            switch (statusNumber)
            {
                case 0:
                    playerStatusPoint = PlayerStates.playerData.str;
                    break;
                case 1:
                    playerStatusPoint = PlayerStates.playerData.vit;
                    break;
                case 2:
                    playerStatusPoint = PlayerStates.playerData.intelligence;
                    break;
                case 3:
                    playerStatusPoint = PlayerStates.playerData.mnd;
                    break;
            }

            if (statusPoint[statusNumber] > playerStatusPoint)  //ステータスポイントが振ってあれば
            {
                statusPoint[statusNumber]--;    //ステータスを振った分だけ戻す
                restStatusPoint++;      //残りステータスポイントを増やす
                if (statusPoint[statusNumber] == playerStatusPoint)
                {
                    statusPartitionTexts[statusNumber].color = Color.black;
                    
                }
                if (restStatusPoint == PlayerStates.playerData.statusPoint)
                {
                    statusText.color = Color.black;
                }
            }
        }
        
    }

    /// <summary>
    /// Push a decide button method.
    /// </summary>
    public void DecideButton()
    {
        //セーブデータに一時記憶した値を入れる
        PlayerStates.playerData.str = statusPoint[0];
        PlayerStates.playerData.vit = statusPoint[1];
        PlayerStates.playerData.intelligence = statusPoint[2];
        PlayerStates.playerData.mnd = statusPoint[3];
        PlayerStates.playerData.statusPoint = restStatusPoint;

        //セーブする
        PlayerStates.SavePlayerData();

        foreach (var status in statusPartitionTexts)
        {
            status.color = Color.black;
        }

        statusText.color = Color.black;

    }

    /// <summary>
    /// Push a chancell button method.
    /// </summary>
    public void ChancellButton()
    {
        // ステータスポイントをセーブデータから呼び出し直す
        statusPoint[0] = PlayerStates.playerData.str;
        statusPoint[1] = PlayerStates.playerData.vit;
        statusPoint[2] = PlayerStates.playerData.intelligence;
        statusPoint[3] = PlayerStates.playerData.mnd;

        restStatusPoint = PlayerStates.playerData.statusPoint;
        foreach (var status in statusPartitionTexts)
        {
            status.color = Color.black;
        }

        statusText.color = Color.black;
    }
}
