using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateRoomWithButton : Photon.MonoBehaviour {
    [SerializeField]
    Text server1;
    [SerializeField]
    Text server2;

    
    int selectServer = 0;
    string roomName;

    // Use this for initialization
    void Start () {
        // ランダムに入室失敗した場合、ルームを作成
        // ルームオプションの作成
        RoomOptions roomOptions = new RoomOptions ();
        roomOptions.isVisible = true;
        roomOptions.isOpen = true;
        roomOptions.maxPlayers = 4;
        roomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable (){ {"CustomProperties", "カスタムプロパティ"} };
        roomOptions.customRoomPropertiesForLobby = new string[] {"CustomProperties"};
    }

    RoomOptions createRoomOptions(bool isVisibled = true, bool isOpen = true, byte maxPlayer = 10)
    {
        // ランダムに入室失敗した場合、ルームを作成
        // ルームオプションの作成
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
        //PhotonNetwork.JoinRoom(server1.text);
        selectServer = 1;
    }

    public void LoadServer2()
    {
        Debug.Log("join server2");
        roomName = server2.text;
        PhotonNetwork.JoinOrCreateRoom(roomName, createRoomOptions(), null);
        //PhotonNetwork.JoinRoom(server2.text);
        selectServer = 2;
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Server" + selectServer.ToString() + " is failed");
        switch (selectServer)
        {
            case 1:
                roomName = server1.text;
                break;
            case 2:
                roomName = server2.text;
                break;
            default:
                roomName = null;
                break;
        }
        PhotonNetwork.CreateRoom(roomName); //ルームを作成する
        Debug.Log("Create " + roomName + " Server");
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