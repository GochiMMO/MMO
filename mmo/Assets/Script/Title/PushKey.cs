using System.Collections;
using UnityEngine;

public class PushKey : MonoBehaviour {
    [SerializeField]
    GameObject popupWindow;
    BoxCollider2D col;  //押した時の当り判定オブジェクト
    
    bool windowEnabledFlag = false;

    // Use this for initialization
    void Start () {
        col = gameObject.GetComponent<BoxCollider2D>();
    }

    //ポップアップウインドウの表示
    void enabledPopupWindow()
    {
        Instantiate(popupWindow, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // オフラインモードで接続する
    public void RunToOfflineMode()
    {
        PhotonNetwork.offlineMode = true;
        PhotonNetwork.ConnectUsingSettings("0.0");
        PhotonNetwork.JoinLobby();
    }

    IEnumerator loadLovel()
    {
        while (PhotonNetwork.connectionState != ConnectionState.Connected)
        {
            yield return null;
        }
        PhotonNetwork.LoadLevel("CharacterSelect");
    }

    // Update is called once per frame
    void Update () {
        //push any keyを押すか、どこかのキーが押されたら
        if ((Input.anyKeyDown || (col.OverlapPoint(Input.mousePosition) && Input.GetMouseButtonDown(0)) ) && !windowEnabledFlag)
        {
            windowEnabledFlag = true;
            enabledPopupWindow();
        }
    }
}