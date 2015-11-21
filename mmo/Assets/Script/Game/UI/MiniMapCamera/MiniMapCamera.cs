using UnityEngine;
using System.Collections;

public class MiniMapCamera : MonoBehaviour {
    // インスペクター上から隠す
    [HideInInspector]
    public GameObject player;
    // 矢印オブジェクト
    [SerializeField, Tooltip("矢印オブジェクト")]
    private GameObject arrow;

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
            // プレイヤーの位置に移動させる
            gameObject.transform.Translate(player.transform.position.x - gameObject.transform.position.x, 0f, player.transform.position.z - gameObject.transform.position.z, Space.World);
            // 矢印オブジェクトを自分の真下に移動させる
            arrow.transform.Translate(gameObject.transform.position.x - arrow.transform.position.x, 0f, gameObject.transform.position.z - arrow.transform.position.z, Space.World);
            // 矢印オブジェクトを回転させる
            arrow.transform.rotation = player.transform.rotation;
        }
    }

    /// <summary>
    /// 拡大ボタン
    /// </summary>
    public void Expansion()
    {

    }

    /// <summary>
    /// 縮小ボタン
    /// </summary>
    public void Reduction()
    {
        // 範囲チェックを行う
        if (camera.fieldOfView < 120)
        {

        }
    }
}
