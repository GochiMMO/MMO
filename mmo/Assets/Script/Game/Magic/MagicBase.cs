using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]  //球体当り判定オブジェクトを自動アタッチ

public class MagicBase : MonoBehaviour {
    [SerializeField, Tooltip("攻撃力")]
    int attack;
    [SerializeField, Tooltip("消費SP")]
    int spendSP;
    [SerializeField, Range(0f, 2f), Tooltip("乱数による振れ幅")]
    float randomNum;

    SphereCollider sphereCollider;
    // Use this for initialization
    void Start () {
        sphereCollider = GetComponent<SphereCollider>();    //当り判定オブジェクトを取得
        sphereCollider.isTrigger = true;    //当り判定のみ検出するようにする
    }

    //振れ幅を計算し、攻撃力を返す
    public int GetAttack()
    {
        return attack + (int)(attack * Random.Range(-randomNum, randomNum));
    }

    void OnTriggerEnter(Collider col)
    {

    }
    
    // Update is called once per frame
    void Update () {
    
    }
}