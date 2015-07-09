using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FindServer : Photon.MonoBehaviour {
    [SerializeField]
    Text tex;

    RoomInfo[] rooms;
    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");  //接続設定
    }

    //ロビーに入ったら
    void OnJoinedLobby()
    {
        Debug.Log("JoinLobby");
    }

    

    // Update is called once per frame
    void Update () {
    
    }
}