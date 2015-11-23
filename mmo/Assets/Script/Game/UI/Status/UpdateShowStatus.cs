using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateShowStatus : MonoBehaviour {
    [SerializeField, Tooltip("HPバーのイメージ画像")]
    Image hpBarImage;
    [SerializeField, Tooltip("SPバーのイメージ画像")]
    Image spBarImage;
    [SerializeField, Tooltip("Expバーのイメージ画像")]
    Image expBarImage;
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

    // 範囲
    int prevHP = -1;
    int prevSP = -1;
    int prevLv = -1;
    int prevMaxHP = -1;
    int prevMaxSP = -1;
    int prevExp = -1;

    // プレイヤーキャラクターのデータ
    PlayerData playerData = null;
    PlayerChar[] partyPlayerChar = new PlayerChar[3];

    // HP,SPバーのサイズが変わる時間
    float hpBarChangeTime = CHANGE_BAR_TIME;
    float spBarChangeTime = CHANGE_BAR_TIME;

    // 変更前HP,SPの値
    int beforeSPValue;
    int beforeHPValue;

    // HP,SPバーがどのくらいで移動完了となるか(秒)
    const float CHANGE_BAR_TIME = 1f;

    // Use this for initialization
    void Start () {
        partySystem = GameObject.Find("Scripts").GetComponent<PartySystem>();
    }
    
    // Update is called once per frame
    void LateUpdate () {
        // 自プレイヤーのデータの参照が未取得の場合
        if (playerData == null)
        {
            // プレイヤーのデータを格納する
            playerData = PlayerStatus.playerData;
            // プレイヤーの名前を設定する
            nameText.text = PlayerStatus.playerData.name;
            // ジョブの画像を変更する
            SetJobImage();
        }
        else
        {
            // HPバーとテキストの更新
            UpdateHP();
            // SPバーとテキストの更新
            UpdateSP();
            // EXPバーのテキストを更新する
            UpdateExp();
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
        if (playerData.HP != prevHP || playerData.MaxHP != prevMaxHP)
        {
            // もしバーのサイズが変わり切る前にHPが変わったら
            if (hpBarChangeTime < CHANGE_BAR_TIME)
            {
                // 前回の値を起点にする
                beforeHPValue = prevHP;
            }
            // HPを更新しておく
            prevHP = playerData.HP;
            // 最大HPを更新する
            prevMaxHP = playerData.MaxHP;
            // 変更する時間を更新する
            hpBarChangeTime = 0f;
        }
        // 変化する時間がまだ到達していなければ
        else if (hpBarChangeTime < CHANGE_BAR_TIME)
        {
            // 時間を足しこむ
            hpBarChangeTime += Time.deltaTime;
            // 時間が到達していたら
            if (hpBarChangeTime > CHANGE_BAR_TIME)
            {
                // 既定時間に設定する
                hpBarChangeTime = CHANGE_BAR_TIME;
            }
            // HPを計算する
            int hp = (int)StaticMethods.GetSinWave(beforeHPValue, prevHP, hpBarChangeTime / CHANGE_BAR_TIME);
            // バーのサイズを計算する
            float size = (float)hp / (float)playerData.MaxHP;
            // サイズを更新する
            hpBarImage.fillAmount = size;
            // HPのテキストを更新する
            hpText.text = hp.ToString() + " / " + playerData.MaxHP.ToString();
        }
        else
        {
            // 以前のHPの値を登録する
            beforeHPValue = prevHP;
        }
    }

    /// <summary>
    /// Update sp bar and text.
    /// </summary>
    void UpdateSP()
    {
        // 前のSPと現在SPが変わったら
        if (playerData.SP != prevSP || playerData.MaxSP != prevMaxSP)
        {
            // もしバーのサイズが変わり切る前にSPが変わったら
            if (spBarChangeTime < CHANGE_BAR_TIME)
            {
                // 前回の値を起点にする
                beforeSPValue = prevSP;
            }
            // SPを更新しておく
            prevSP = playerData.SP;
            // 最大SPを更新する
            prevMaxSP = playerData.MaxSP;
            // 変更する時間を更新する
            spBarChangeTime = 0f;
        }
        // 変化する時間がまだ到達していなければ
        else if (spBarChangeTime < CHANGE_BAR_TIME)
        {
            // 時間を足しこむ
            spBarChangeTime += Time.deltaTime;
            // 時間が到達していたら
            if (spBarChangeTime > CHANGE_BAR_TIME)
            {
                // 既定時間に設定する
                spBarChangeTime = CHANGE_BAR_TIME;
            }
            // SPを計算する
            int sp = (int)StaticMethods.GetSinWave(beforeSPValue, prevSP, spBarChangeTime / CHANGE_BAR_TIME);
            // バーのサイズを計算する
            float size = (float)sp / (float)playerData.MaxSP;
            // サイズを更新する
            spBarImage.fillAmount = size;
            // SPのテキストを更新する
            spText.text = sp.ToString() + " / " + playerData.MaxSP.ToString();
        }
        else
        {
            // 以前のSPの値を登録する
            beforeSPValue = prevSP;
        }
    }

    /// <summary>
    /// 経験値のバーを更新する
    /// </summary>
    void UpdateExp()
    {
        // 経験値が変動していたら
        if (playerData.nowExp != prevExp)
        {
            // 経験値がどのくらいたまっているか計算する
            float size = (float)playerData.nowExp / (float)PlayerStatus.nextLevel;
            // サイズを変更する
            expBarImage.fillAmount = size;
            // EXPを更新する
            prevExp = playerData.nowExp;
        }
    }

    /// <summary>
    /// Update level text.
    /// </summary>
    void UpdateLevel()
    {
        // レベルアップしたら
        if (playerData.Lv != prevLv)
        {
            // レベルを更新する
            levelText.text = playerData.Lv.ToString();
            // レベルを更新しておく
            prevLv = playerData.Lv;
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
    /// パーティーメンバーのステータスを更新する
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
