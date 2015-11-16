using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateShowStatus : MonoBehaviour {
    [SerializeField, Tooltip("HPバーのイメージ画像")]
    GameObject hpBarImage;
    [SerializeField, Tooltip("SPバーのイメージ画像")]
    GameObject spBarImage;
    [SerializeField, Tooltip("Lvを出力するテキスト")]
    Text levelText;
    [SerializeField, Tooltip("名前を出力するテキスト")]
    Text nameText;
    [SerializeField, Tooltip("HPを表示するテキスト")]
    Text hpText;
    [SerializeField, Tooltip("SPを表示するテキスト")]
    Text spText;
    [SerializeField, Tooltip("職業のイメージ画像を表示するイメージ")]
    Image jobImage;
    [Tooltip("アーチャーのイメージ画像")]
    public Sprite archerImage;
    [Tooltip("ウォーリアのイメージ画像")]
    public Sprite warriorImage;
    [Tooltip("ソーサラーのイメージ画像")]
    public Sprite sorcererImage;
    [Tooltip("モンクのイメージ画像")]
    public Sprite monkImage;
    [SerializeField, Tooltip("パーティーのステータスを表示するウインドウ")]
    UpdatePartyStatus[] partyStatusWindow;

    PartySystem partySystem;

    int prevHP = -1;
    int prevSP = -1;
    int prevLv = -1;

    // プレイヤーキャラクターのデータ
    PlayerChar playerChar = null;
    PlayerChar[] partyPlayerChar = new PlayerChar[3];
    // Use this for initialization
    void Start () {
        partySystem = GameObject.Find("Scripts").GetComponent<PartySystem>();
    }
    
    // Update is called once per frame
    void LateUpdate () {
        // 自プレイヤーのデータの参照が未取得の場合
        if (!playerChar)
        {
            // Playerとタグが付いたオブジェクトを取得する
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                // PhotonViewを取得する
                PhotonView photonView = player.GetPhotonView();
                // PhotonViewが存在し、自分で出したキャラならば
                if (photonView && photonView.isMine)
                {
                    // プレイヤーのデータを取得する
                    playerChar = player.GetComponent<PlayerChar>();
                    // プレイヤーの名前を設定する
                    nameText.text = PlayerStatus.playerData.name;
                    // ジョブの画像を変更する
                    SetJobImage();
                    break;
                }
            }
        }
        else
        {
            // HPバーとテキストの更新
            UpdateHP();
            // SPバーとテキストの更新
            UpdateSP();
            // レベルを更新
            UpdateLevel();
            // パーティーのステータスを更新する
            UpdatePartyStatus();
        }
    }

    /// <summary>
    /// Update hp bar and text.
    /// </summary>
    void UpdateHP()
    {
        // 前のHPと現在HPが変わったら
        if (playerChar.GetPlayerData().HP != prevHP)
        {
            // サイズを計算する
            float size = playerChar.GetPlayerData().HP / playerChar.GetPlayerData().MaxHP;
            // サイズを変更する
            hpBarImage.transform.localScale = new Vector3(size, 1f, 1f);
            // テキストを変更する
            hpText.text = playerChar.GetPlayerData().HP.ToString() + " / " + playerChar.GetPlayerData().MaxHP.ToString();
            // HPを更新しておく
            prevHP = playerChar.GetPlayerData().HP;
        }
    }

    /// <summary>
    /// Update sp bar and text.
    /// </summary>
    void UpdateSP()
    {
        // 前のSPと現在SPが変わったら
        if (playerChar.GetPlayerData().SP != prevSP)
        {
            // サイズを計算する
            float size = playerChar.GetPlayerData().SP / playerChar.GetPlayerData().MaxSP;
            // サイズを変更する
            spBarImage.transform.localScale = new Vector3(size, 1f, 1f);
            // テキストを変更する
            spText.text = playerChar.GetPlayerData().SP.ToString() + " / " + playerChar.GetPlayerData().MaxSP.ToString();
            // SPを更新しておく
            prevSP = playerChar.GetPlayerData().SP;
        }
    }

    /// <summary>
    /// Update level text.
    /// </summary>
    void UpdateLevel()
    {
        // レベルアップしたら
        if (playerChar.GetPlayerData().Lv != prevLv)
        {
            // レベルを更新する
            levelText.text = playerChar.GetPlayerData().Lv.ToString();
            // レベルを更新しておく
            prevLv = playerChar.GetPlayerData().Lv;
        }
    }

    /// <summary>
    /// Set job image on status bar.
    /// </summary>
    void SetJobImage()
    {
        // 職業によって変更する
        switch (PlayerStatus.playerData.job)
        {
            case 0:
                // アーチャーの画像
                jobImage.sprite = archerImage;
                break;
            case 1:
                // ウォーリアの画像
                jobImage.sprite = warriorImage;
                break;
            case 2:
                // ソーサラーの画像
                jobImage.sprite = sorcererImage;
                break;
            case 3:
                // モンクの画像
                jobImage.sprite = monkImage;
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void UpdatePartyStatus()
    {
        // パーティーメンバーのリストを取得する
        GameObject[] partyMember = partySystem.GetPartyMember();
        // 自分が発見されたら１を設定する
        int serachMeInParty = 0;

        // パーティーメンバーがいる場合処理を行う
        if (partyMember != null)
        {
            // パーティーメンバーの数だけ繰り返す
            for (int i = 0; i < partyMember.Length; i++)
            {
                // 自分でなければ処理を行う
                if (partyMember[i].GetPhotonView().ownerId != PhotonNetwork.player.ID)
                {
                    // ノンアクティブだったら
                    if (!partyStatusWindow[i - serachMeInParty].gameObject.activeInHierarchy)
                    {
                        partyStatusWindow[i - serachMeInParty].gameObject.SetActive(true);
                    }
                    // プレイヤーキャラを設定する
                    partyPlayerChar[i - serachMeInParty] = partyMember[i].GetComponent<PlayerChar>();
                    // パーティーウィンドウのシステムにプレイヤーキャラを設定する
                    if (partyStatusWindow[i - serachMeInParty].playerChar != partyPlayerChar[i - serachMeInParty])
                    {
                        partyStatusWindow[i - serachMeInParty].SetPlayerChar(partyPlayerChar[i - serachMeInParty]);
                    }
                }
                // 自分が発見されたら配列の添え字をずらすためにフラグを立てる
                else
                {
                    serachMeInParty = 1;
                }
            }
            // パーティーメンバーがいない分だけ繰り返す
            for (int i = partyMember.Length-1; i < partyStatusWindow.Length; i++)
            {
                // アクティブなら
                if (partyStatusWindow[i].gameObject.activeInHierarchy)
                {
                    // 非アクティブにする
                    partyStatusWindow[i].gameObject.SetActive(false);
                }
            }
        }
        // パーティーメンバーがいない場合
        else
        {
            // パーティーステータスウインドウの数だけ繰り返す
            for (int i = 0; i < partyStatusWindow.Length; i++)
            {
                // パーティーステータスウインドウがアクティブなら
                if (partyStatusWindow[i].gameObject.activeInHierarchy)
                {
                    // ノンアクティブにする
                    partyStatusWindow[i].gameObject.SetActive(false);
                }
                // 配列の要素をクリアしておく
                partyPlayerChar[i] = null;
            }
        }
    }
}
