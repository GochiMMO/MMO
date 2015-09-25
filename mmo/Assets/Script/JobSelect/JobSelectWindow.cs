using UnityEngine;
using System.Collections;

public class JobSelectWindow : MonoBehaviour {
    [SerializeField, Tooltip("名前入力ウインドウのプレハブ")]
    GameObject inputNameWindow;
    
    // Use this for initialization
    void Start () {
        
    }

    /// <summary>
    /// Push a button method
    /// </summary>
    public void PushYesButton()
    {
        GameObject.Instantiate(inputNameWindow);
        GameObject.Destroy(this.gameObject);
    }
    
    //いいえボタンを押した時の処理
    public void PushNoButton()
    {
        SelectJob.windowVisibleFlag = false;
        GameObject.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update () {
    
    }
}
