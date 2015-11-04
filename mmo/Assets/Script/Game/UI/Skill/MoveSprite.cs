using UnityEngine;
using System.Collections;

public class MoveSprite : MonoBehaviour {
    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
        // クリックが離されたら
        if (Input.GetMouseButtonUp(0))
        {
            GameObject.Destroy(gameObject);     // 自身を削除
        }
    }

    // LateUpdate is called once per after all update methods.
    void LateUpdate()
    {
        this.gameObject.transform.position = Input.mousePosition;
    }
}
