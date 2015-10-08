using UnityEngine;
using System.Collections;

public class YNWindow : MonoBehaviour {

    // Use this for initialization
    void Start () {
    
    }

    public void Yes()
    {
        PhotonNetwork.JoinOrCreateRoom("test", StaticMethods.createRoomOptions(), null);  //ルームを作成
        //PhotonNetwork.JoinOrCreateRoom("GameRoom", StaticMethods.createRoomOptions(), null);
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
        //if (PhotonNetwork.connectionStateDetailed == PeerState.Joined)
        //{
        //    PhotonNetwork.LoadLevel("TestGame");
        //}
    }
}