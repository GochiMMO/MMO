using UnityEngine;
using System.Collections;

public class RotateModel : MonoBehaviour {
    [SerializeField, Tooltip("回転するスピード(秒)")]
    float rotateSpeed;
    [SerializeField, Tooltip("回転させるためのコライダー")]
    BoxCollider2D col;
    [SerializeField, Tooltip("出現させるウインドウのプレハブ")]
    GameObject windowPrefab;

    public static bool nowActive = false;

    bool thisModelActive = false;
    Vector3 moveSpeed;
    float startTime = 0;
    float scallingValue = 0.5f;
    Vector3 firstScale;
    PushYes windowObject;

    // Use this for initialization
    void Start () {
        moveSpeed = new Vector3();
        firstScale = this.transform.localScale;
    }

    //元の場所に戻る処理
    public void MoveFirstPosition()
    {
        moveSpeed *= -1f;
        startTime = Time.time;
        StartCoroutine("MoveToFirstPos");
        windowObject.DeleteObject();    //ウインドウ削除
    }

    //元の場所に戻る処理（コルーチン）
    IEnumerator MoveToFirstPos()
    {
        while (Time.time - startTime <= 1.0f)
        {
            this.transform.Translate(moveSpeed * Time.deltaTime, Space.World);  //移動させる
            this.transform.localScale -= (firstScale * scallingValue * Time.deltaTime);
            yield return null;
        }
        thisModelActive = false;
        nowActive = false;
    }

    //こちらに来る処理（コルーチン
    IEnumerator MoveToCenter()
    {
        while (Time.time - startTime <= 1.0f)
        {
            this.transform.Translate(moveSpeed * Time.deltaTime, Space.World);  //移動させる
            this.transform.localScale += (firstScale * scallingValue * Time.deltaTime);
            yield return null;
        }

        GameObject obj = GameObject.Instantiate(windowPrefab);
        windowObject = obj.GetComponent<PushYes>();
        windowObject.parentModel = this;
    }

    // Update is called once per frame
    void Update () {
        if (col.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && !nowActive)
        {
            if(Input.GetMouseButtonDown(0)){
                Vector3 leftUpPosition = Camera.main.ScreenToWorldPoint(Vector3.zero);   //画面の左下座標を取得する
                Vector3 rightDownPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));    //右下の座標を取得する
                moveSpeed.x = (rightDownPosition.x + leftUpPosition.x) / 2f - this.transform.position.x;
                moveSpeed.y = (leftUpPosition.y + rightDownPosition.y) / 2f - this.transform.position.y;
                moveSpeed.z = -5f;
                startTime = Time.time;
                StartCoroutine("MoveToCenter");
                nowActive = true;
                thisModelActive = true;
            }
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
        else if (thisModelActive)
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }
}
