using UnityEngine;
using System.Collections;

public class PlayerStartUp : Photon.MonoBehaviour {
    [SerializeField, Tooltip("アーチャーのアニメーションコントローラー")]
    RuntimeAnimatorController archerAnimationController;
    [SerializeField, Tooltip("ウォーリアのアニメーションコントローラー")]
    RuntimeAnimatorController warriorAnimationController;
    [SerializeField, Tooltip("魔法職のアニメーションコントローラー")]
    RuntimeAnimatorController sorcererAnimationController;
    [SerializeField, Tooltip("モンクのアニメーションコントローラー")]
    RuntimeAnimatorController monkAnimationController;

    // Use this for initialization
    void Start () {
        // 自分自身ならば
        if (photonView.isMine)
        {

        }
        else
        {
            // プレイヤー処理用スクリプトが存在しなければ
            if (gameObject.GetComponent<PlayerChar>() == null)
            {
                // ジョブを送らせる
                photonView.RPC("SendMyJob", photonView.owner, PhotonNetwork.player);
            }
        }
    }
    
    /// <summary>
    /// ジョブの番号を送る関数
    /// </summary>
    /// <param name="targetPlayer">送り先</param>
    [PunRPC]
    public void SendMyJob(PhotonPlayer targetPlayer)
    {
        // ジョブの番号を送り返す
        photonView.RPC("ReciveJob", targetPlayer, PlayerStatus.playerData.job);
    }

    /// <summary>
    /// ジョブ番号を受け取る関数
    /// </summary>
    /// <param name="job">ジョブの番号</param>
    [PunRPC]
    public void ReciveJob(int job)
    {
        PlayerChar playerChar = null;
        // ジョブの番号によって処理を分ける
        switch (job)
        {
            case 0:
                // アーチャーのコンポ―ネントを入れる
                playerChar = gameObject.AddComponent<Archer>();
                // アーチャーのアニメーションを設定する
                gameObject.GetComponent<Animator>().runtimeAnimatorController = archerAnimationController;
                break;
            case 1:
                // ウォーリアのコンポ―ネントを入れる
                playerChar = gameObject.AddComponent<Warrior>();
                // ウォーリアのアニメーションを設定する
                gameObject.GetComponent<Animator>().runtimeAnimatorController = warriorAnimationController;
                break;
            case 2:
                // ソーサラーのコンポーネントを入れる
                playerChar = gameObject.AddComponent<Sorcerer>();
                // ソーサラーのアニメーションを設定する
                gameObject.GetComponent<Animator>().runtimeAnimatorController = sorcererAnimationController;
                break;
            case 3:
                // モンクのコンポーネントを入れる
                playerChar = gameObject.AddComponent<Monk>();
                // モンクのアニメーションを設定する
                gameObject.GetComponent<Animator>().runtimeAnimatorController = monkAnimationController;
                break;
        }
        // 同期対象に追加する
        photonView.ObservedComponents.Add(playerChar);
    }

    // Update is called once per frame
    void Update () {
    
    }
}
