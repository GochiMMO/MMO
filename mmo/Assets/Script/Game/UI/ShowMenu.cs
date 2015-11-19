using UnityEngine;


public class ShowMenu : MonoBehaviour {
    /// <summary>
    /// 出したオブジェクトのインスタンスの参照を登録しておく変数
    /// </summary>
    static GameObject objectInstance = null;

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
        }
    }
}
