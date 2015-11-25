using UnityEngine;

public class ShowMenu : MonoBehaviour {
    /// <summary>
    /// 出したオブジェクトのインスタンスの参照を登録しておく変数
    /// </summary>
    static GameObject objectInstance = null;
    /// <summary>
    /// 現在表示中のメニューのプレハブの参照
    /// </summary>
    static GameObject nowPreviewPrefab = null;

    /// <summary>
    /// メニューを表示する関数
    /// </summary>
    /// <param name="menuPrefab">メニューのプレハブ</param>
    public void InstantiateMenu(GameObject menuPrefab)
    {
        // インスタンスが作成されていなければ
        if (!objectInstance)
        {
            // 作成する
            objectInstance = GameObject.Instantiate(menuPrefab);
            // 現在参照しているプレハブを登録する
            nowPreviewPrefab = menuPrefab;
        }
        // 現在表示中のプレハブが押されたプレハブと同じならば
        else if(nowPreviewPrefab == menuPrefab)
        {
            // 削除させる
            objectInstance.GetComponentInChildren<MenuObjects>().Destroy();
        }
        else
        {
            // 削除させる
            objectInstance.GetComponentInChildren<MenuObjects>().Destroy();
            // 作成する
            objectInstance = GameObject.Instantiate(menuPrefab);
            // 現在表示しているプレハブを更新する
            nowPreviewPrefab = menuPrefab;
        }
    }
}