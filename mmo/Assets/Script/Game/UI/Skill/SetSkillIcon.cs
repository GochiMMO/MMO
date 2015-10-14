using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class SetSkillIcon : MonoBehaviour {
    BoxCollider2D col;
    Image img;

    // Use this for initialization
    void Start () {
        col = gameObject.GetComponent<BoxCollider2D>(); // コライダーを取得
        img = gameObject.GetComponent<Image>();     // 画像コンポーネントを取得
    }
    
    // Update is called once per frame
    void Update () {
        
    }
}
