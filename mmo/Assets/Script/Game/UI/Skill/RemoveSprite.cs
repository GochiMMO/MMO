using UnityEngine;
using System.Collections;

public class RemoveSprite : MonoBehaviour
{
    public BoxCollider2D parentColliders;

    bool catchFlag = false;

    // Update is called once per frame
    void Update()
    {
        // クリックされたら
        if (Input.GetMouseButtonDown(0))
        {
            // 画像の上にマウスカーソルがあれば
            if (parentColliders.OverlapPoint(Input.mousePosition))
            {
                // つかんでいるフラグをオンにする
                catchFlag = true;
                // 移動している画像をスキルスロットに教える
                SetSkillIcon.moveImage = this.gameObject;

                // そのアイコンがアイテムだった時の処理
                UseItem useItem = gameObject.GetComponent<UseItem>();
                if (useItem)
                {
                    // アイテムを使うスクリプトをオフにする
                    useItem.enabled = false;
                    // アイテムのフラグをオフにする
                    SetSkillIcon.skillorItemFlag[useItem.skillPaletteNumber] = SetSkillIcon.PALETTE_CODE.NONE;
                    // クールタイムのオブジェクトを非アクティブにする
                    SetSkillIcon.itemCoolTimeObjects[useItem.skillPaletteNumber].SetActive(false);
                }
            }
        }
        // 画像をつかんでいるフラグが立っていたら
        if (catchFlag)
        {
            // 画像の座標をマウスに合わせる
            this.transform.position = Input.mousePosition;

            // ボタンが離されたら
            if (Input.GetMouseButtonUp(0))
            {
                // スキルスロットのコライダーと重なっていたら
                foreach (var collider in SetSkillIcon.col) {
                    // コライダーと重なっていたら
                    if (collider.OverlapPoint(Input.mousePosition))
                    {
                        // つかんでいるフラグをオフにする
                        catchFlag = false;
                        // 座標をコライダーの真ん中に合わせる
                        this.transform.position = parentColliders.bounds.center;
                    }
                }
                // コライダーと重なっていなければ
                if(catchFlag)
                {
                    // 削除する
                    GameObject.Destroy(this.gameObject);
                }
            }
        }
    }
}