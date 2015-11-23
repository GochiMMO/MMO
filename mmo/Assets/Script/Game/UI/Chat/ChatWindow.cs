using UnityEngine;
using UnityEngine.UI;

public class ChatWindow : Photon.MonoBehaviour {
    [SerializeField, Tooltip("インプットテキストボックス")]
    InputField input;
    [SerializeField, Tooltip("表示するテキスト欄")]
    Text allRoomChat;
    [SerializeField, Tooltip("パーティーチャットテキスト欄")]
    Text partyChatText;
    [SerializeField, Tooltip("個別チャットテキスト欄")]
    Text playerToPlayerChatText;
    [SerializeField, Tooltip("ログウインドウのテキスト欄")]
    Text logWindowText;
    [SerializeField, Tooltip("プレイヤーの名前の色")]
    Color nameColor;
    [SerializeField, Tooltip("全体チャット切り替えボタン")]
    GameObject allChatButton;
    [SerializeField, Tooltip("パーティーチャット切り替えボタン")]
    GameObject partyChatButton;
    [SerializeField, Tooltip("個別チャット切り替えボタン")]
    GameObject ptopChatButton;
    [SerializeField, Tooltip("ログウインドウの切り替えボタン")]
    GameObject logWindowButton;
    [SerializeField, Tooltip("送信ボタン")]
    UnityEngine.UI.Button sendButton;
    [SerializeField, Tooltip("名前入力テキスト欄")]
    InputField nameInput;

    BoxCollider2D[] buttons = new BoxCollider2D[4];
    PartySystem partySystem;
    public Color activeColor;
    public Color nonActiveColor;
    TypeOfChat nowChatNumber = TypeOfChat.ALL;
    const int MAX_CHAT_LINE_NUM = 50;
    // 最小化するためのスクリプトコンポーネント
    MiniChatWindow miniChatWindow;

    int[] chatLineNum = new int[4];

    // チャットの種類
    enum TypeOfChat
    {
        ALL = 0,
        PARTY = 1,
        PTOP = 2,
        LOG = 3
    }
    
    // Use this for initialization
    void Start () {
        // チャット切り替えボタンのコライダーの参照を取得
        buttons[0] = allChatButton.GetComponent<BoxCollider2D>();
        buttons[1] = partyChatButton.GetComponent<BoxCollider2D>();
        buttons[2] = ptopChatButton.GetComponent<BoxCollider2D>();
        buttons[3] = logWindowButton.GetComponent<BoxCollider2D>();
        // パーティーシステムを取得
        partySystem = GameObject.Find("Scripts").GetComponent<PartySystem>();
        // 最小化するためのスクリプトコンポーネントを取得する
        miniChatWindow = gameObject.GetComponent<MiniChatWindow>();
    }
    
