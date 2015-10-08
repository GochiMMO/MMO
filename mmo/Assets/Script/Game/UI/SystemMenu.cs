using UnityEngine;
using System.Collections;

public class SystemMenu : MonoBehaviour {
    [SerializeField, Tooltip("ログアウトするかどうかのウインドウ")]
    GameObject logoutWindow;
    [SerializeField, Tooltip("ゲーム終了するかどうかのウインドウ")]
    GameObject quitWindow;

    /// <summary>
    /// Create logout window instance method.
    /// </summary>
    public void InstantiateLogoutWindow()
    {
        GameObject logoutWindowInstance = GameObject.Instantiate(logoutWindow);
        GameObject yesButton = logoutWindowInstance.transform.GetChild(2).gameObject;
        yesButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => GameObject.Find("Scripts").GetComponent<PartySystem>().RemoveMemberInParty(PhotonNetwork.player.ID));
        yesButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PushLogoutButton);
    }

    /// <summary>
    /// Create game quit window instance method.
    /// </summary>
    public void InstantiateQuitWindow()
    {
        GameObject quitWindowInstance = GameObject.Instantiate(quitWindow);
        GameObject yesButton = quitWindowInstance.transform.GetChild(2).gameObject;
        yesButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => GameObject.Find("Scripts").GetComponent<PartySystem>().RemoveMemberInParty(PhotonNetwork.player.ID));
        yesButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PushGameQuitButton);
    }

    /// <summary>
    /// Go to "CharacterSelect" level and leave this room.
    /// </summary>
    public void PushLogoutButton()
    {
        Debug.Log("Push logout button.");
        new WaitForSeconds(2f);
        PhotonNetwork.LeaveRoom();  //部屋から出る
        //PhotonNetwork.JoinOrCreateRoom("CharacterSelect", StaticMethods.createRoomOptions(), null);     //部屋を作る
        PhotonNetwork.LoadLevel("CharacterSelect");
    }

    /// <summary>
    /// Disconnect and quit game.
    /// </summary>
    public void PushGameQuitButton()
    {
        Debug.Log("Push quit button.");
        new WaitForSeconds(2f);
        PhotonNetwork.LeaveRoom();      // ルームから出る
        PhotonNetwork.LeaveLobby();     // ロビーから出る
        //PhotonNetwork.Disconnect();     //切断
        Application.Quit();
    }
}
