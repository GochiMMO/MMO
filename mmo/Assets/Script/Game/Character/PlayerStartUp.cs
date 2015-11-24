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

    Animator anim;
    PhotonAnimatorView animView;

    // Use this for initialization
    void Start () {
        anim = gameObject.GetComponent<Animator>();
        animView = gameObject.GetComponent<PhotonAnimatorView>();
        // 自分自身ならば
        if (photonView.isMine)
        {

        }
        else
        {
            // ジョブを送らせる
            photonView.RPC("SendMyJob", photonView.owner);
        }
    }
    
    /// <summary>
    /// ジョブの番号を送る関数
    /// </summary>
    /// <param name="targetPlayer">送り先</param>
    [PunRPC]
    public void SendMyJob(PhotonMessageInfo info)
    {
        // ジョブの番号を送り返す
        photonView.RPC("ReciveJob", info.sender, PlayerStatus.playerData.job);
        // デバッグ用
        Debug.Log(info.sender.name);
    }

    /// <summary>
    /// ジョブ番号を受け取る関数
    /// </summary>
    /// <param name="job">ジョブの番号</param>
    [PunRPC]
    public void ReciveJob(int job, PhotonMessageInfo info)
    {
        PlayerChar playerChar = null;
        // ジョブの番号によって処理を分ける
        switch (job)
        {
            case 0:
                // アーチャーのコンポ―ネントを入れる
                playerChar = gameObject.AddComponent<Archer>();
                // アーチャーのアニメーションを設定する
                anim.runtimeAnimatorController = archerAnimationController;
                break;
            case 1:
                // ウォーリアのコンポ―ネントを入れる
                playerChar = gameObject.AddComponent<Warrior>();
                // ウォーリアのアニメーションを設定する
                anim.runtimeAnimatorController = warriorAnimationController;
                break;
            case 2:
                // ソーサラーのコンポーネントを入れる
                playerChar = gameObject.AddComponent<Sorcerer>();
                // ソーサラーのアニメーションを設定する
                anim.runtimeAnimatorController = sorcererAnimationController;
                break;
            case 3:
                // モンクのコンポーネントを入れる
                playerChar = gameObject.AddComponent<Monk>();
                // モンクのアニメーションを設定する
                anim.runtimeAnimatorController = monkAnimationController;
                break;
        }
        // 同期対象に追加する
        photonView.ObservedComponents.Add(playerChar);
        // レイヤーの0番を同期に設定する
        animView.SetLayerSynchronized(0, PhotonAnimatorView.SynchronizeType.Discrete);
        // 同期設定を行う
        animView.SetParameterSynchronized("RunFlag", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
    }

    // Update is called once per frame
    void Update () {
    
    }
}
