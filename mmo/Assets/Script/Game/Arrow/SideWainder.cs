﻿using UnityEngine;
using System.Collections;

public class SideWainder : MonoBehaviour {
    [SerializeField, Tooltip("左前に飛ばすオブジェクト")]
    FireShot leftObj;
    [SerializeField, Tooltip("前方に飛ばすオブジェクト")]
    FireShot frontObj;
    [SerializeField, Tooltip("右前に飛ばすオブジェクト")]
    FireShot rightObj;
    [SerializeField, Tooltip("左右に広がる角度")]
    float angle = 40f;

    PlayerAttack playerAttack;

    // Use this for initialization
    void Start () {
        playerAttack = gameObject.GetComponent<PlayerAttack>();
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    /// <summary>
    /// 発射する
    /// </summary>
    /// <param name="fowardVec">前方に飛ばすオブジェクトの角度</param>
    public void SetShotVec(Vector3 fowardVec)
    {
        // 左方向に発射する
        leftObj.SetShotVec(fowardVec, -angle);
        // 前方向に発射する
        frontObj.SetShotVec(fowardVec);
        // 右方向に発射する
        rightObj.SetShotVec(fowardVec, angle);

        // 攻撃力等を設定する
        leftObj.gameObject.GetComponent<PlayerAttack>().SetProperties(playerAttack);
        frontObj.gameObject.GetComponent<PlayerAttack>().SetProperties(playerAttack);
        rightObj.gameObject.GetComponent<PlayerAttack>().SetProperties(playerAttack);
    }
}
