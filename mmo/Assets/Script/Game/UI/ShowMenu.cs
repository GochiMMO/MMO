using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]  // 当り判定オブジェクトを自動アタッチ
public class ShowMenu : MonoBehaviour {
    [SerializeField, Tooltip("出すメニューのプレハブ")]
    GameObject menuPrefab;

    GameObject objectInstance = null;
    BoxCollider2D boxCol;
    // Use this for initialization
    void Start () {
        boxCol = GetComponent<BoxCollider2D>();
    }
    
    // Update is called once per frame
    void Update () {
        if (boxCol.OverlapPoint(Input.mousePosition) && Input.GetMouseButtonDown(0))   //画像がクリックされたら
        {
            if (!objectInstance)
            {
                objectInstance = GameObject.Instantiate(menuPrefab);
            }
        }
    }
}
