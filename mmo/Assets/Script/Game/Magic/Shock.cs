using UnityEngine;
using System.Collections;

public class Shock : Photon.MonoBehaviour {
    [SerializeField, Tooltip("ショットのエフェクト")]
    ParticleSystem shotEffect;
    [SerializeField, Tooltip("爆発のエフェクト")]
    ParticleSystem shockEffect;
    [SerializeField, Tooltip("ショットの当り判定")]
    SphereCollider shotCollider;
    [SerializeField, Tooltip("爆発の当り判定")]
    SphereCollider shockCollider;

    public int speed;   //移動速度
    public float shotLife;  //ショット寿命
    public float shockLife; //ショック寿命

    float startTime;    //開始時間
    Vector3 moveVector; //移動ベクトル
    bool hitFlag;       //当たったかのフラグ

    PhotonTransformView photonTransformView;
    // Use this for initialization
    void Start () {
        shockEffect.Stop();   //爆発パーティクルをストップさせる
        shockCollider.enabled = false;  //爆発当り判定をオフ
        photonTransformView = GetComponent<PhotonTransformView>();

        startTime = Time.time;  //時間を登録
    }

    //方向の登録
    public void SetDirection(float yAngle)
    {
        moveVector.y = 0f;
        moveVector.x = Mathf.Cos(yAngle * Mathf.PI / 180f) * -speed;
        moveVector.z = Mathf.Sin(yAngle * Mathf.PI / 180f) * speed;
    }

    //ヒットした時
    void OnTriggerEnter(Collider col)
    {
        if (!hitFlag && col.gameObject.tag != "Player") //プレイヤーでなく、ヒットした最初のフレームならば
        {
            hitFlag = true;
            shotEffect.Stop();
            shockEffect.Play();
            shockCollider.enabled = true;
            shotCollider.enabled = false;
            moveVector = Vector3.zero;      //移動ベクトルを消す
            startTime = Time.time;  //時間を計測しなおす
        }
    }

    // Update is called once per frame
    void Update () {
        if (!hitFlag)
        {
            if (photonView.isMine)
            {
                photonTransformView.SetSynchronizedValues(speed: moveVector, turnSpeed: 0);
            }
            this.transform.Translate(moveVector * Time.deltaTime, Space.World);
            //時間が経ったら
            if (photonView.isMine)
            {
                if (Time.time - startTime >= shotLife)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
        }
        else
        {
            if (photonView.isMine)
            {
                if (Time.time - startTime >= shockLife)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
        }
    }
}