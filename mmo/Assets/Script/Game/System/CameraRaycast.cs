using UnityEngine;
using System.Collections;

public class CameraRaycast : MonoBehaviour {
    // Update is called once per frame
    void Update()
    {
        // 左クリックされたら
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            // 光線を発射する
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // 光線が何かと当たったかをチェックする
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                // OnMouseDownメッセージを送信する
            }
        }
    }
}
