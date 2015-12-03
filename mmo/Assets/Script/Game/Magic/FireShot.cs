using UnityEngine;
using System.Collections;

public class FireShot : Photon.MonoBehaviour {
    [SerializeField, Tooltip("１秒間に移動する距離")]
    float speed = 12f;     //移動速度

    Vector3 moveVec;        //移動方向とその力
    PhotonTransformView photonTransformView;
    PhotonView pv;

    // Use this for initialization
    void Start () {
        photonTransformView = GetComponent<PhotonTransformView>();
        pv = gameObject.GetComponent<PhotonView>();
    }

    /// <summary>
    /// collision method auto run per frame.
    /// </summary>
    /// <param name="collider">Collider class.</param>
    void OnTriggerEnter(Collider collider)
    {
        if (pv.isMine)  //出現させたのが自分ならば処理を行う
        {
            if (collider.gameObject.tag != "Player")        //プレイヤーとは当り判定を取らない
            {
                PhotonNetwork.Destroy(this.gameObject);   //ヒットしたので自身を破壊
            }
        }
    }

    /// <summary>
    /// 発射する方向を設定する関数
    /// </summary>
    /// <param name="direction">方向</param>
    public void SetShotVec(Vector3 direction)
    {
        if (!pv || !photonTransformView)
        {
            Start();
        }
        this.transform.forward = direction;
        moveVec = direction * speed;
        moveVec.y = 0;
    }

    /// <summary>
    /// 発射する方向を設定する関数
    /// </summary>
    /// <param name="direction">方向</param>
    public void SetShotVec(Vector3 direction, float rotateAngle)
    {
        if (!pv || !photonTransformView)
        {
            Start();
        }
        this.transform.forward = direction;
        moveVec = direction * speed;
        moveVec.y = 0;
        transform.Rotate(Vector3.up, rotateAngle);
    }

    // Update is called once per frame
    void Update () {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }
}