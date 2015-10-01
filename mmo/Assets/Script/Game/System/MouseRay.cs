using UnityEngine;
using System.Collections;

public class MouseRay : MonoBehaviour {
    [SerializeField, Tooltip("名前を表示させるネームプレート")]
    GameObject namePlate;
    [SerializeField, Tooltip("右クリックしたときのオプションウィンドウ")]
    GameObject optionWindow;


    GameObject namePlateInstance = null;
    GameObject optionWindowInstance = null;
    PartySystem partySystem;
    GameObject playerObject;
    GameObject targetPlayer;
    bool leftClickFlag;     // 左クリックのフラグ
    bool rightClickFlag;    // 右クリックのフラグ

    // Use this for initialization
    void Start () {
        partySystem = this.gameObject.GetComponent<PartySystem>();
    }
    
    // Update is called once per frame
    void Update () {
        // クリックしたかのフラグを入れる
        leftClickFlag = Input.GetMouseButtonDown(0);
        rightClickFlag = Input.GetMouseButtonDown(1);

        // クリックされたとき
        if (leftClickFlag || rightClickFlag)
        {
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool rayHitFlag = Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity);  //レイを飛ばし、オブジェクトが存在するか調べる

            // マウスクリックした場所からレイを飛ばし、オブジェクトがあるかどうか
            if (rayHitFlag)
            {
                deleteNamePlate();  // ネームプレートを削除する

                // ヒットしたオブジェクトが敵ならば
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    // ネームプレートをインスタンス化する
                    namePlateInstance = GameObject.Instantiate(namePlate);
                    // ネームプレートに(Clone)を外した文字列をいれる
                    namePlateInstance.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = hit.collider.gameObject.name.Replace("(Clone)", "");
                }

                // プレイヤーなら
                else if (hit.collider.gameObject.tag == "Player")
                {
                    // プレイヤーの検索
                    if(!playerObject){
                        foreach(var player in GameObject.FindGameObjectsWithTag("Player")){
                            if(player.GetComponent<PhotonView>().owner.ID == PhotonNetwork.player.ID){
                                playerObject = player;
                            }
                        }
                    }

                    // 自分以外のプレイヤーの数だけ繰り返す
                    foreach (var player in PhotonNetwork.otherPlayers)
                    {
                        //プレイヤーのIDとレイがヒットしたオブジェクトのIDが同じなら
                        if (player.ID == hit.collider.gameObject.GetComponent<PhotonView>().ownerId)
                        {
                            // ネームプレートをインスタンス化する
                            namePlateInstance = GameObject.Instantiate(namePlate);
                            // ネームプレートの名前を変更する
                            namePlateInstance.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = player.name;

                            // 右クリックされた時の処理
                            if (rightClickFlag)
                            {
                                targetPlayer = hit.collider.gameObject;     // 送り先を格納しておく
                                optionWindowInstance = StaticMethods.GameObjectInstantiate(optionWindow, Input.mousePosition);
                                optionWindowInstance.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => partySystem.SetTarget(targetPlayer));
                                optionWindowInstance.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => partySystem.SetOwner(playerObject));
                                optionWindowInstance.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => partySystem.PushInviteButton());
                                optionWindowInstance.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => GameObject.Destroy(optionWindowInstance));
                            }
                        }
                    }
                }
            }
            // オブジェクトが存在していなければ
            else
            {
                deleteNamePlate();
                DeleteOptionWindow();
            }
        }
        
    }

    /// <summary>
    /// If exist name plate instance, destroy it.
    /// </summary>
    void deleteNamePlate()
    {
        if (namePlateInstance != null)
        {
            GameObject.Destroy(namePlateInstance);
            namePlateInstance = null;
        }
    }

    /// <summary>
    /// If exists option window, destroy it.
    /// </summary>
    void DeleteOptionWindow()
    {
        if (optionWindowInstance != null)
        {
            GameObject.Destroy(optionWindowInstance);
            optionWindowInstance = null;
        }
    }
}
