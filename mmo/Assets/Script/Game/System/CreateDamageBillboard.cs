using UnityEngine;
using System.Collections;

public class CreateDamageBillboard : Photon.MonoBehaviour {
    [SerializeField]
    GameObject damageBillboard;

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    [PunRPC]
    public void DrawDamageBillboard(int damage, Vector3 position){
        GameObject dame = GameObject.Instantiate(damageBillboard, position, Quaternion.identity) as GameObject;
        dame.GetComponent<DrawDamage>().SetValue(damage);
    }
}
