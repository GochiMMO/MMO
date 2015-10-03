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
    [SerializeField, Tooltip("プレイヤーの名前の色")]
    Color nameColor;
    [SerializeField, Tooltip("全体チャット切り替えボタン")]
    GameObject allChatButton;
    [SerializeField, Tooltip("パーティーチャット切り替えボタン")]
    GameObject partyChatButton;
    [SerializeField, Tooltip("個別チャット切り替えボタン")]
    GameObject ptopChatButton;
    [SerializeField, Tooltip("送信ボタン")]
    UnityEngine.UI.Button sendButton;

    BoxCollider2D[] buttons = new BoxCollider2D[3];
    PartySystem partySystem;
    public Color activeColor;
    public Color nonActiveColor;
    int nowChatNumber = 0;
    
    // Use this for initialization
    void Start () {
        // チャット切り替えボタンのコライダーの参照を取得
        buttons[0] = allChatButton.GetComponent<BoxCollider2D>();
        buttons[1] = partyChatButton.GetComponent<BoxCollider2D>();
        buttons[2] = ptopChatButton.GetComponent<BoxCollider2D>();
        // パーティーシステムを取得
        partySystem = GameObject.Find("Scripts").GetComponent<PartySystem>();
    }
    
    // Update is called once per frame
    void Update () {
        // クリックされたら
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                // ボタンの上にマウスカーソルがあるかどうか
                if (buttons[i].OverlapPoint(Input.mousePosition))
                {
                    switch (i)
                    {
                            // 全体チャットに切り替える
                        case 0:
                            // 全体チャットをオンにする
                            allRoomChat.gameObject.SetActive(true);
                            // 他のチャットを非アクティブにする
                            partyChatText.gameObject.SetActive(false);
                            playerToPlayerChatText.gameObject.SetActive(false);
                            // ボタンに全体送信のメソッドを登録
                            sendButton.onClick.RemoveAllListeners();
                            sendButton.onClick.AddListener(SendChatInputForAll);
                            // 切り替えボタンの色を変更する
                            setButtonsColor(0);
                            nowChatNumber = i;  // 現在チャット番号を切り替える
                            break;

                            // パーティーチャットに切り替える
                        case 1:
                            // パーティーメンバーがいなければ
                            if (partySystem.GetPartyMember() == null)
                            {
                                if (string.IsNullOrEmpty(allRoomChat.text))
                                {
                                    allRoomChat.text += "あなたはパーティーに所属していません";
                                }
                                else
                                {
                                    allRoomChat.text += "\nあなたはパーティーに所属していません";
                                }
                                break;
                            }
                            // パーティーメンバーが存在する場合
                            // パーティーチャットをオンにする
                            partyChatText.gameObject.SetActive(true);
                            // 他のチャットを非アクティブにする
                            allRoomChat.gameObject.SetActive(false);
                            playerToPlayerChatText.gameObject.SetActive(false);
                            // ボタンにパーティー送信のメソッドを登録
                            sendButton.onClick.RemoveAllListeners();
                            sendButton.onClick.AddListener(SendChatInputForPartyMember);
                            // 切り替えボタンの色を変更する
                            setButtonsColor(1);
                            nowChatNumber = i;  // 現在チャット番号を切り替える
                            break;

                            // 個別チャットに切り替える
                        case 2:
                            // 個別チャットをオンにする
                            playerToPlayerChatText.gameObject.SetActive(true);
                            // 他のチャットを非アクティブにする
                            allRoomChat.gameObject.SetActive(false);
                            partyChatText.gameObject.SetActive(false);
                            // ボタンに全体送信のメソッドを登録
                            sendButton.onClick.RemoveAllListeners();
                            sendButton.onClick.AddListener(SendChatInputForPlayer);
                            // 切り替えボタンの色を変更する
                            setButtonsColor(2);
                            nowChatNumber = i;  // 現在チャット番号を切り替える
                            break;
                    }

                    
                    // ２つ以上のボタンが同時に押されることが無いため、処理が完了したらループから抜ける
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttonNumber"></param>
    void setButtonsColor(int buttonNumber)
    {
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
                    case 0:
                        // 全体にチャットを送信する
                        SendChatInputForAll();
                        break;
                    case 1:
                        SendChatInputForPartyMember();
                        break;
                    case 2:
                        SendChatInputForPlayer();
                        break;
                }
                
                return; // printing the now modified list would result in an error. to avoid this, we just skip this single frame
            }
            else
            {
                //GUI.FocusControl("ChatInput");
            }
        }
    }
    
    
    /// <summary>
    /// Send text of input field to all players.
    /// </summary>
    public void SendChatInputForAll()
    {
        // RPCでテキストを送信する
        this.photonView.RPC("Chat", PhotonTargets.All, input.text, 0);

        input.text = "";
        GUI.FocusControl("");
    }

    /// <summary>
    /// Send text of input field to my party members.
    /// </summary>
    public void SendChatInputForPartyMember()
    {
        GameObject[] member = partySystem.GetPartyMember();
        
        for (int i = 0; i < member.Length; i++)
        {
            this.photonView.RPC("Chat", member[i].GetPhotonView().owner, input.text, 1);
            input.text = "";
            GUI.FocusControl("");
        }
    }

    /// <summary>
    /// Send text of input field to my party members.
    /// </summary>
    public void SendChatInputForPlayer()
    {
        /*
        this.photonView.RPC("Chat", member[i].GetPhotonView().owner, input.text, 1);
        input.text = "";
        GUI.FocusControl("");
        */

    }

    /// <summary>
    /// When this method recieve a text and text number from other players or me, write it on my text field.
    /// </summary>
    /// <param name="newLine">A recieve text.</param>
    /// <param name="textNumber">what text field.</param>
    /// <param name="mi">send by.</param>
    [PunRPC]
    public void Chat(string newLine, int textNumber, PhotonMessageInfo mi)
    {
        // 送ってきた人の名前を設定した色で表示する
        string playerNameColor = "<color=#" + nameColor.ToHexStringRGBA() + ">";
        string senderName = playerNameColor + mi.sender.name + "</color> ";

        switch (textNumber)
        {
            case 0:
                // 全体チャットリストに登録する
                if (!string.IsNullOrEmpty(allRoomChat.text))
                {
                    this.allRoomChat.text += "\n" + senderName +  newLine;
                }
                else
                {
                    this.allRoomChat.text += senderName + newLine;
                }
                break;

            case 1:
                // パーティーチャットリストに登録する
                if (!string.IsNullOrEmpty(partyChatText.text))
                {
                    this.partyChatText.text += "\n" + senderName + newLine;
                }
                else
                {
                    this.partyChatText.text += senderName + newLine;
                }
                break;

            case 2:
                // 個別チャットリストに登録する
                if (!string.IsNullOrEmpty(playerToPlayerChatText.text))
                {
                    this.partyChatText.text += "\n" + senderName + newLine;
                }
                else
                {
                    this.partyChatText.text += senderName + newLine;
                }
                break;
        }
        
    }
}
