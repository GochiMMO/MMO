using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PartySystem : Photon.MonoBehaviour {
    [SerializeField, Tooltip("パーティーから誘われた時に出すウィンドウ")]
    GameObject inviteWindow;
    [SerializeField, Tooltip("パーティーにすでに入っている時に表示されるウインドウ")]
    GameObject alreadyWindow;
    [SerializeField, Tooltip("あなたはリーダーではありません。")]
    GameObject notLeaderWindow;

    PhotonView myPhoton;
    public const int PARTY_MAX_MEMBER = 4; // １パーティー４人まで
    List<GameObject> partyMember;   // パーティーメンバー
    bool engagedFlag = false;       // (自分が)加入しているかのフラグ
    bool isLeader = false;

    GameObject target;      // 送信先
    GameObject owner;       // 送信元

    // Use this for initialization.
    void Start () {
        partyMember = new List<GameObject>();
        myPhoton = GetComponent<PhotonView>();
    }

    /// <summary>
    /// Get party member of array.
    /// </summary>
    /// <returns>Party members.</returns>
    public GameObject[] GetPartyMember()
    {
        // パーティーに加入してあったら
        if (engagedFlag)
        {
            foreach (GameObject obj in partyMember)
            {
                Debug.Log(obj.GetPhotonView().owner.name);
            }
            return partyMember.ToArray();
        }
        // 加入していなければ
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Add party member.
    /// </summary>
    /// <param name="member">Additional member.</param>
    [PunRPC]
    void AddMember(int memberID)
    {
        var member = StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(memberID, "Player");
        Debug.Log("add member name of " + member.GetPhotonView().owner.name);
        if (!partyMember.Contains(member))  // 加入済みメンバーでなければ
        {
            partyMember.Add(member);    // リストに加える
        }
        engagedFlag = true;
    }

    /// <summary>
    /// 
    /// </summary>
    [PunRPC]
    void InstantiateAlreadyWindow()
    {
        GameObject.Instantiate(alreadyWindow);
    }

    /// <summary>
    /// Local variable "target" setter.
    /// </summary>
    /// <param name="target">game object</param>
    public void SetTarget(GameObject target)
    {
        Debug.Log("SetTarget name of " + target.GetPhotonView().owner.name);
        this.target = target;
    }

    /// <summary>
    /// Local variable "owner" setter.
    /// </summary>
    /// <param name="owner">game object</param>
    public void SetOwner(GameObject owner)
    {
        Debug.Log("SetOwner name of " + owner.GetPhotonView().owner.name);
        this.owner = owner;
    }

    /// <summary>
    /// Push it which is "OptionMenuWindow"'s invite button.
    /// </summary>
    /// <param name="member">Me</param>
    public void PushInviteButton()
    {
        Debug.Log("PushInviteButton send to " + target.GetComponent<PhotonView>().owner.name);
        if (isLeader || !engagedFlag)
        {
            myPhoton.RPC("InstantiateInviteWindow", target.GetComponent<PhotonView>().owner, target.GetPhotonView().ownerId, owner.GetPhotonView().ownerId);
        }
        else
        {
            GameObject.Instantiate(notLeaderWindow);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="member"></param>
    [PunRPC]
    public void InstantiateInviteWindow(int targetID, int ownerID)
    {
        if (this.engagedFlag)
        {
            myPhoton.RPC("InstantiateAlreadyWindow", StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(ownerID, "Player").GetPhotonView().owner);
            return;
        }
        Debug.Log("Recive instantiate of invite window");
        GameObject obj = GameObject.Instantiate(inviteWindow);

        owner = StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(ownerID, "Player");
        target = StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(targetID, "Player");

        obj.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = owner.GetComponent<PhotonView>().owner.name;
        obj.transform.GetChild(0).GetChild(3).GetComponent<UnityEngine.UI.Button>().onClick.AddListener( () => AddMemberInParty() );
        obj.transform.GetChild(0).GetChild(3).GetComponent<UnityEngine.UI.Button>().onClick.AddListener( () => obj.GetComponent<Methods>().DestroyObject());
        obj.transform.GetChild(0).GetChild(3).GetComponent<UnityEngine.UI.Button>().onClick.AddListener( () => myPhoton.RPC("SetLeader", owner.GetPhotonView().owner));
    }

    /// <summary>
    /// Claim to send of own party member.
    /// </summary>
    /// <param name="targetMember">Address</param>
    [PunRPC]
    public void ClaimToSendYourPartyMember(int targetID)
    {
        GameObject target = StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(targetID, "Player");
        foreach (var member in partyMember) // メンバー全員分繰り返す
        {
            myPhoton.RPC("AddMember", target.GetComponent<PhotonView>().owner, member.GetPhotonView().ownerId); // メンバーに加入させる
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [PunRPC]
    public void SetLeader()
    {
        this.isLeader = true;
    }

    /// <summary>
    /// Call "AddMember" method of selected player.
    /// </summary>
    /// <param name="member"></param>
    public void AddMemberInParty()
    {
        PhotonPlayer targetMember = owner.GetComponent<PhotonView>().owner;    // 送る先

        // 自分のパーティーリストに入れる
        //AddMember(owner.GetPhotonView().ownerId);       // お誘いが来たので相手が先にパーティーに加入する
        //AddMember(target.GetPhotonView().ownerId);      // 次に自分が加入する
        
        // 相手側のパーティーリストに入れる
        myPhoton.RPC("AddMember", targetMember, owner.GetPhotonView().ownerId);       // 相手が先に入る
        myPhoton.RPC("AddMember", targetMember, target.GetPhotonView().ownerId);      // 次に自分が入る

        Debug.Log("RPC coll owner name " + owner.GetPhotonView().owner.name);
        Debug.Log("RPC coll target name " + target.GetPhotonView().owner.name);

        // 相手のパーティーメンバーを送らせる
        //myPhoton.RPC("ClaimToSendYourPartyMember", targetMember, target.GetPhotonView().ownerId);
        myPhoton.RPC("SendAllPartyMemberForAllMember", targetMember);

        engagedFlag = true;     // 加入フラグを立てる
    }

    /// <summary>
    /// 
    /// </summary>
    [PunRPC]
    void SendAllPartyMemberForAllMember()
    {
        for (int i = 0; i < partyMember.Count; i++)
        {
            for (int j = 0; j < partyMember.Count; j++ )
            {
                if (!partyMember[i].GetPhotonView().isMine)
                {
                    myPhoton.RPC("AddMember", partyMember[i].GetPhotonView().owner, partyMember[j].GetPhotonView().ownerId);
                }
            }
        }
    }

    /// <summary>
    /// Remove party member.
    /// </summary>
    /// <param name="member">Removing member.</param>
    [PunRPC]
    void RemoveMember(int memberID)
    {
        GameObject member = StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(memberID, "Player");
        partyMember.Remove(member);
        //DisbandParty();     // 解散するかどうか
    }

    /// <summary>
    /// Remove me for my party.
    /// </summary>
    public void RemoveMemberInParty(int memberID)
    {
        // 自分がリーダーならば次のメンバーにリーダーを引き継がせる
        if (isLeader)
        {
            myPhoton.RPC("SetLeader", partyMember[1].GetPhotonView().owner);
        }

        // パーティーメンバー全員にメンバー脱退を行わせる
        for (int i = 0; i < partyMember.Count;i++ )
        {
            if (partyMember[i].GetPhotonView().ownerId != PhotonNetwork.player.ID)
            {
                myPhoton.RPC("RemoveMember", partyMember[i].GetComponent<PhotonView>().owner, memberID);
            }
        }

        engagedFlag = false;    // 加入フラグを折る
        isLeader = false;

        partyMember.Clear();
    }

    /// <summary>
    /// My engaged party disband method.
    /// </summary>
    void DisbandParty()
    {
        if (engagedFlag && partyMember.Count < 2)
        {
            engagedFlag = false;    // パーティー加入フラグを折る
            partyMember.Clear();    // リストを空にする
        }
    }

    /// <summary>
    /// Update per after "Update" method.
    /// </summary>
    void LateUpdate()
    {
        DisbandParty();
    }
}