using UnityEngine;
using System.Collections;

public class MatchingServer : Photon.MonoBehaviour {
    [SerializeField]
    string roomName;

    private PhotonView myPhotonView;

    // Use this for initialization
    void Start () {
        PhotonNetwork.ConnectUsingSettings("0.1");  //接続設定
    }

    /*
    //部屋を立てた時
    public void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.room.name + " Room Join");
    }*/

    //ロビーに接続したかどうか
    public void OnJoinedLobby()
    {
        Debug.Log("JoinLobby");
        PhotonNetwork.CreateRoom(roomName); //roomNameという名前の部屋を立てる
        //PhotonNetwork.JoinRandomRoom();
    }

    // Update is called once per frame
    void Update () {
    
    }

    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}