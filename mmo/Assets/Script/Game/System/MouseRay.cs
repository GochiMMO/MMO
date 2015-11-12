using UnityEngine;

public class MouseRay : MonoBehaviour {
    [SerializeField, Tooltip("名前を表示させるネームプレート")]
    GameObject namePlate;
    [SerializeField, Tooltip("右クリックしたときのオプションウィンドウ")]
    GameObject optionWindow;
    [SerializeField, Tooltip("チャットのためのスクリプトを登録")]
    ChatWindow chatWindow;

    Ray ray;
    RaycastHit hit;
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
        ray = new Ray();
        hit = new RaycastHit();
        
    }
    
    // Update is called once per frame
    void Update () {
        // クリックしたかのフラグを入れる
        leftClickFlag = Input.GetMouseButtonDown(0);
        rightClickFlag = Input.GetMouseButtonDown(1);

        // クリックされたとき
        if (leftClickFlag || rightClickFlag)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool rayHitFlag = Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity);  //レイを飛ばし、オブジェクトが存在するか調べる

            // 右クリックオプションウインドウを削除する
            DeleteOptionWindow();

            // マウスクリックした場所からレイを飛ばし、オブジェクトがあるかどうか
            if (rayHitFlag)
            {
                // ネームプレートを削除する
                deleteNamePlate();

                // ヒットしたオブジェクトが敵ならば
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    // ネームプレートをインスタンス化する
                    namePlateInstance = GameObject.Instantiate(namePlate);

                    // ネームプレートに(Clone)を外した文字列をいれる
                    namePlateInstance.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = hit.collider.gameObject.GetComponent<EnemyData>().ToString();
                }

                // プレイヤーなら
                else if (hit.collider.gameObject.tag == "Player")
                {
                    // プレイヤーが設定されていない時
                    if (!playerObject)
                    {
                        // プレイヤーを検索し、設定する
                        playerObject = StaticMethods.FindGameObjectWithPhotonNetworkIDAndObjectTag(PhotonNetwork.player.ID, "Player");
                    }

                    // ネームプレートをインスタンス化する
                    namePlateInstance = GameObject.Instantiate(namePlate);

                    // ネームプレートの名前を変更する
                    namePlateInstance.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = hit.collider.gameObject.GetPhotonView().owner.name;

                    // 右クリックされ、かつ自分以外のプレイヤーだった場合
                    if (rightClickFlag && hit.collider.gameObject.GetPhotonView().ownerId != PhotonNetwork.player.ID)
                    {
                        targetPlayer = hit.collider.gameObject;     // 送り先を格納しておく

                        // 右クリックオプションウインドウの作成
                        optionWindowInstance = GameObject.Instantiate(optionWindow);

                        // 右クリックされた座標に移動
                        optionWindowInstance.transform.GetChild(0).position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

                        // ボタンのメソッドを削除する
                        optionWindowInstance.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
                        // PTに誘うボタンのメソッドを登録する
                        optionWindowInstance.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => partySystem.SetTarget(targetPlayer));
                        optionWindowInstance.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => partySystem.SetOwner(playerObject));
                        optionWindowInstance.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => partySystem.PushInviteButton());
                        optionWindowInstance.transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => GameObject.Destroy(optionWindowInstance));
                        // チャットを送るボタンのメソッドを登録する
                        optionWindowInstance.transform.GetChild(0).GetChild(2).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => chatWindow.SetPtoPChat(targetPlayer.GetPhotonView().owner.name));
                        optionWindowInstance.transform.GetChild(0).GetChild(2).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => GameObject.Destroy(optionWindowInstance));
                    }
                }
            }
            // クリックされた場所にオブジェクトが存在していなければ
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
            if (!optionWindowInstance.transform.GetChild(0).GetComponent<BoxCollider2D>().OverlapPoint(Input.mousePosition))
            {
                GameObject.Destroy(optionWindowInstance);
                optionWindowInstance = null;
            }
        }
    }
}
