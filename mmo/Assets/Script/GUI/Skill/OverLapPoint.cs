using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]   //当たり判定オブジェクトを自動でアタッチ
public class OverLapPoint : MonoBehaviour {

    [SerializeField,Tooltip("出すスキルのメニュー")]
    GameObject SkillCanvas;

    GameObject objectInstance = null;
    BoxCollider2D boxCol;

	// Use this for initialization
	void Start () {
        boxCol = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        ScrollOnMouseCheck a = gameObject.transform.parent.parent.parent.gameObject.GetComponent<ScrollOnMouseCheck>();

        if (boxCol.OverlapPoint(Input.mousePosition) && a.GetMouseFlag() == true)
        {
            if(!objectInstance){
                objectInstance = GameObject.Instantiate(SkillCanvas);
            }
        }
        else
        {
            if (objectInstance)
            {
                GameObject.Destroy(objectInstance);
            }
        }
	}
}
