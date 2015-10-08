using UnityEngine;
using System.Collections;

public class YNWindow : MonoBehaviour {

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

    public void Yes()
    {
        PhotonNetwork.JoinOrCreateRoom("test", createRoomOptions(), null);  //ルームを作成
        
    }

    public void No()
    {
        DestroyObject(this.gameObject);
    }

    void OnJoinedRoom()
    {
        Debug.Log("Join room");
        PhotonNetwork.LoadLevel(2);
    }

    // Update is called once per frame
    void Update () {
    
    }
}