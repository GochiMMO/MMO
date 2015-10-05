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
    [SerializeField, Tooltip("パーティーメンバーのHPバーイラスト")]
    GameObject[] partyMembersHPBarImage;
    [SerializeField, Tooltip("パーティーメンバーのHPテキスト")]
    Text[] partyMembersHPText;
    [SerializeField, Tooltip("パーティーメンバーのレベル表示テキスト")]
    Text[] partyMembersLevelText;
    [SerializeField, Tooltip("パーティーメンバーの名前表示テキスト")]
    Text[] partyMembersNameText;

    PartySystem partySystem;
    int prevHP;
    int prevSP;
    int prevLv;

    int[] partyPlayerPrevHP = new int[3];

    PlayerChar playerChar;  // プレイヤーキャラクターのデータ
    
    PlayerChar[] partyPlayerChar = new PlayerChar[3];    // パーティープレイヤーのデータ

    GameObject[] partyMembers;
    // Use this for initialization
    void Start () {
        partySystem = GameObject.Find("Scripts").GetComponent<PartySystem>();
    }
    
    // Update is called once per frame
    void LateUpdate () {
        // 自プレイヤーのデータを取得する
        if (!playerChar)
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetPhotonView().owner.ID == PhotonNetwork.player.ID)
                {
                    // プレイヤーのデータを取得する
                    playerChar = player.GetComponent<PlayerChar>();
                    // プレイヤーの名前を設定する
                    nameText.text = playerChar.GetPlayerData().name;
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
            // PT系の処理を行う
            UpdatePartyMemberList();

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
            hpBarImage.transform.localScale.Set(size, 1f, 1f);
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
            spBarImage.transform.localScale.Set(size, 1f, 1f);
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
        switch (playerChar.GetPlayerData().job)
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
    /// Update party member list on changed.
    /// </summary>
    void UpdatePartyMemberList()
    {
        // パーティーメンバーを取得
        GameObject[] partyMember = partySystem.GetPartyMember();
        // パーティーメンバーが存在する場合
        if (partyMember != null)
        {
            // パーティーメンバーだけ繰り返す
            for (int i = 0; i < partyMember.Length; i++)
            {
                // PTメンバーには自分も含まれているため自分ならば処理を行わないようにする
                if (partyMember[i].GetPhotonView().owner.ID != PhotonNetwork.player.ID)
                {
                    // 登録されているパーティーメンバーと参照が異なっていたら
                    if (partyMembers[i] != partyMember[i])
                    {
                        // プレイヤーキャラのデータを取得
                        partyPlayerChar[i] = partyMember[i].GetComponent<PlayerChar>();
                        // パーティーメンバーを登録
                        partyMembers[i] = partyMember[i];
                    }
                }
            }
            // パーティーメンバーが埋まってないときは空いたところにnullを入れる
            for (int i = partyMember.Length; i < partyPlayerChar.Length + 1; i++)
            {
                partyPlayerChar[i] = null;
            }
        }
    }

    /// <summary>
    /// Upteda party member's hp bar and hp text on changed.
    /// </summary>
    void PartyHPUpdate()
    {
        // パーティープレイヤーだけ繰り返す
        for (int i = 0; i < partyPlayerPrevHP.Length; i++)
        {
            if (partyPlayerChar[i] && partyPlayerPrevHP[i] != partyPlayerChar[i].GetPlayerData().HP)
            {
                // HPバーのサイズを計算する
                float size = partyPlayerChar[i].GetPlayerData().HP / partyPlayerChar[i].GetPlayerData().MaxHP;
                // HPバーのサイズを変更する
                partyMembersHPBarImage[i].transform.localScale.Set(size, 1f, 1f);
                // HPのテキストを変更する
                partyMembersHPText[i].text = partyPlayerChar[i].GetPlayerData().HP.ToString() + " / " + partyPlayerChar[i].GetPlayerData().MaxHP.ToString();
                // HPの値を更新する
                partyPlayerPrevHP[i] = partyPlayerChar[i].GetPlayerData().HP;
            }
        }
    }

    void UpdatePartyMemberNameAndLevel()
    {

    }
}
