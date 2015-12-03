using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NowPoint : MonoBehaviour {

    public PlayerData data;  // プレイヤーデータ 
    private int now_skillpoint ;    // 現在所持スキルポイント

    void Set()
    {
        this.data = PlayerStatus.playerData;        
        // 初期化
        this.now_skillpoint = data.skillPoint;
    }

    void Start () {
        Set();
	}
	
	void Update () {
        //　テキストの表示
        this.GetComponent<Text>().text = now_skillpoint.ToString();
        // 現在のスキルポイントとプレイヤーの持っているポイントが異なれば
        if (this.now_skillpoint != data.skillPoint)
        {
            // ポイントを入れなおす
            this.now_skillpoint = data.skillPoint;
        }
    }

}
