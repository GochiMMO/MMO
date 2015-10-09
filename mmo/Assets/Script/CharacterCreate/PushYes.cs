using UnityEngine;
using System.Collections;

public class PushYes : MonoBehaviour {
    [SerializeField, Tooltip("はいが押された時に遷移するシーン")]
    string scene;

    public RotateModel parentModel;
    
    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    //はいが押された時の処理
    public void PushYesButton()
    {
        RotateModel.nowActive = false;  //モデルが回転できる状態に戻す
        PhotonNetwork.LoadLevel(scene);   //次のシーン（職業選択）に遷移する
    }

    //いいえが押された時の処理
    public void PushNo()
    {
        parentModel.MoveFirstPosition();    //最初の場所に戻す処理
    }

    //オブジェクトを破壊する
    public void DeleteObject()
    {
        GameObject.Destroy(this.gameObject);
    }
}
