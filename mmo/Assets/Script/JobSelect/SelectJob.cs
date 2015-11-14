﻿using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SelectJob : MonoBehaviour {
    [SerializeField, Tooltip("ウインドウのプレハブ")]
    GameObject window;
    [SerializeField, Tooltip("職業の名前")]
    string jobName;
    [SerializeField, Tooltip("職業の番号")]
    int jobNumber;

    // 出すモデルのインスタンス
    GameObject model;

    public static bool windowVisibleFlag = false;
    BoxCollider2D col;

    // Use this for initialization
    void Start () {
        // コライダーを取得する
        col = this.gameObject.GetComponent<BoxCollider2D>();
        switch (PlayerStatus.playerData.characterNumber)
        {
            case 0:
                // 青年を出す

                break;
            case 1:
                // ムキムキを出す
                model = GameObject.Instantiate(Resources.Load("Player/mukimuki")) as GameObject;
                break;
            case 2:
                // 少女を出す

                break;
            case 3:
                // ババアを出す

                break;
        }
        // モデルの位置を変更する
        model.transform.position = col.bounds.center + Vector3.back * 5f;
        // モデルのスケーリングを行う
        model.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        // 回転させる
        model.transform.Rotate(Vector3.up, 180f);
        // モデルの物理演算等をオフにする
        model.GetComponent<Rigidbody>().isKinematic = true;
        // アニメーションコンポーネントを取得する
        Animator anim = model.GetComponent<Animator>();
        // ジョブによってアニメーションを付ける
        switch (jobNumber)
        {
            case 0:
                // アーチャーのアニメーションをアタッチする
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Player/ArcherAnimation");
                
                break;
            case 1:
                // ウォーリアのアニメーションをアタッチする
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Player/WarriorAnimation");

                break;
            case 2:
                // ソーサラーのアニメーションをアタッチする
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Player/SorcererAnimation");

                break;
            case 3:
                // モンクのアニメーションをアタッチする
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Player/MonkAnimation");

                break;
        }
    }
    
    // Update is called once per frame
    void Update () {
        if (col.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && !windowVisibleFlag)  //マウスカーソルが乗ってるとき
        {
            //ここにモデルのアニメーションを書く(職業毎のアニメーションをさせる為)

            //クリックされたとき
            if (Input.GetMouseButtonDown(0))
            {
                PlayerStatus.playerData.job = jobNumber;
                GameObject.Instantiate(window);
                windowVisibleFlag = true;
            }
        }
    }
}
