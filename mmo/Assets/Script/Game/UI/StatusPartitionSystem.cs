using UnityEngine;
using UnityEngine.UI;

public class StatusPartitionSystem : MonoBehaviour {
    [SerializeField, Tooltip("ステータスを表示するテキスト群")]
    GameObject texts;
    [SerializeField, Tooltip("スキルポイントを表示しておくテキスト")]
    Text statusText;

    Text[] statusPartitionTexts;

    int[] statusPoint;
    int restStatusPoint;

    // Use this for initialization
    void Start () {
        statusPartitionTexts = new Text[4];
        statusPoint = new int[4];

        for (int i = 0; i < 4; i++)
        {
            statusPartitionTexts[i] = texts.transform.GetChild(i).gameObject.GetComponent<Text>();
        }

        statusPoint[0] = PlayerStates.playerData.str;
        statusPoint[1] = PlayerStates.playerData.vit;
        statusPoint[2] = PlayerStates.playerData.intelligence;
        statusPoint[3] = PlayerStates.playerData.mnd;

        restStatusPoint = PlayerStates.playerData.statusPoint;
    }
    
    // Update is called once per frame
    void Update () {
        Debug.Log(statusPoint[0]);
        //表示処理
        for (int i = 0; i < 4; i++)
        {
            statusPartitionTexts[i].text = statusPoint[i].ToString();
        }
        statusText.text = restStatusPoint.ToString();
    }

    public void PushPlusButton(int statusNumber)
    {
        if (restStatusPoint > 0)
        {
            statusPoint[statusNumber]++;
            restStatusPoint--;
        }
    }

    public void PushMinusButton(int statusNumber)
    {
        if (restStatusPoint < PlayerStates.playerData.statusPoint)
        {
            statusPoint[statusNumber]--;
            restStatusPoint++;
        }
    }

    public void DecideButton()
    {
        PlayerStates.playerData.str = statusPoint[0];
        PlayerStates.playerData.vit = statusPoint[1];
        PlayerStates.playerData.intelligence = statusPoint[2];
        PlayerStates.playerData.mnd = statusPoint[3];
        PlayerStates.playerData.statusPoint = restStatusPoint;
    }

    public void ChancellButton()
    {
        statusPoint[0] = PlayerStates.playerData.str;
        statusPoint[1] = PlayerStates.playerData.vit;
        statusPoint[2] = PlayerStates.playerData.intelligence;
        statusPoint[3] = PlayerStates.playerData.mnd;

        restStatusPoint = PlayerStates.playerData.statusPoint;
    }
}
