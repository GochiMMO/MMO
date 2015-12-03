using UnityEngine;
using System.Collections;

public class MoveSkills : MonoBehaviour {

    GameObject list;
    Vector3 position;
    Vector3 add_position;

	void Start () {
        // アスペクト比を合わせる
        float aspect = Screen.width / 1280f;
        // リストを探す
        list = GameObject.FindGameObjectWithTag("ListSkill");
        // 変数　positionに　リストの位置を入れる
        position = list.transform.position;

        // debug
        if (this.position.y < 300 * aspect)
        {
            // 表示位置を計算させる
            this.add_position = this.position + Vector3.up * 300f * aspect + Vector3.right * 67 * aspect;
        }
        else
        {
            // 表示位置を計算させる
            this.add_position = this.position + Vector3.down * 215f * aspect + Vector3.right * 67 * aspect;
        }

        // 表示位置を計算させる
        //this.add_position = this.position + Vector3.down * 215f * aspect + Vector3.right * 67 * aspect;

        // ウィンドウ位置を決める
        transform.position = this.add_position;
    }
}
