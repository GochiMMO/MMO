using UnityEngine;
using System.Collections;

public class MiniMapCamera : MonoBehaviour {
    // インスペクター上から隠す
    [HideInInspector]
    public GameObject player;
    // 矢印オブジェクト
    [SerializeField, Tooltip("矢印オブジェクト")]
    private GameObject arrow;

    // 離すY軸距離
    float yDistance = -1f;

    new private Camera camera;

    bool changeFlag = false;

    // Use this for initialization
    void Start () {
        // カメラスクリプトを取得する
        camera = gameObject.GetComponent<Camera>();
    }
    
    // Update is called once per frame
    void Update () {
        // プレイヤーが設定されていたら
        if (player != null)
        {
            // Y軸の距離が設定されていなければ
            if (yDistance == -1f)
            {
                // Y軸距離を計算する
                yDistance = player.transform.position.y - gameObject.transform.position.y;
            }
            float cameraY = gameObject.transform.position.y;

            // プレイヤーの位置に移動させる
            gameObject.transform.Translate(player.transform.position.x - gameObject.transform.position.x,  player.transform.position.y  - gameObject.transform.position.y - yDistance, player.transform.position.z - gameObject.transform.position.z, Space.World);
            // 矢印オブジェクトを自分の真下に移動させる
            arrow.transform.Translate(gameObject.transform.position.x - arrow.transform.position.x, player.transform.position.y - cameraY - yDistance, gameObject.transform.position.z - arrow.transform.position.z, Space.World);
            // 矢印オブジェクトを回転させる
            arrow.transform.rotation = player.transform.rotation;
        }
    }

    /// <summary>
    /// 視覚範囲をヌラーって変える関数
    /// </summary>
    /// <param name="value">変える量</param>
    /// <returns>反復子</returns>
    IEnumerator ChangeFieldOfView(float value)
    {
        // 変えてるフラグを立てる
        changeFlag = true;
        // 開始時刻を記録する
        float startTime = Time.time;
        // 1秒間繰り返す
        while (startTime + 1f > Time.time)
        {
            // カメラの画角を変える
            camera.fieldOfView += value * Time.deltaTime;
            yield return null;
        }
        // 処理終了
        changeFlag = false;
        // コルーチンから抜ける
        yield break;
    }

    /// <summary>
    /// 拡大ボタン
    /// </summary>
    public void Expansion()
    {
        // 範囲チェックを行う
        if (camera.fieldOfView > 60f && !changeFlag)
        {
            // カメラの見える範囲を縮小する(つまり1キャラが大きく映る)
            StartCoroutine(ChangeFieldOfView(-10f));
            
        }
    }

    /// <summary>
    /// 縮小ボタン
    /// </summary>
    public void Reduction()
    {
        // 範囲チェックを行う
        if (camera.fieldOfView < 120f && !changeFlag)
        {
            // カメラの見える範囲を拡大する(つまり1キャラが小さく映る)
            StartCoroutine(ChangeFieldOfView(10f));
        }
    }
}
