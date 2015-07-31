using UnityEngine;
using System.Collections;

public class Skill : Photon.MonoBehaviour {
    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
        if (photonView.isMine)  //ネットワークに接続しているキャラが自分自身であるかどうか
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject fireObj = PhotonNetwork.Instantiate("Magics/Fire", transform.position, Quaternion.identity, 0) as GameObject;     //炎のスキルを出す
                fireObj.GetComponent<FireShot>().SetShotVec(this.transform.rotation.eulerAngles.y + 90f);
            }
            if (Input.GetMouseButtonDown(1))
            {
                PhotonNetwork.Instantiate("Magics/Thunder", transform.position, Quaternion.identity, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PhotonNetwork.Instantiate("Magics/Burn", transform.position + Vector3.up * 1, Quaternion.identity, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PhotonNetwork.Instantiate("Magics/Flare", transform.position + Vector3.up * 1, Quaternion.identity, 0);
            }
        }
    }
}