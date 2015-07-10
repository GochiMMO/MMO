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

    public void LoadServer1()
    {
        Debug.Log("join server1");
        roomName = server1.text;
        PhotonNetwork.JoinOrCreateRoom(roomName, createRoomOptions(), null);
    }

    public void LoadServer2()
    {
        Debug.Log("join server2");
        roomName = server2.text;
        PhotonNetwork.JoinOrCreateRoom(roomName, createRoomOptions(), null);
    }

    //ルームに入ったとき
    void OnJoinedRoom()
    {
        Debug.Log(roomName + " room join");
    }

    // Update is called once per frame
    void Update () {
    }
}