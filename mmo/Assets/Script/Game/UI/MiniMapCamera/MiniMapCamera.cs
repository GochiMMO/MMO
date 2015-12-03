using UnityEngine;
using System.Collections;

public class MiniMapCamera : MonoBehaviour {
    // インスペクター上から隠す
    [HideInInspector]
    public GameObject player;
    // 矢印オブジェクト
    [SerializeField, Tooltip("矢印オブジェクト")]
    private GameObject arrow;
    [SerializeField, Tooltip("画角の広さの最大値")]
    private float maxViewPort = 120f;
    [SerializeField, Tooltip("画角の広さの最小値")]
    private float minViewPort = 60f;

    // 離すY軸距離
    float yDistance = -1f;

    new private Camera camera;

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
            // 回転を合わせる
            arrow.transform.Rotate(Vector3.up, 180f, Space.World);
        }
    }

    /// <summary>
    /// 視覚範囲をヌラーって変える関数
    /// </summary>
    /// <param name="value">変える量</param>
    /// <returns>反復子</returns>
    IEnumerator ChangeFieldOfView(float value)
    {
        // 開始時刻を記録する
        float startTime = Time.time;
        // 1秒間繰り返す
        while (startTime + 1f > Time.time)
        {
            // カメラの画角を変える
            camera.fieldOfView += value * Time.deltaTime;
            // 画角が上限に達していたら
            if (camera.fieldOfView >= maxViewPort)
            {
                // カメラの画角を最大に設定する
                camera.fieldOfView = maxViewPort;
                // 処理から抜ける
                yield break;
            }
            // 画角が下限に達していたら
            else if (camera.fieldOfView <= minViewPort)
            {
                // カメラの画角を最小に設定する
                camera.fieldOfView = minViewPort;
                // 処理から抜ける
                yield break;
            }
            yield return null;
        }
        // コルーチンから抜ける
        yield break;
    }

    /// <summary>
    /// 拡大ボタン
    /// </summary>
    public void Expansion()
    {
        // 範囲チェックを行う
        if (camera.fieldOfView > minViewPort)
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
        if (camera.fieldOfView < maxViewPort)
        {
            // カメラの見える範囲を拡大する(つまり1キャラが小さく映る)
            StartCoroutine(ChangeFieldOfView(10f));
        }
    }
}
