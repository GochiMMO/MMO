using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]   //当たり判定オブジェクトを自動でアタッチ
public class OverLapPoint : MonoBehaviour {

    [SerializeField,Tooltip("出すスキルのメニュー")]
    GameObject SkillCanvas;

    GameObject objectInstance = null;
    BoxCollider2D boxCol;

	
	void Start () {
        boxCol = GetComponent<BoxCollider2D>();

        // テクスチャーを表示
        transform.Find("dummy").GetComponent<Image>().enabled = true;
    }
	
	void Update () {
        ScrollOnMouseCheck a = gameObject.transform.parent.parent.parent.gameObject.GetComponent<ScrollOnMouseCheck>();

        if (boxCol.OverlapPoint(Input.mousePosition) && a.GetMouseFlag() == true)
        {
            // オブジェクトがなければ作る
            if(!objectInstance){
                objectInstance = GameObject.Instantiate(SkillCanvas);
            }

            // テクスチャーを非表示
            transform.Find("dummy").GetComponent<Image>().enabled = false;
        }
        else
        {
            if (objectInstance)
            {
                // オブジェクトがあれば壊す
                GameObject.Destroy(objectInstance);

                // テクスチャーを表示
                transform.Find("dummy").GetComponent<Image>().enabled = true;
            }
        }
	}
}
