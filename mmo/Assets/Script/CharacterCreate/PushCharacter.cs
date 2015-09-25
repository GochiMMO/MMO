using UnityEngine;
using System.Collections;

public class PushCharacter : MonoBehaviour {
    BoxCollider2D col = null;
    // Use this for initialization
    void Start () {
        col = GetComponent<BoxCollider2D>();
    }

    
    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (col.OverlapPoint(Input.mousePosition))
            {

            }
        }
    }
}
