using UnityEngine;
using System.Collections;

public class Meteo : Photon.MonoBehaviour {
    [SerializeField]
    ParticleSystem meteoParticle;
    [SerializeField]
    float lifeTime;

    SphereCollider col;
    bool hitFlag = false;
    float startTime = 0f;
    PhotonView pv;

    void OnParticleCollision(GameObject obj)
    {
        //衝突したパーティクルがメテオならば
        if (!hitFlag)
        {
            col.enabled = true;
            hitFlag = true;
            startTime = Time.time;
        }
    }

    // Use this for initialization
    void Awake () {
        col = gameObject.transform.parent.gameObject.GetComponent<SphereCollider>();
        col.enabled = false;
        pv = transform.parent.gameObject.GetComponent<PhotonView>();
    }
    
    // Update is called once per frame
    void Update () {
        if (hitFlag)
        {
            if (pv.isMine)
            {
                if (Time.time - startTime >= lifeTime)
                {
                    PhotonNetwork.Destroy(this.gameObject.transform.parent.gameObject);
                }
            }
        }
    }
}
