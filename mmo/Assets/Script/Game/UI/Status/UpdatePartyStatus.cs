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
    Image hpBarObject;

    public PlayerChar playerChar = null;

    int prevHP;

    // Use this for initialization
    void Start () {
        nameText.text = playerChar.GetPlayerData().name;
    }

    /// <summary>
    /// プレイヤーのデータを格納する
    /// </summary>
    /// <param name="playerChar">プレイヤーのデータ</param>
    public void SetPlayerChar(PlayerChar playerChar)
    {
        Debug.Log("SetPlayerChar(" + playerChar.GetPlayerData().name + ")");
        this.playerChar = playerChar;
        Start();
    }
    
    // Update is called once per frame
    void Update () {
        // プレイヤーが設定されている場合行う
        if (playerChar != null)
        {
            UpdateHPBarImageAndText();
            levelText.text = playerChar.GetPlayerData().Lv.ToString();
        }
    }

    /// <summary>
    /// ＨＰバーとテキストの更新を行う
    /// </summary>
    void UpdateHPBarImageAndText()
    {
        // 現在HPと設定されたHPが異なっていたら
        if (playerChar.GetPlayerData().HP != prevHP)
        {
            // サイズを計算する
            float size = (float)playerChar.GetPlayerData().HP / (float)playerChar.GetPlayerData().MaxHP;
            // 大きさを変更する
            hpBarObject.fillAmount = size;
            // HPのテキスト表示を更新する
            hpText.text = playerChar.GetPlayerData().HP.ToString() + " / " + playerChar.GetPlayerData().MaxHP.ToString();
            // HPを設定する
            prevHP = playerChar.GetPlayerData().HP;
        }
    }
}
