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
        statusPoint[0] = PlayerStatus.playerData.str;
        statusPoint[1] = PlayerStatus.playerData.vit;
        statusPoint[2] = PlayerStatus.playerData.intelligence;
        statusPoint[3] = PlayerStatus.playerData.mnd;
        restStatusPoint = PlayerStatus.playerData.statusPoint;
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
        if (restStatusPoint < PlayerStatus.playerData.statusPoint)  //残りステータスポイントがセーブデータのステータスポイントより少なければ（振っていれば）
        {
            int playerStatusPoint = 0;  //プレイヤーのステータスポイント
            
            //どのステータスを変動させるのか
            switch (statusNumber)
            {
                case 0:
                    playerStatusPoint = PlayerStatus.playerData.str;
                    break;
                case 1:
                    playerStatusPoint = PlayerStatus.playerData.vit;
                    break;
                case 2:
                    playerStatusPoint = PlayerStatus.playerData.intelligence;
                    break;
                case 3:
                    playerStatusPoint = PlayerStatus.playerData.mnd;
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
                if (restStatusPoint == PlayerStatus.playerData.statusPoint)
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
        PlayerStatus.playerData.str = statusPoint[0];
        PlayerStatus.playerData.vit = statusPoint[1];
        PlayerStatus.playerData.intelligence = statusPoint[2];
        PlayerStatus.playerData.mnd = statusPoint[3];
        PlayerStatus.playerData.statusPoint = restStatusPoint;

        //セーブする
        PlayerStatus.SavePlayerData();

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
        statusPoint[0] = PlayerStatus.playerData.str;
        statusPoint[1] = PlayerStatus.playerData.vit;
        statusPoint[2] = PlayerStatus.playerData.intelligence;
        statusPoint[3] = PlayerStatus.playerData.mnd;

        restStatusPoint = PlayerStatus.playerData.statusPoint;
        foreach (var status in statusPartitionTexts)
        {
            status.color = Color.black;
        }

        statusText.color = Color.black;
    }
}
