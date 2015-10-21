using UnityEngine;
using System.Collections;

public class UseItem : MonoBehaviour {
    [SerializeField, Tooltip("アイテムのID")]
    int itemID;
    public int skillPaletteNumber;    // スキルパレットの番号
    public static bool itemCoolTimeFlag = false;    // クールタイムかのフラグ
    // Use this for initialization
    void Start () {
    }
    
    // Update is called once per frame
    void Update () {
        // スキルの番号が押された時
        if (Input.GetKeyDown(KeyCode.Alpha1 + skillPaletteNumber))
        {
            // クールタイムのオブジェクトが存在してない時
            if (!itemCoolTimeFlag)
            {
                // アイテムを使う処理
                Debug.Log("Use " + Items.items[itemID].ToString());
                // クールタイムを発生させる
                itemCoolTimeFlag = true;
                SetSkillIcon.GenerationItemCoolTime();
                Debug.Log("クールタイムセット");
            }
        }
    }
}
