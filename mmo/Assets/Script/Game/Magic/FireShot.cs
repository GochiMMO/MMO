using UnityEngine;
using System.Collections;

public class FireShot : Photon.MonoBehaviour {
    [SerializeField]
    float speed = 0.1f;     //移動速度
    [SerializeField]
    int lifeSpan = 3;       //寿命(秒)

    int lifeCount = 0;      //寿命カウンター
    Vector3 moveVec;        //移動方向とその力
    Collider col;           //コライダー
    bool shotFlag = false;  //移動するかどうか
    PhotonTransformView photonTransformView;
    Rigidbody rig;

    // Use this for initialization
    void Start () {
        //shotFlag = false;
        col = this.GetComponent<SphereCollider>();
        photonTransformView = GetComponent<PhotonTransformView>();
        rig = GetComponent<Rigidbody>();
        if (photonView.isMine)
        {
            rig.isKinematic = false;
        }
    }

    //当り判定
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
        shotFlag = true;
    }

    // Update is called once per frame
    void Update () {
        if (photonView.isMine)
        {
            photonTransformView.SetSynchronizedValues(speed: moveVec * 60f, turnSpeed: 0);
        }
        this.transform.Translate(moveVec, Space.World);
        if (lifeCount++ >= lifeSpan * 60)   //寿命が尽きたら
        {
            PhotonNetwork.Destroy(this.gameObject);     //自身を削除
            return;
        }
        
    }
}