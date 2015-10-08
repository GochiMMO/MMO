using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdatePartyStatus : MonoBehaviour {
    [SerializeField, Tooltip("レベルを表示するテキスト")]
    Text levelText;
    [SerializeField, Tooltip("名前を表示するテキスト")]
    Text nameText;
    [SerializeField, Tooltip("ＨＰを表示するテキスト")]
    Text hpText;
    [SerializeField, Tooltip("ＨＰバーのゲームオブジェクト")]
    GameObject hpBarObject;

    public PlayerChar playerChar;

    int prevHP;

    // Use this for initialization
    void Start () {
        nameText.text = playerChar.GetPlayerData().name;
    }

    public void SetPlayerChar(PlayerChar playerChar)
    {
        this.playerChar = playerChar;
        Start();
    }
    
    // Update is called once per frame
    void Update () {
        UpdateHPBarImageAndText();
        levelText.text = playerChar.GetPlayerData().Lv.ToString();
    }

    void UpdateHPBarImageAndText()
    {
        // 現在HPと設定されたHPが異なっていたら
        if (playerChar.GetPlayerData().HP != prevHP)
        {
            // サイズを計算する
            float size = (float)playerChar.GetPlayerData().HP / (float)playerChar.GetPlayerData().MaxHP;
            // 大きさを変更する
            hpBarObject.transform.localScale.Set(size, 1f, 1f);
            // HPのテキスト表示を更新する
            hpText.text = playerChar.GetPlayerData().HP.ToString() + " / " + playerChar.GetPlayerData().MaxHP.ToString();
            // HPを設定する
            prevHP = playerChar.GetPlayerData().HP;
        }
    }
}
