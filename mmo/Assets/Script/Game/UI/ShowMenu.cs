using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]  //球体当り判定オブジェクトを自動アタッチ
public class ShowMenu : MonoBehaviour {
    [SerializeField, Tooltip("出すメニューのプレハブ")]
    GameObject menuPrefab;

    BoxCollider2D boxCol;
    // Use this for initialization
    void Start () {
        boxCol = GetComponent<BoxCollider2D>();
    }
    
    // Update is called once per frame
    void Update () {
        if (boxCol.OverlapPoint(Input.mousePosition) && Input.GetMouseButtonDown(0))   //画像がクリックされたら
        {
            /*GameObject menuObject = */
            GameObject.Instantiate(menuPrefab);
            //menuObject.transform.SetParent(this.gameObject.transform.parent.parent);
        }
    }
}
