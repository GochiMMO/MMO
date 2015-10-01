using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowPartyMember : MonoBehaviour {
    [SerializeField, Tooltip("一番目のパーティーメンバーを表示させる枠")]
    GameObject firstMember;
    [SerializeField, Tooltip("二番目のパーティーメンバーを表示させる枠")]
    GameObject secondMember;
    [SerializeField, Tooltip("三番目のパーティーメンバーを表示させる枠")]
    GameObject thirdMember;
    [SerializeField, Tooltip("四番目のパーティーメンバーを表示させる枠")]
    GameObject fourthMember;
    [SerializeField, Tooltip("パーティメンバーがいなかったときに表示させるもの")]
    GameObject emptyMemberShowing;
    [SerializeField, Tooltip("アーチャーのイメージ画像")]
    Sprite archerImage;
    [SerializeField, Tooltip("ウォーリアのイメージ画像")]
    Sprite warriorImage;
    [SerializeField, Tooltip("ソーサラーのイメージ画像")]
    Sprite sorcererImage;
    [SerializeField, Tooltip("モンクのイメージ画像")]
    Sprite monkImage;
    [SerializeField, Tooltip("脱退ボタン")]
    GameObject removeButton;

    PartySystem partySystem;

    GameObject[] partyMember;
    GameObject[] memberFrame;

    // Use this for initialization
    void Start () {
        partySystem = GameObject.Find("Scripts").GetComponent<PartySystem>();
        memberFrame = new GameObject[4];
        memberFrame[0] = firstMember;
        memberFrame[1] = secondMember;
        memberFrame[2] = thirdMember;
        memberFrame[3] = fourthMember;
        removeButton.SetActive(true);

        // 脱退ボタンのメソッド登録
        removeButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => partySystem.RemoveMemberInParty(PhotonNetwork.player.ID));
        removeButton.SetActive(false);
    }
    
    // Update is called once per frame
    void Update () {
        partyMember = partySystem.GetPartyMember();     // パーティーメンバーを取得する
        if (partyMember != null)
        {
            ShowMember();
        }
        else
        {
            ShowEmptyMember();
        }
    }

    /// <summary>
    /// Show empty showing object.
    /// </summary>
    void ShowEmptyMember()
    {
        for(int i = 0 ; i < memberFrame.Length; i++){
            memberFrame[i].SetActive(false);
        }
        emptyMemberShowing.SetActive(true);
    }

    /// <summary>
    /// Show party member list.
    /// </summary>
    void ShowMember()
    {
        emptyMemberShowing.SetActive(false);    // メンバーがいなかったときに表示させるものを非表示にする
        removeButton.SetActive(true);
        // パーティーメンバーの数だけ繰り返す
        for (int i = 0; i < partyMember.Length; i++)
        {
            Debug.Log("Party Member number = " + i.ToString());
            Debug.Log("Party Member name = " + partyMember[i].GetPhotonView().owner.name);
            memberFrame[i].SetActive(true);     // アクティブにする

            // レベルと名前と画像を表示させる
            memberFrame[i].transform.GetChild(1).GetComponent<Text>().text = partyMember[i].GetComponent<PlayerChar>().GetPlayerData().Lv.ToString() + " Lv";
            memberFrame[i].transform.GetChild(2).GetComponent<Text>().text = partyMember[i].GetComponent<PlayerChar>().GetPlayerData().name;
            switch (partyMember[i].GetComponent<PlayerChar>().GetPlayerData().job)
            {
                case 0:
                    memberFrame[i].transform.GetChild(3).GetComponent<Image>().sprite = archerImage;
                    break;
                case 1:
                    memberFrame[i].transform.GetChild(3).GetComponent<Image>().sprite = warriorImage;
                    break;
                case 2:
                    memberFrame[i].transform.GetChild(3).GetComponent<Image>().sprite = sorcererImage;
                    break;
                case 3:
                    memberFrame[i].transform.GetChild(3).GetComponent<Image>().sprite = monkImage;
                    break;
            }
        }
        // 人数分以外はフレームは非表示にする
        for (int i = partyMember.Length; i < PartySystem.PARTY_MAX_MEMBER; i++)
        {
            memberFrame[i].SetActive(false);
        }
    }
}
