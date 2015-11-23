using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniChatWindow : MonoBehaviour {
    [SerializeField, Tooltip("最大化ボタンコンポーネント")]
    UnityEngine.UI.Button maxButton;
    [SerializeField, Tooltip("点滅する時間")]
    float flashingSpeed = 0.5f;

    // チャットの最大化や最小化のアニメーションを行うコンポーネント
    Animator chatAnim;

    // 最小化状態かのフラグ
    bool miniFlag = false;
    // 点滅中かのフラグ
    bool flashingFlag = false;

    /// <summary>
    /// 最初に行われる処理
    /// </summary>
    void Start()
    {
        // チャットのアニメーションコンポーネントを取得する
        chatAnim = gameObject.GetComponent<Animator>();
    }

    /// <summary>
    /// チャットを受け取ったとき呼ばれる関数
    /// </summary>
    public void RecieveChat()
    {
        // 最小化されており、点滅フラグが立っていなければ
        if (miniFlag && !flashingFlag)
        {
            // 点滅中のフラグを立てる
            flashingFlag = true;
            // 点滅させるコルーチンを走らせる
            StartCoroutine(FlashingMaxButton());
        }

    }

    /// <summary>
    /// チャットボタンを点滅させる処理
    /// </summary>
    /// <returns></returns>
    IEnumerator FlashingMaxButton()
    {
        // ボタンをハイライトさせる処理を書く
        var colors = maxButton.colors;
        // 最小化されている間繰り返す
        while (miniFlag)
        {
            // 色が灰色ならば
            if (colors.normalColor.r == Color.gray.r)
            {
                // 白に変更する
                colors.normalColor = Color.white;
            }
            // 白ならば
            else
            {
                // 灰色に変更する
                colors.normalColor = Color.gray;
            }
            // 色をボタンに適用する
            maxButton.colors = colors;
            // 1秒待つ
            yield return new WaitForSeconds(flashingSpeed);
        }
        // 点滅させるフラグを折る
        flashingFlag = false;
        // 色を白に戻す
        colors.normalColor = Color.white;
        maxButton.colors = colors;
        // 処理を抜ける
        yield break;
    }

    /// <summary>
    /// 最小化ボタンを押した時の処理
    /// </summary>
    public void pushMiniButton()
    {
        // アニメーションを再生する
        chatAnim.SetTrigger("FadeOut");
        // 最小化フラグを立てる
        miniFlag = true;
    }

    /// <summary>
    /// 最大化ボタンを押した時の処理
    /// </summary>
    public void pushMaxButton()
    {
        // アニメーションを再生する
        chatAnim.SetTrigger("FadeIn");
        // 最小化フラグを折る
        miniFlag = false;
    }
}
