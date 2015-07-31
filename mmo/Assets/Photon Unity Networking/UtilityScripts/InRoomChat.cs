using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class InRoomChat : Photon.MonoBehaviour 
{
    public Rect GuiRect = new Rect(0,0, 250,300);
    public bool IsVisible = true;
    public bool AlignBottom = false;
    public List<string> messages = new List<string>();
    private string inputLine = "";
    private Vector2 scrollPos = Vector2.zero;

    public static readonly string ChatRPC = "Chat";

    public void Start()
    {
        if (this.AlignBottom)
        {
            this.GuiRect.y = Screen.height - this.GuiRect.height;
        }
    }

    public void OnGUI()
    {
        /*
        if (!this.IsVisible || PhotonNetwork.connectionStateDetailed != PeerState.Joined)
        {
            return;
        }
        */
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(this.inputLine))
            {
                this.photonView.RPC("Chat", PhotonTargets.All, this.inputLine);
                this.inputLine = "";
                GUI.FocusControl("");
                return; // printing the now modified list would result in an error. to avoid this, we just skip this single frame
            }
            else
            {
                GUI.FocusControl("ChatInput");  //フォーカスをチャット入力フォームに移動
            }
        }

        GUI.SetNextControlName("");

        GUILayout.BeginArea(this.GuiRect);  //エリアの開始

        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);  //スクロールを作成する

        GUILayout.FlexibleSpace();  //適当に幅を空ける

        //for (int i = messages.Count - 1; i >= 0; i--) //上が最新チャットになる処理
        for (int i = 0; i < messages.Count; i++ )   //下が最新チャットになる処理
        {
            GUILayout.Label(messages[i]);
        }
        
        GUILayout.EndScrollView();  //スクロールバーの設定終了

        GUILayout.BeginHorizontal();

        GUI.SetNextControlName("ChatInput");

        inputLine = GUILayout.TextField(inputLine);     //ユーザーが編集できるテキストボックスを生成

        if (GUILayout.Button("Send", GUILayout.ExpandWidth(false)))     //Sendボタンを作成し、監視する
        {
            this.photonView.RPC("Chat", PhotonTargets.All, this.inputLine);     //チャットを送信する(ルーム全員に)
            this.inputLine = "";        //文字列を削除する
            GUI.FocusControl("");       //フォーカス位置を変更する
            //scrollPos.y = Mathf.Infinity;   //一番下に合わせる処理
        }
        GUILayout.EndHorizontal();  //水平処理の終了
        GUILayout.EndArea();        //Guiを表示するエリアの処理の終了
    }

    [PunRPC]
    public void Chat(string newLine, PhotonMessageInfo mi)
    {
        string senderName = "anonymous";

        if (mi != null && mi.sender != null)
        {
            if (!string.IsNullOrEmpty(mi.sender.name))
            {
                senderName = mi.sender.name;
            }
            else
            {
                senderName = "player " + mi.sender.ID;
            }
        }

        //リストに追加する
        this.messages.Add(senderName +": " + newLine);
        scrollPos.y = Mathf.Infinity;   //一番下に合わせる処理
    }

    public void AddLine(string newLine)
    {
        this.messages.Add(newLine);
    }
}
