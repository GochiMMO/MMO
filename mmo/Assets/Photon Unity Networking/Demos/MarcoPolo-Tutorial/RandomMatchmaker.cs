using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class RandomMatchmaker : Photon.PunBehaviour
{
    //private PhotonView myPhotonView;
    //public bool offlineMode = false;
    [SerializeField]
    UnityStandardAssets.Cameras.FreeLookCam cam;
    [SerializeField, Tooltip("アーチャーのアニメーションコントローラー")]
    RuntimeAnimatorController archerAnimationController;
    [SerializeField, Tooltip("ウォーリアのアニメーションコントローラー")]
    RuntimeAnimatorController warriorAnimationController;
    [SerializeField, Tooltip("魔法職のアニメーションコントローラー")]
    RuntimeAnimatorController sorcererAnimationController;
    [SerializeField, Tooltip("モンクのアニメーションコントローラー")]
    RuntimeAnimatorController monkAnimationController;

    GameObject player;

    void Awake()
    {
        switch (PlayerStatus.playerData.characterNumber)
        {
            case 0:
                // 青年を出す処理を書く
                break;

            case 1:
                // ムキムキを出す
                player = PhotonNetwork.Instantiate("Player/mukimuki", new Vector3(PlayerStatus.playerData.x, PlayerStatus.playerData.y, PlayerStatus.playerData.z), Quaternion.identity, 0);
                break;
            case 2:
                // 少女を出す
                break;
            case 3:
                // BBAを出す
                break;
        }

        // 職業によって処理分け
        switch (PlayerStatus.playerData.job)
        {
            // アーチャー
            case 0:
                // アーチャーを入れる
                Archer archer = player.AddComponent<Archer>();
                // PhotonViewの通信を使うリストにアーチャーを登録する
                player.GetComponent<PhotonView>().ObservedComponents.Add(archer);
                // アニメーションを変更する
                player.GetComponent<Animator>().runtimeAnimatorController = archerAnimationController;
                break;
            case 1:
                // ウォーリアを入れる
                Warrior warrior = player.AddComponent<Warrior>();
                // PhotonViewの通信を使うリストにアーチャーを登録する
                player.GetComponent<PhotonView>().ObservedComponents.Add(warrior);

                warrior.Initialize();

                Debug.Log(warrior.GetPlayerData().name);

                // アニメーションを変更する
                player.GetComponent<Animator>().runtimeAnimatorController = warriorAnimationController;
                break;
            case 2:
                // ソーサラーを入れる
                Sorcerer sorcerer = player.AddComponent<Sorcerer>();
                // PhotonViewの通信を使うリストにアーチャーを登録する
                player.GetComponent<PhotonView>().ObservedComponents.Add(sorcerer);
                // アニメーションを変更する
                player.GetComponent<Animator>().runtimeAnimatorController = sorcererAnimationController;
                break;
            case 3:
                // アーチャーを入れる
                Monk monk = player.AddComponent<Monk>();
                // PhotonViewの通信を使うリストにアーチャーを登録する
                player.GetComponent<PhotonView>().ObservedComponents.Add(monk);
                // アニメーションを変更する
                player.GetComponent<Animator>().runtimeAnimatorController = monkAnimationController;
                break;
        }

    }

    // Use this for initialization
    void Start()
    {
        
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
