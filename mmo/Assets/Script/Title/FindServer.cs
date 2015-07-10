using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FindServer : Photon.MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings("ルシファー");  //接続設定
    }

    //ロビーに入ったら
    void OnJoinedLobby()
    {
        //Debug.Log("JoinLobby");
    }

    // Update is called once per frame
    void Update () {
    
    }
}