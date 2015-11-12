using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class RandomMatchmaker : Photon.PunBehaviour
{
    //private PhotonView myPhotonView;
    //public bool offlineMode = false;
    [SerializeField]
    UnityStandardAssets.Cameras.FreeLookCam cam;
    
    // Use this for initialization
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings("0.1");
        //PhotonNetwork.offlineMode = offlineMode;
        GameObject player = null;
        switch (PlayerStates.playerData.characterNumber)
        {
            case 0:
                // 青年を出す処理を書く
                break;

            case 1:
                // ムキムキを出す
                player = PhotonNetwork.Instantiate("Player/mukimuki", new Vector3(PlayerStates.playerData.x, PlayerStates.playerData.y, PlayerStates.playerData.z), Quaternion.identity, 0);
                break;
            case 2:
                // 少女を出す
                break;
            case 3:
                // BBAを出す
                break;
        }
        
        // GameObject monster = PhotonNetwork.Instantiate("monsterprefab", Vector3.zero, Quaternion.identity, 0);
        cam.SetTarget(player.transform);
        cam.enabled = true;
        //myPhotonView = monster.GetComponent<PhotonView>();
    }

    public override void OnJoinedLobby()
    {
        //Debug.Log("JoinRandom");
        //PhotonNetwork.JoinRandomRoom();
    }

    public void OnPhotonRandomJoinFailed()
    {
        //PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        //GameObject monster = PhotonNetwork.Instantiate("monsterprefab", Vector3.zero, Quaternion.identity, 0);
        //monster.GetComponent<myThirdPersonController>().isControllable = true;
        //myPhotonView = monster.GetComponent<PhotonView>();
    }
    /*
    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

        if (PhotonNetwork.connectionStateDetailed == PeerState.Joined)
        {
            bool shoutMarco = GameLogic.playerWhoIsIt == PhotonNetwork.player.ID;

            if (shoutMarco && GUILayout.Button("Marco!"))
            {
                myPhotonView.RPC("Marco", PhotonTargets.All);
            }
            if (!shoutMarco && GUILayout.Button("Polo!"))
            {
                myPhotonView.RPC("Polo", PhotonTargets.All);
            }
        }
    }
    */
}
