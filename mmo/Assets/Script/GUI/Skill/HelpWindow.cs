using UnityEngine;
using System.Collections;

public class HelpWindow : MonoBehaviour {

    [SerializeField, Tooltip("出すスキルのメニュー")]
    GameObject SkillCanvas;

    GameObject objectInstance = null;
    BoxCollider2D boxCol;

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
            if (!objectInstance)
            {
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
