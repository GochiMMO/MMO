using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateRoomWithButton : Photon.MonoBehaviour {
    [SerializeField]
    Text server1;
    [SerializeField]
    Text server2;

    string roomName;

    // Use this for initialization
    void Start () {
    }

    RoomOptions createRoomOptions(bool isVisibled = true, bool isOpen = true, byte maxPlayer = 10)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.isVisible = isVisibled;
        roomOptions.isOpen = isOpen;
        roomOptions.maxPlayers = maxPlayer;
        roomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "CustomProperties", "カスタムプロパティ" } };
        roomOptions.customRoomPropertiesForLobby = new string[] { "CustomProperties" };
        return roomOptions;
    }

    //サーバー１をロードする
    public void LoadServer1()
    {
        roomName = server1.text;
        Debug.Log(roomName + "join server1");
        //PhotonNetwork.JoinOrCreateRoom(roomName, createRoomOptions(), null);
        PhotonNetwork.ConnectUsingSettings(roomName);   //名前を使って接続する
    }

    //サーバー２をロードする
    public void LoadServer2()
    {
        roomName = server2.text;
        Debug.Log(roomName + "join server2");
        //PhotonNetwork.JoinOrCreateRoom(roomName, createRoomOptions(), null);
        PhotonNetwork.ConnectUsingSettings(roomName);   //名前を使って接続する
    }

    //ルームに入ったとき
    void OnJoinedLobby()
    {
        Debug.Log(roomName + " Lobby join");
        PhotonNetwork.LoadLevel("CharacterSelect");
    }

    // Update is called once per frame
    void Update () {
    }
}