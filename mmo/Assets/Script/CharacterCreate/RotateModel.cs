using UnityEngine;
using System.Collections;

public class RotateModel : MonoBehaviour {
    [SerializeField, Tooltip("回転するスピード(秒)")]
    float rotateSpeed;
    [SerializeField, Tooltip("回転させるためのコライダー")]
    BoxCollider2D col;
    [SerializeField, Tooltip("出現させるウインドウのプレハブ")]
    GameObject windowPrefab;
    [SerializeField, Tooltip("キャラクターの番号")]
    int characterNumber;

    public static bool nowActive = false;

    bool thisModelActive = false;
    Vector3 moveSpeed;
    float startTime = 0;
    float scallingValue = 0.2f;
    Vector3 firstScale;
    PushYes windowObject;

    // Use this for initialization
    void Start () {
        moveSpeed = new Vector3();
        firstScale = this.transform.localScale;
    }

    /// <summary>
    /// Move to first position and start coroutine "MoveToFirstPos".
    /// </summary>
    public void MoveFirstPosition()
    {
        moveSpeed *= -1f;
        startTime = Time.time;
        StartCoroutine("MoveToFirstPos");
        windowObject.DeleteObject();    //ウインドウ削除
    }

    /// <summary>
    /// Select character move to own position from center position. (Coroutine)
    /// </summary>
    /// <returns>IEnumerator</returns>
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

    /// <summary>
    /// Selected character is to move to center position from own position. (Coroutine) 
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator MoveToCenter()
    {
        // 開始時間を設定する
        float startTime = Time.time;
        // 開始時間から1秒間繰り返す
        while (Time.time - startTime <= 1.0f)
        {
            // 移動させる
            this.transform.Translate(moveSpeed * Time.deltaTime, Space.World);
            // 拡大させる
            this.transform.localScale += (firstScale * scallingValue * Time.deltaTime);
            // 反復処理
            yield return null;
        }
        // キャラクターの番号を設定する
        PlayerStatus.playerData.characterNumber = this.characterNumber;
        // このキャラクターでよろしいですか？ウィンドウを作成し、参照を取得する
        GameObject obj = GameObject.Instantiate(windowPrefab);
        // はい、いいえボタンのコンポーネントを取得する
        windowObject = obj.GetComponent<PushYes>();
        // 最初の場所に戻すために自分(スクリプト)を登録する
        windowObject.parentModel = this;
    }

    // Update is called once per frame
    void Update () {
        // コライダーの上でクリックし、他のキャラクターが選択されていなければ
        if (col.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && !nowActive)
        {
            // 左クリックされたら
            if(Input.GetMouseButtonDown(0)){
                // 画面の左上の座標を取得する
                Vector3 leftUpPosition = Camera.main.ScreenToWorldPoint(Vector3.zero);   
                // 画面の右下の座標を取得する
                Vector3 rightDownPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
                // 左右の真ん中の位置から自分の位置までの差分を計算する
                moveSpeed.x = (rightDownPosition.x + leftUpPosition.x) / 2f - this.transform.position.x;
                // 上下の真ん中の位置から自分の位置までの差分を計算する
                moveSpeed.y = (leftUpPosition.y + rightDownPosition.y) / 8f - this.transform.position.y;
                // Z軸移動速度を設定する
                moveSpeed.z = -5f;
                // 現在の時間を入れる
                startTime = Time.time;
                // 真ん中位置に移動させるコルーチンを走らせる
                StartCoroutine("MoveToCenter");
                // キャラクターが選択されているフラグをオンにする
                nowActive = true;
                // コライダーの上にカーソルが無くても回転するようにする
                thisModelActive = true;
            }
            // コライダーの上にカーソルがあるのでモデルを回転させる
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
        // そのモデルが選択されているならば
        else if (thisModelActive)
        {
            // 回転させる
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }
}
