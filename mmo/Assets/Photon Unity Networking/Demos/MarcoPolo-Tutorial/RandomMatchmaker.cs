using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class RandomMatchmaker : Photon.PunBehaviour
{
    //private PhotonView myPhotonView;
    //public bool offlineMode = false;
    [SerializeField]
    UnityStandardAssets.Cameras.FreeLookCam cam;
    [SerializeField, Tooltip("�A�[�`���[�̃A�j���[�V�����R���g���[���[")]
    RuntimeAnimatorController archerAnimationController;
    [SerializeField, Tooltip("�E�H�[���A�̃A�j���[�V�����R���g���[���[")]
    RuntimeAnimatorController warriorAnimationController;
    [SerializeField, Tooltip("���@�E�̃A�j���[�V�����R���g���[���[")]
    RuntimeAnimatorController sorcererAnimationController;
    [SerializeField, Tooltip("�����N�̃A�j���[�V�����R���g���[���[")]
    RuntimeAnimatorController monkAnimationController;

    GameObject player;

    void Awake()
    {
        switch (PlayerStatus.playerData.characterNumber)
        {
            case 0:
                // �N���o������������
                break;

            case 1:
                // ���L���L���o��
                player = PhotonNetwork.Instantiate("Player/mukimuki", new Vector3(PlayerStatus.playerData.x, PlayerStatus.playerData.y, PlayerStatus.playerData.z), Quaternion.identity, 0);
                break;
            case 2:
                // �������o��
                break;
            case 3:
                // BBA���o��
                break;
        }

        // �E�Ƃɂ���ď�������
        switch (PlayerStatus.playerData.job)
        {
            // �A�[�`���[
            case 0:
                // �A�[�`���[������
                Archer archer = player.AddComponent<Archer>();
                // PhotonView�̒ʐM���g�����X�g�ɃA�[�`���[��o�^����
                player.GetComponent<PhotonView>().ObservedComponents.Add(archer);
                // �A�j���[�V������ύX����
                player.GetComponent<Animator>().runtimeAnimatorController = archerAnimationController;
                break;
            case 1:
                // �E�H�[���A������
                Warrior warrior = player.AddComponent<Warrior>();
                // PhotonView�̒ʐM���g�����X�g�ɃA�[�`���[��o�^����
                player.GetComponent<PhotonView>().ObservedComponents.Add(warrior);

                warrior.Initialize();

                Debug.Log(warrior.GetPlayerData().name);

                // �A�j���[�V������ύX����
                player.GetComponent<Animator>().runtimeAnimatorController = warriorAnimationController;
                break;
            case 2:
                // �\�[�T���[������
                Sorcerer sorcerer = player.AddComponent<Sorcerer>();
                // PhotonView�̒ʐM���g�����X�g�ɃA�[�`���[��o�^����
                player.GetComponent<PhotonView>().ObservedComponents.Add(sorcerer);
                // �A�j���[�V������ύX����
                player.GetComponent<Animator>().runtimeAnimatorController = sorcererAnimationController;
                break;
            case 3:
                // �A�[�`���[������
                Monk monk = player.AddComponent<Monk>();
                // PhotonView�̒ʐM���g�����X�g�ɃA�[�`���[��o�^����
                player.GetComponent<PhotonView>().ObservedComponents.Add(monk);
                // �A�j���[�V������ύX����
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
