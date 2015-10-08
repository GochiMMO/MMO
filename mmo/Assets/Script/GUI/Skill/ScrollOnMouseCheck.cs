using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]   //当たり判定オブジェクトを自動でアタッチ
public class ScrollOnMouseCheck : MonoBehaviour {

    BoxCollider2D boxCol;

    bool mouseFlag = false;

    public bool GetMouseFlag()
    {
        return mouseFlag;
    }

    // Use this for initialization
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boxCol.OverlapPoint(Input.mousePosition))
        {
              mouseFlag = true;
        }
        else
        {
              mouseFlag = false;
        }
    }
}
