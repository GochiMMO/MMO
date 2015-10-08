using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PartySystem : Photon.MonoBehaviour {
    [SerializeField, Tooltip("パーティーから誘われた時に出すウィンドウ")]
    GameObject inviteWindow;
    [SerializeField, Tooltip("パーティーにすでに入っている時に表示されるウインドウ")]
    GameObject alreadyWindow;
    [SerializeField, Tooltip("あなたはリーダーではありません。")]
    GameObject notLeaderWindow;
    [SerializeField, Tooltip("パーティーメンバーがいっぱいの時に出すウインドウ")]
    GameObject fullOfMemberWindow;

    PhotonView myPhoton;
    public const int PARTY_MAX_MEMBER = 4; // １パーティー４人まで
    List<GameObject> partyMember;   // パーティーメンバー
    bool engagedFlag = false;       // (自分が)加入しているかのフラグ
    bool isLeader = false;          // リーダーであるかどうか

    GameObject target;      // 送信先
    GameObject owner;       // 送信元

    // Use this for initialization.
    void Start () {
        // パーティーメンバーを格納するリスト
        partyMember = new List<GameObject>();
        // 自分のPhotonViewを取得する
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
            // for Debug

            Debug.Log(partyMember.Count);
            foreach (var obj in partyMember)
            {
                Debug.Log(obj.GetPhotonView().owner.name);
            }
            
            // パーティーメンバーを配列にして返す
            return partyMember.ToArray();
        }
        // 加入していなければ
        else
        {
            return null;
        }
    }

    /// <summary>
    /// [RPC]Add party member.
    /// </summary>
    /// <param name="member">Additional member's ID.</param>
    [PunRPC]
    void AddMember(int memberID)
    {
        // IDからプレイヤーを検索する
        var member = StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(memberID, "Player");

        Debug.Log("add member, name of " + member.GetPhotonView().owner.name);

        // 加入済みメンバーでなければ
        if (!partyMember.Contains(member))
        {
            partyMember.Add(member);    // リストに加える
        }
        engagedFlag = true;     // 加入フラグを立てる
    }

    /// <summary>
    /// [RPC]Instantiate already engaged a party window.
    /// </summary>
    [PunRPC]
    void InstantiateAlreadyWindow()
    {
        // 既にパーティーに加入していると画面に表示する
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
        Debug.Log("SetOwner name is " + owner.GetPhotonView().owner.name);
        this.owner = owner;
    }

    /// <summary>
    /// Instantiate, if your party member is full.
    /// </summary>
    void InstantiateFullMemberWindow()
    {
        // メンバーがいっぱいであると表示する
        GameObject.Instantiate(fullOfMemberWindow);
    }

    /// <summary>
    /// Push it which is "OptionMenuWindow"'s invite button.
    /// </summary>
    /// <param name="member">Me</param>
    public void PushInviteButton()
    {
        Debug.Log("PushInviteButton send to " + target.GetComponent<PhotonView>().owner.name);

        // パーティーメンバーが規定人数に達していたとき
        if(partyMember.Count >= PARTY_MAX_MEMBER)
        {
            // メンバーがいっぱいであると表示する
            InstantiateFullMemberWindow();
        }

        // リーダーであるか、パーティーに加入していない時
        else if (isLeader || !engagedFlag)
        {
            // 相手を招待する
            myPhoton.RPC("InstantiateInviteWindow", target.GetComponent<PhotonView>().owner, target.GetPhotonView().ownerId, owner.GetPhotonView().ownerId);
            //PhotonNetwork.RPC(StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(PhotonNetwork.player.ID, "Player").GetPhotonView(), "InstantiateInviteWindow", target.GetPhotonView().owner, true, owner.GetPhotonView().ownerId);
        }

        // 加入しており、リーダーではないとき
        else
        {
            // リーダーではないウインドウを表示する
            GameObject.Instantiate(notLeaderWindow);
        }
    }

    /// <summary>
    /// [RPC]Instantiate "InviteWindow".
    /// </summary>
    /// <param name="member"></param>
    [PunRPC]
    public void InstantiateInviteWindow(int targetID, int ownerID)
    {
        // 加入フラグが立っていたら
        if (this.engagedFlag)
        {
            // 既に加入しているウィンドウを出す命令を相手に送り、その後の処理はしない
            myPhoton.RPC("InstantiateAlreadyWindow", StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(ownerID, "Player").GetPhotonView().owner);
            return;
        }

        Debug.Log("Recive instantiate of invite window");

        // 招待された時のウィンドウを表示させる
        GameObject obj = GameObject.Instantiate(inviteWindow);

        // IDから招待された相手を検索する(IDはRPCの引数で送信されてくる)
        owner = StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(ownerID, "Player");
        target = StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(targetID, "Player");

        // ウインドウに表示する名前の設定
        obj.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = owner.GetComponent<PhotonView>().owner.name;

        // ウインドウに表示されているボタンにメソッドの登録
        var button = obj.transform.GetChild(0).GetChild(3).GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(() => AddMemberInParty());
        button.onClick.AddListener(() => myPhoton.RPC("SetLeader", owner.GetPhotonView().owner));
        button.onClick.AddListener(() => obj.GetComponent<Methods>().DestroyObject());
    }

    /// <summary>
    /// Claim to send of own party member.
    /// </summary>
    /// <param name="targetMember">Address</param>
    [PunRPC]
    public void ClaimToSendYourPartyMember(int targetID)
    {
        // 誰に送るかを検索する
        GameObject target = StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(targetID, "Player");

        foreach (var member in partyMember) // メンバー全員分繰り返す
        {
            myPhoton.RPC("AddMember", target.GetComponent<PhotonView>().owner, member.GetPhotonView().ownerId); // メンバーに加入させる
        }
    }

    /// <summary>
    /// Set role of leader.(Leader is only one person in a party.)
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
    /// Send it, that is my party member for my party member.
    /// </summary>
    [PunRPC]
    void SendAllPartyMemberForAllMember()
    {
        // パーティーメンバー分だけ繰り返す
        for (int i = 0; i < partyMember.Count; i++)
        {
            // パーティーメンバーだけ繰り返す
            for (int j = 0; j < partyMember.Count; j++ )
            {
                // 自分ではない人に送信する
                if (!partyMember[i].GetPhotonView().isMine)
                {
                    // 他のパーティーメンバーに自分のメンバーリストを全送信する
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
        // IDからオブジェクトを検索する
        GameObject member = StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(memberID, "Player");
        // リストから削除する
        partyMember.Remove(member);
    }

    /// <summary>
    /// Remove me for my party.
    /// </summary>
    public void RemoveMemberInParty(int memberID)
    {
        // 加入していなかったら
        if (!engagedFlag)
        {
            return;
        }

        // 自分がリーダーならば次のメンバーにリーダーを引き継がせる
        if (isLeader)
        {
            myPhoton.RPC("SetLeader", partyMember[1].GetPhotonView().owner);
        }

        // パーティーメンバー全員にメンバー脱退を行わせる
        for (int i = 0; i < partyMember.Count;i++ )
        {
            // 自分の場合は行わない
            if (!partyMember[i].GetPhotonView().isMine)
            {
                // 脱退させる命令を出す
                myPhoton.RPC("RemoveMember", partyMember[i].GetPhotonView().owner, memberID);
            }
        }

        engagedFlag = false;    // 加入フラグを折る
        isLeader = false;       // リーダーフラグを折る

        partyMember.Clear();    // リストを空にする
    }

    /// <summary>
    /// My engaged party disband method.
    /// </summary>
    void DisbandParty()
    {
        // 加入フラグが立っており、メンバー数が２人未満（１人）のとき
        if (engagedFlag && partyMember.Count < 2)
        {
            engagedFlag = false;    // パーティー加入フラグを折る
            isLeader = false;       // リーダーフラグを折る
            partyMember.Clear();    // リストを空にする
        }
    }

    /// <summary>
    /// Update per after "Update" method.
    /// </summary>
    void LateUpdate()
    {
        // 解散するかどうか
        DisbandParty();
    }

    /*
    void OnDestroy()
    {
        RemoveMemberInParty(PhotonNetwork.player.ID);
    }
     */
    
}