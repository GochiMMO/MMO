using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class SetSkillIcon : MonoBehaviour {
    public enum PALETTE_CODE{
        NONE = 0,
        ITEM = 1,
        SKILL = 2
    }
    public static GameObject moveImage;
    public static BoxCollider2D[] col { private set; get; }     // コライダーの配列
    public static GameObject[] skills { private set; get; }     // セットされたスキル

    public static GameObject[] itemCoolTimeObjects;     // アイテムのクールタイムオブジェクトの配列
    public static GameObject[] skillCoolTimeObjects;    // スキルのクールタイムオブジェクトの配列

    public static PALETTE_CODE[] skillorItemFlag = new PALETTE_CODE[5];

    // Use this for initialization
    void Start () {
        col = gameObject.GetComponents<BoxCollider2D>(); // コライダーを取得
        skills = new GameObject[col.Length];    // コライダーの数だけスキルをセットできる
        itemCoolTimeObjects = new GameObject[col.Length];   // コライダーの数だけクールタイム用のオブジェクトが存在する
        skillCoolTimeObjects = new GameObject[col.Length];  // コライダーの数だけスキルのクールタイムオブジェクトが存在する
        // コライダーの数だけ繰り返す
        for (int i = 0; i < col.Length; i++)
        {
            // クールタイムオブジェクトを登録する
            itemCoolTimeObjects[i] = gameObject.transform.parent.GetChild(2).GetChild(i).gameObject;
            skillCoolTimeObjects[i] = gameObject.transform.parent.GetChild(3).GetChild(i).gameObject;
        }
    }

    /// <summary>
    /// Set active of cool time object. 
    /// </summary>
    public static void GenerationItemCoolTime()
    {
        // クールタイムオブジェクトの文だけ繰り返す
        for (int i = 0; i < itemCoolTimeObjects.Length; i++)
        {
            // コードがアイテムならば
            if (skillorItemFlag[i] == PALETTE_CODE.ITEM)
            {
                itemCoolTimeObjects[i].GetComponent<UpdateItemCoolTime>().SetCoolTime(Items.coolTime);
                itemCoolTimeObjects[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// スキルのクールタイムを発生させる処理
    /// </summary>
    /// <param name="skillNumber">どのスキルか（どこのパレットか）</param>
    public static void GenerationSkillCoolTime(int skillNumber)
    {
        // クールタイムを設定する
        SyncSkillCoolTime.SetSkillCoolTime(skillNumber, SkillControl.skills[skillNumber].GetCoolTime());
    }
    
    // Update is called once per frame
    void Update () {
        // イメージ画像が存在していれば
        if (moveImage)
        {
            // ボタンを離した瞬間
            if (Input.GetMouseButtonUp(0))
            {
                // 配列分繰り返す
                for (int i = 0; i < col.Length; i++)
                {
                    // マウスのポジションがコライダーの上にあるとき
                    if (col[i].OverlapPoint(Input.mousePosition))
                    {
                        // もし先に何かスキルがセットされていたら
                        if (skills[i] != moveImage)
                        {
                            // そのスキルを削除する
                            GameObject.Destroy(skills[i]);
                        }
                        
                        // スキル配列にスキルの画像をセットする
                        skills[i] = GameObject.Instantiate(moveImage);
                        GameObject.Destroy(moveImage);
                        
                        // 親を自分に設定する
                        skills[i].transform.SetParent(this.transform);

                        // 位置を調整し、サイズを変更する
                        skills[i].transform.position = col[i].bounds.center;
                        skills[i].transform.localScale = new Vector3(1f, 1f, 1f);
                        skills[i].GetComponent<RectTransform>().sizeDelta = new Vector2(80f, 80f);

                        // 動かすコンポーネントをオフにする
                        skills[i].GetComponent<MoveSprite>().enabled = false;

                        // アイテムを使うクラスの変数定義
                        UseItem useItem;
                        // そのセットされたものがアイテムならば
                        if ((useItem = skills[i].GetComponent<UseItem>()))
                        {
                            // アイテムを使うスクリプトに番号をセットする
                            useItem.enabled = true;
                            useItem.skillPaletteNumber = i;
                            skillorItemFlag[i] = PALETTE_CODE.ITEM;

                            // アイテムの使用フラグが立っていれば
                            if (UseItem.itemCoolTimeFlag)
                            {
                                // クールタイムのオブジェクトをアクティブにする
                                itemCoolTimeObjects[i].SetActive(true);
                            }
                        }
                        else
                        {
                            // スキルを使うコンポーネントを取得する
                            UseSkill useSkill = skills[i].GetComponent<UseSkill>();
                            // セットされたものはスキルとする
                            skillorItemFlag[i] = PALETTE_CODE.SKILL;
                            // スキルがセットされているパレットの番号をセットする
                            useSkill.skillPaletteNumber = i;
                            // スキルを使えるようにする
                            useSkill.enabled = true;
                            // スキルを使うコンポーネントをセットする
                            skillCoolTimeObjects[i].GetComponent<UpdateSkillCoolTime>().useSkill = useSkill;
                            // スキルのクールタイムオブジェクトをアクティブ化する
                            skillCoolTimeObjects[i].SetActive(true);
                        }

                        // 離れるコンポーネントをオンにする
                        RemoveSprite skillComponent = skills[i].GetComponent<RemoveSprite>();
                        skillComponent.enabled = true;
                        skillComponent.parentColliders = col[i];

                        // 参照を解放する
                        moveImage = null;
                        break;
                    }
                }
            }
        }
    }
}