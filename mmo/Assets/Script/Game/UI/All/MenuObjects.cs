using UnityEngine;
using System.Collections;

abstract public class MenuObjects : MonoBehaviour {
    /// <summary>
    /// 削除関数
    /// </summary>
    virtual public void Destroy()
    {
        // 自身を削除する
        GameObject.Destroy(this.gameObject.transform.root.gameObject);
    }
}
