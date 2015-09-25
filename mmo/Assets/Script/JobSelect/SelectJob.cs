using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SelectJob : MonoBehaviour {
    [SerializeField, Tooltip("ウインドウのプレハブ")]
    GameObject window;
    [SerializeField, Tooltip("アニメーション用のモデルのプレハブ")]
    GameObject model;
    [SerializeField, Tooltip("職業の名前")]
    string jobName;
    [SerializeField, Tooltip("職業の番号")]
    int jobNumber;

    public static bool windowVisibleFlag = false;
    BoxCollider2D col;

    // Use this for initialization
    void Start () {
        col = this.gameObject.GetComponent<BoxCollider2D>();
    }
    
    // Update is called once per frame
    void Update () {
        if (col.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && !windowVisibleFlag)  //マウスカーソルが乗ってるとき
        {
            //ここにモデルのアニメーションを書く(職業毎のアニメーションをさせる為)

            //クリックされたとき
            if (Input.GetMouseButtonDown(0))
            {
                PlayerStates.playerData.job = jobNumber;
                GameObject.Instantiate(window);
                windowVisibleFlag = true;
                PlayerStates.LoadFirstStatus(jobName);
            }
        }
    }
}
