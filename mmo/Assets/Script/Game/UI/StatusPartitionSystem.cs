using UnityEngine;
using UnityEngine.UI;

public class StatusPartitionSystem : MonoBehaviour {
    [SerializeField, Tooltip("ステータスを表示するテキスト群")]
    GameObject texts;
    [SerializeField, Tooltip("スキルポイントを表示しておくテキスト")]
    Text statusText;

    Text[] statusPartitionTexts;    //ステータスを表示するテキスト
    // ステータスを表示するテキスト
    Text[] realStatusText = new Text[4];

    int[] statusPoint;      //ステータスを表示させておくテキスト
    int restStatusPoint;    //残りステータスポイント

    // 追加される値が格納されている変数
    int addAttack;
    int addDefense;
    int addMagicAttack;
    int addMagicDefense;
    int addHP;
    int addSP;

    // Use this for initialization
    void Start()
    {
        statusPartitionTexts = new Text[4];
        statusPoint = new int[4];       //メモリ領域を確保

        // ステータスポイントを表示させるテキストを格納する
        for (int i = 0; i < 4; i++)
        {
            // ステータスポイントを表示するテキストを1ずつ格納する
            statusPartitionTexts[i] = texts.transform.GetChild(i).gameObject.GetComponent<Text>();
        }
        // 攻撃力等のステータスを表示するテキストを格納する
        for (int i = 4; i < 8; i++)
        {
            realStatusText[i - 4] = texts.transform.GetChild(i).gameObject.GetComponent<Text>();
        }

        // それぞれ格納する
        statusPoint[0] = PlayerStatus.playerData.str;
        statusPoint[1] = PlayerStatus.playerData.vit;
        statusPoint[2] = PlayerStatus.playerData.intelligence;
        statusPoint[3] = PlayerStatus.playerData.mnd;

        // 残りステータスポイントを格納する
        restStatusPoint = PlayerStatus.playerData.statusPoint;
    }
    
    // Update is called once per frame
    void Update () {
        // 表示処理
        for (int i = 0; i < 4; i++)
        {
            // テキストを表示する
            statusPartitionTexts[i].text = statusPoint[i].ToString();
        }
        // 残りステータスポイントを表示する
        statusText.text = restStatusPoint.ToString();
    }

    /// <summary>
    /// プラスボタンを押した時の処理
    /// </summary>
    /// <param name="statusNumber">Kind of status number.</param>
    public void PushPlusButton(int statusNumber)
    {
        if (restStatusPoint > 0)    //ステータスポイントが余っていたら
        {
            // ステータスを増やす
            statusPoint[statusNumber]++;
            // 残りステータスポイントを減らす
            restStatusPoint--;              
            // 残りステータスポイントを表示するテキストを赤色にする
            statusText.color = Color.red;
            // 色を緑色にする
            statusPartitionTexts[statusNumber].color = Color.green;
            // ステータスを更新する
            UpdateAddStatus();
        }
    }

    /// <summary>
    /// 追加されるステータスの幅を計算する関数
    /// </summary>
    /// <param name="statusNumber">追加するステータスの番号</param>
    void UpdateAddStatus()
    {
        // 各値を初期化する
        addHP = addSP = addAttack = addDefense = addMagicAttack = addMagicDefense = 0;
        // ステータスの数だけ繰り返す
        for (int i = 0; i < statusPoint.Length; i++)
        {
            // 変わったステータスの値を計算するための変数
            int changeStatus = 0;
            
            switch (i)
            {
                case 0:
                    changeStatus = PlayerStatus.playerData.str;
                    break;
                case 1:
                    changeStatus = PlayerStatus.playerData.vit;
                    break;
                case 2:
                    changeStatus = PlayerStatus.playerData.intelligence;
                    break;
                case 3:
                    changeStatus = PlayerStatus.playerData.mnd;
                    break;
            }
            // ステータスの上がり幅が入ってるクラスを取得する
            var status_up = PlayerStatus.level_status.sheets[i].list[0];
            // 各ステータスの増加幅を再計算する
            addHP += (int)(statusPoint[i] * status_up.HP) - (int)(changeStatus * status_up.HP);
            addSP += (int)(statusPoint[i] * status_up.SP) - (int)(changeStatus * status_up.SP);
            addAttack += (int)(statusPoint[i] * status_up.Attack) - (int)(changeStatus * status_up.Attack);
            addDefense += (int)(statusPoint[i] * status_up.Defense) - (int)(changeStatus * status_up.Defense);
            addMagicAttack += (int)(statusPoint[i] * status_up.MagicAttack) - (int)(changeStatus * status_up.MagicAttack);
            addMagicDefense += (int)(statusPoint[i] * status_up.MagicDefense) - (int)(changeStatus * status_up.MagicDefense);
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
            // ステータスポイントを１でも振って有れば
            if (statusPoint[statusNumber] > playerStatusPoint)
            {
                // 振ったステータスを減らす
                statusPoint[statusNumber]--;    
                // 残りステータスポイントを増やす
                restStatusPoint++;

                // 振った後と振る前とでステータスポイントが一緒になるならば
                if (statusPoint[statusNumber] == playerStatusPoint)
                {
                    // テキストの色を黒に戻す
                    statusPartitionTexts[statusNumber].color = Color.white;
                }
                // 残りのポイントが降る前と同じならば
                if (restStatusPoint == PlayerStatus.playerData.statusPoint)
                {
                    // テキストの色を黒に戻す
                    statusText.color = Color.white;
                }
            }

            // 振った分のステータスの増減値を再計算する
            UpdateAddStatus();
        }
        
    }

    /// <summary>
    /// Push a decide button method.
    /// </summary>
    public void DecideButton()
    {
        // セーブデータに一時記憶した値を入れる
        PlayerStatus.playerData.str = statusPoint[0];
        PlayerStatus.playerData.vit = statusPoint[1];
        PlayerStatus.playerData.intelligence = statusPoint[2];
        PlayerStatus.playerData.mnd = statusPoint[3];
        PlayerStatus.playerData.statusPoint = restStatusPoint;

        // プレイヤーのステータスをstr,vit,int,mndを考慮したものに更新する
        PlayerStatus.UpdateStatus();

        // セーブする
        PlayerStatus.SavePlayerData();

        // 振った分のステータスを表示するテキスト分だけ繰り返す
        foreach (var status in statusPartitionTexts)
        {
            // それぞれのテキストの色を黒に変更する
            status.color = Color.white;
        }
        // テキストの色を黒に変更する
        statusText.color = Color.white;

        // ステータスの再計算を行う
        UpdateAddStatus();
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
            status.color = Color.white;
        }

        statusText.color = Color.white;

        // ステータスの再計算を行う
        UpdateAddStatus();
    }
}