    // Update is called once per frame
    void Update () {
        // クリックされたら
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < System.Enum.GetValues(typeof(TypeOfChat)).Length; i++)
            {
                // ボタンの上にマウスカーソルがあるかどうか
                if (buttons[i].OverlapPoint(Input.mousePosition))
                {
                    switch (i)
                    {
                            // 全体チャットに切り替える処理
                        case (int)TypeOfChat.ALL:
                            // 全体チャットをオンにする
                            allRoomChat.gameObject.SetActive(true);
                            // 名前入力テキストをオフにする
                            nameInput.gameObject.SetActive(false);
                            // 他のチャットを非アクティブにする
                            partyChatText.gameObject.SetActive(false);
                            playerToPlayerChatText.gameObject.SetActive(false);
                            logWindowText.gameObject.SetActive(false);
                            // ボタンに全体送信のメソッドを登録
                            sendButton.onClick.RemoveAllListeners();
                            sendButton.onClick.AddListener(SendChatInputForAll);
                            // 切り替えボタンの色を変更する
                            setButtonsColor((int)TypeOfChat.ALL);
                            nowChatNumber = TypeOfChat.ALL;  // 現在チャット番号を切り替える
                            break;

                            // パーティーチャットに切り替える処理
                        case (int)TypeOfChat.PARTY:
                            // パーティーメンバーがいなければ
                            if (partySystem.GetPartyMember() == null)
                            {
                                // 現在のチャットが全体チャットなら
                                if (nowChatNumber == 0)
                                {
                                    // 全体チャットにPTに加入してない旨を記述する
                                    allRoomChat.text += CreateChatText("<color=yellow>パーティーに所属していません。</color>", allRoomChat.text);
                                    break;
                                }
                                // 現在のチャットが個別チャットなら
                                else
                                {
                                    // 個別チャットにPTに加入してない旨を記述する
                                    playerToPlayerChatText.text += CreateChatText("<color=yellow>パーティーに所属していません。</color>", playerToPlayerChatText.text);
                                    break;
                                }
                            }
                            // パーティーメンバーが存在する場合
                            // パーティーチャットをオンにする
                            partyChatText.gameObject.SetActive(true);
                            // 名前入力テキストをオフにする
                            nameInput.gameObject.SetActive(false);
                            // 他のチャットを非アクティブにする
                            allRoomChat.gameObject.SetActive(false);
                            playerToPlayerChatText.gameObject.SetActive(false);
                            logWindowText.gameObject.SetActive(false);
                            // ボタンにパーティー送信のメソッドを登録
                            sendButton.onClick.RemoveAllListeners();
                            sendButton.onClick.AddListener(SendChatInputForPartyMember);
                            // 切り替えボタンの色を変更する
                            setButtonsColor((int)TypeOfChat.PARTY);
                            nowChatNumber = TypeOfChat.PARTY;  // 現在チャット番号を切り替える
                            break;

                            // 個別チャットに切り替える処理
                        case (int)TypeOfChat.PTOP:
                            // 個別チャットをオンにする
                            playerToPlayerChatText.gameObject.SetActive(true);
                            // 名前入力テキストをオンにする
                            nameInput.gameObject.SetActive(true);
                            // 他のチャットを非アクティブにする
                            allRoomChat.gameObject.SetActive(false);
                            partyChatText.gameObject.SetActive(false);
                            logWindowText.gameObject.SetActive(false);
                            // ボタンに全体送信のメソッドを登録
                            sendButton.onClick.RemoveAllListeners();
                            sendButton.onClick.AddListener(SendChatInputForPlayer);
                            // 切り替えボタンの色を変更する
                            setButtonsColor((int)TypeOfChat.PTOP);
                            nowChatNumber = TypeOfChat.PTOP;  // 現在チャット番号を切り替える
                            break;
                        case (int)TypeOfChat.LOG:
                            // ログウインドウをオンにする
                            logWindowText.gameObject.SetActive(true);
                            // 名前入力テキストをオフにする
                            nameInput.gameObject.SetActive(false);
                            // 他のチャットを非アクティブにする
                            partyChatText.gameObject.SetActive(false);
                            playerToPlayerChatText.gameObject.SetActive(false);
                            allRoomChat.gameObject.SetActive(false);
                            // ボタンに全体送信のメソッドを登録
                            sendButton.onClick.RemoveAllListeners();
                            //sendButton.onClick.AddListener(SendChatInputForAll);
                            // 切り替えボタンの色を変更する
                            setButtonsColor((int)TypeOfChat.LOG);
                            nowChatNumber = TypeOfChat.LOG;  // 現在チャット番号を切り替える
                            break;
                    }
                    // ２つ以上のボタンが同時に押されることが無いため、処理が完了したらループから抜ける
                    break;
                }
            }
        }

        // パーティーチャットでかつパーティーメンバーがいない場合（脱退、解散が起こった場合）
        if (nowChatNumber == TypeOfChat.PARTY && partySystem.GetPartyMember() == null)
        {
            // 全体チャットをオンにする
            allRoomChat.gameObject.SetActive(true);
            // 他のチャットを非アクティブにする
            partyChatText.gameObject.SetActive(false);
            playerToPlayerChatText.gameObject.SetActive(false);
            logWindowText.gameObject.SetActive(false);
            // ボタンに全体送信のメソッドを登録
            sendButton.onClick.RemoveAllListeners();
            sendButton.onClick.AddListener(SendChatInputForAll);
            // 切り替えボタンの色を変更する
            setButtonsColor((int)TypeOfChat.ALL);
            nowChatNumber = TypeOfChat.ALL;  // 現在チャット番号を切り替える
        }
    }

    /// <summary>
    /// Change player to player chat.
    /// </summary>
    /// <param name="name">Send to name.</param>
    public void SetPtoPChat(string name)
    {
        // 個別チャットをオンにする
        playerToPlayerChatText.gameObject.SetActive(true);
        // 名前入力テキストをオンにする
        nameInput.gameObject.SetActive(true);
        // 他のチャットを非アクティブにする
        allRoomChat.gameObject.SetActive(false);
        partyChatText.gameObject.SetActive(false);
        logWindowText.gameObject.SetActive(false);
        // ボタンに全体送信のメソッドを登録
        sendButton.onClick.RemoveAllListeners();
        sendButton.onClick.AddListener(SendChatInputForPlayer);
        // 切り替えボタンの色を変更する
        setButtonsColor((int)TypeOfChat.PTOP);
        nowChatNumber = TypeOfChat.PTOP;  // 現在チャット番号を切り替える
        // 名前を設定する
        nameInput.text = name;
    }


    /// <summary>
    /// Change active button's color.
    /// </summary>
    /// <param name="buttonNumber">Active button number.</param>
    void setButtonsColor(int buttonNumber)
    {
        // ボタンの数だけ繰り返す
        for (int i = 0; i < buttons.Length; i++)
        {
            // アクティブにするボタンでなかった場合
            if (buttonNumber != i)
            {
                // 背景色の変更
                buttons[i].gameObject.GetComponent<Image>().color = nonActiveColor;
                // テキストカラーの変更
                buttons[i].gameObject.transform.GetChild(0).GetComponent<Text>().color = nonActiveColor;
            }

            // アクティブにするボタン
            else
            {
                // 背景色の変更
                buttons[i].gameObject.GetComponent<Image>().color = activeColor;
                // テキストカラーの変更
                buttons[i].gameObject.transform.GetChild(0).GetComponent<Text>().color = activeColor;
            }
        }
    }

    // Update per frame and wakeup event.
    public void OnGUI()
    {
        // ネットワークに接続されていないときはできない
        if (PhotonNetwork.connectionStateDetailed != PeerState.Joined)
        {
            return;
        }

        // エンターキーが押された時の処理
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            // 文字列が空では無い場合
            if (!string.IsNullOrEmpty(input.text))
            {
                switch (nowChatNumber)
                {
                    case TypeOfChat.ALL:
                        // 全体にチャットを送信する
                        SendChatInputForAll();
                        break;
                    case TypeOfChat.PARTY:
                        // パーティーにチャットを送信する
                        SendChatInputForPartyMember();
                        break;
                    case TypeOfChat.PTOP:
                        // 個別にチャットを送信する
                        SendChatInputForPlayer();
                        break;
                }
                
                return; // printing the now modified list would result in an error. to avoid this, we just skip this single frame
            }
            else
            {
                // フォーカスを外す
                GUI.FocusControl("");
            }
        }
    }
    
    
    /// <summary>
    /// Send text of input field to all players.
    /// </summary>
    public void SendChatInputForAll()
    {
        // RPCでテキストを送信する
        this.photonView.RPC("AllChat", PhotonTargets.All, input.text);

        input.text = "";
        GUI.FocusControl("");
    }

    /// <summary>
    /// Send text of input field to my party members.
    /// </summary>
    public void SendChatInputForPartyMember()
    {
        GameObject[] member = partySystem.GetPartyMember();
        // パーティーメンバー分繰り返す
        for (int i = 0; i < member.Length; i++)
        {
            // チャットを送信する
            this.photonView.RPC("PartyChat", member[i].GetPhotonView().owner, input.text);
        }
        // テキスト入力欄を空にする
        input.text = "";
        GUI.FocusControl("");
    }

    /// <summary>
    /// Send text of input field to my party members.
    /// </summary>
    public void SendChatInputForPlayer()
    {
        // 名前入力テキストが空だった場合
        if (string.IsNullOrEmpty(nameInput.text))
        {
            // 名前を入力してくださいとチャット欄に出力する
            playerToPlayerChatText.text += CreateChatText(StaticMethods.CreateRichTextFromCaptionAndColorName("名前を入力してください。", "yellow"), playerToPlayerChatText.text);
            return;
        }

        // 名前から送り先の検索
        GameObject[] sendObject = StaticMethods.FindGameObjectsWithNameAndTag(nameInput.text, "Player");

        // その名前のプレイヤーが存在しなければ
        if (sendObject == null)
        {
            // 名前が見つからなかった旨を記述する
            playerToPlayerChatText.text += CreateChatText(StaticMethods.CreateRichTextFromCaptionAndColorName(nameInput.text + "さんが見つかりませんでした。", "yellow"), playerToPlayerChatText.text);
            return;
        }

        // 送る
        for (int i = 0; i < sendObject.Length; i++)
        {
            photonView.RPC("PtoPChat", sendObject[i].GetPhotonView().owner, input.text);
        }

        // 自分の個別チャット欄に送り先と送った内容を記述する
        playerToPlayerChatText.text += CreateChatText(StaticMethods.CreateRichTextFromCaptionAndColor("<size=15>To</size> " + nameInput.text, nameColor) + " " + input.text, playerToPlayerChatText.text);

        input.text = "";
        GUI.FocusControl("");
    }

    /// <summary>
    /// When this method recieve a text and text number from other players or me, write it on my text field.
    /// </summary>
    /// <param name="newLine">A recieve text.</param>
    /// <param name="photonMessageInfo">sended by.</param>
    [PunRPC]
    public void AllChat(string newLine, PhotonMessageInfo photonMessageInfo)
    {
        // 名前
        string senderName = StaticMethods.CreateRichTextFromCaptionAndColor(photonMessageInfo.sender.name, nameColor) + " ";
        // 全体チャットリストに登録する
        allRoomChat.text += CreateChatText(senderName + newLine, allRoomChat.text);
        // チャット行数を調べ、最大保存数を超えているかチェックし、行数を加算する
        if (chatLineNum[(int)TypeOfChat.ALL]++ > MAX_CHAT_LINE_NUM)
        {
            // 改行まで削除する
            allRoomChat.text.Remove(0, allRoomChat.text.IndexOf('\n'));
            // チャット行数を減らす
            chatLineNum[(int)TypeOfChat.ALL]--;
        }
        // チャットを受け取ったことを通知する
        miniChatWindow.RecieveChat();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="photonMessageInfo"></param>
    [PunRPC]
    public void PartyChat(string text, PhotonMessageInfo photonMessageInfo)
    {
        // 名前
        string senderName = StaticMethods.CreateRichTextFromCaptionAndColor(photonMessageInfo.sender.name, nameColor) + " ";
        // パーティーチャットリストに登録する
        partyChatText.text += CreateChatText(senderName + text, partyChatText.text);
        // 全体チャットリストに登録する
        allRoomChat.text += CreateChatText(senderName + StaticMethods.CreateRichTextFromCaptionAndColor(text, partyChatText.color), allRoomChat.text);
        // チャットの行数に加算する
        chatLineNum[(int)TypeOfChat.ALL]++;
        // チャットの行数に加算する
        chatLineNum[(int)TypeOfChat.PARTY]++;

        // 全体チャットの行数を調べ、最大保存数を超えているかチェックし、行数を加算する
        if (chatLineNum[(int)TypeOfChat.ALL]++ > MAX_CHAT_LINE_NUM)
        {
            // 改行まで削除する
            allRoomChat.text.Remove(0, allRoomChat.text.IndexOf('\n'));
            // チャット行数を減らす
            chatLineNum[(int)TypeOfChat.ALL]--;
        }

        // パーティーチャットの行数を調べ、最大保存数を超えているかチェックし、行数を加算する
        if (chatLineNum[(int)TypeOfChat.PARTY]++ > MAX_CHAT_LINE_NUM)
        {
            // 改行まで削除する
            allRoomChat.text.Remove(0, partyChatText.text.IndexOf('\n'));
            // チャット行数を減らす
            chatLineNum[(int)TypeOfChat.PARTY]--;
        }
        // チャットを受け取ったことを通知する
        miniChatWindow.RecieveChat();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="photonMessageInfo"></param>
    [PunRPC]
    public void PtoPChat(string text, PhotonMessageInfo photonMessageInfo)
    {
        // 名前
        string senderName = StaticMethods.CreateRichTextFromCaptionAndColor("<size=15>From</size> " + photonMessageInfo.sender.name, nameColor) + " ";
        // 個別チャットリストに登録する
        playerToPlayerChatText.text += CreateChatText(senderName + text, playerToPlayerChatText.text);
        // 全体チャットリストに登録する
        allRoomChat.text += CreateChatText(senderName + StaticMethods.CreateRichTextFromCaptionAndColor(text, playerToPlayerChatText.color), allRoomChat.text);
        // チャットの行数に加算する
        chatLineNum[(int)TypeOfChat.ALL]++;
        // チャットの行数に加算する
        chatLineNum[(int)TypeOfChat.PTOP]++;

        // 全体チャットの行数を調べ、最大保存数を超えているかチェックし、行数を加算する
        if (chatLineNum[(int)TypeOfChat.ALL]++ > MAX_CHAT_LINE_NUM)
        {
            // 改行まで削除する
            allRoomChat.text.Remove(0, allRoomChat.text.IndexOf('\n'));
            // チャット行数を減らす
            chatLineNum[(int)TypeOfChat.ALL]--;
        }

        // 個別チャットの行数を調べ、最大保存数を超えているかチェックし、行数を加算する
        if (chatLineNum[(int)TypeOfChat.PTOP]++ > MAX_CHAT_LINE_NUM)
        {
            // 改行まで削除する
            allRoomChat.text.Remove(0, playerToPlayerChatText.text.IndexOf('\n'));
            // チャット行数を減らす
            chatLineNum[(int)TypeOfChat.PTOP]--;
        }
        // チャットを受け取ったことを通知する
        miniChatWindow.RecieveChat();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="caption"></param>
    /// <param name="chatText"></param>
    /// <returns></returns>
    string CreateChatText(string caption, string chatText)
    {
        // 登録するテキストが空であるかどうか
        if (string.IsNullOrEmpty(chatText))
        {
            // 空ならそのまま返す
            return caption;
        }
        else
        {
            // 空でないなら改行付きで返す
            return "\n" + caption;
        }
    }
}