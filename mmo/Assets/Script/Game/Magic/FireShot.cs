using UnityEngine;
using System.Collections;

public class FireShot : Photon.MonoBehaviour {
    [SerializeField, Tooltip("１秒間に移動する距離")]
    float speed = 12f;     //移動速度

    Vector3 moveVec;        //移動方向とその力
    //Collider col;           //コライダー
    //bool shotFlag = false;  //移動するかどうか
    PhotonTransformView photonTransformView;
    Rigidbody rig;

    // Use this for initialization
    void Start () {
        //shotFlag = false;
        //col = this.GetComponent<SphereCollider>();
        photonTransformView = GetComponent<PhotonTransformView>();
        rig = GetComponent<Rigidbody>();
        if (photonView.isMine)
        {
            rig.isKinematic = false;
        }
    }

    /// <summary>
    /// collision method auto run per frame.
    /// </summary>
    /// <param name="collider">Collider class.</param>
    void OnTriggerEnter(Collider collider)
    {
        if (photonView.isMine)  //出現させたのが自分ならば処理を行う
        {
            if (collider.gameObject.tag != "Player")        //プレイヤーとは当り判定を取らない
            {
                PhotonNetwork.Destroy(this.gameObject);   //ヒットしたので自身を破壊
            }
        }
    }

    public void SetShotVec(float direction)
    {
        moveVec.x = -Mathf.Cos(direction * Mathf.PI / 180f) * speed;
        moveVec.z = Mathf.Sin(direction * Mathf.PI / 180f) * speed;
        moveVec.y = 0;
        //shotFlag = true;
    }

    // Update is called once per frame
    void Update () {
        if (photonView.isMine)
        {
            photonTransformView.SetSynchronizedValues(speed: moveVec, turnSpeed: 0);
        }
        this.transform.Translate(moveVec * Time.deltaTime, Space.World);
    }
}