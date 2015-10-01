using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class MoveWindow : MonoBehaviour {
    [SerializeField, Tooltip("ウインドウを動かすためのコライダー")]
    BoxCollider2D titleBarCollider;

    bool catchFlag = false;
    Vector2 mouseDeltaPosition;
    Vector2 mousePastPosition;
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (titleBarCollider.OverlapPoint(Input.mousePosition) && Input.GetMouseButton(0))
        {
            catchFlag = true;
        }
        if(catchFlag && Input.GetMouseButtonUp(0)){
            catchFlag = false;
        }

        if (catchFlag)
        {
            mouseDeltaPosition.x = Input.mousePosition.x - mousePastPosition.x;
            mouseDeltaPosition.y = Input.mousePosition.y - mousePastPosition.y;
            gameObject.transform.Translate(mouseDeltaPosition, Space.World);
        }

        mousePastPosition = Input.mousePosition;
    }

}
