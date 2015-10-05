﻿using UnityEngine;
using System.Collections;

public class PlayerChar : Photon.MonoBehaviour {
    PlayerData playerData;

    // Use this for initialization
    void Start () {
        playerData = PlayerStates.playerData;
    }

    /// <summary>
    /// Player data getter.
    /// </summary>
    /// <returns>player data.</returns>
    public PlayerData GetPlayerData()
    {
        return this.playerData;
    }

    /// <summary>
    /// Synchronized player data.
    /// </summary>
    /// <param name="stream">PhotonStream</param>
    /// <param name="info">PhotonMessageInfo</param>
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(playerData.name);
            stream.SendNext(playerData.Lv);
            stream.SendNext(playerData.job);
            stream.SendNext(playerData.str);
            stream.SendNext(playerData.vit);
            stream.SendNext(playerData.intelligence);
            stream.SendNext(playerData.mnd);
        }
        else
        {
            playerData.name = (string)stream.ReceiveNext();
            playerData.Lv = (int)stream.ReceiveNext();
            playerData.job = (int)stream.ReceiveNext();
            playerData.str = (int)stream.ReceiveNext();
            playerData.vit = (int)stream.ReceiveNext();
            playerData.intelligence = (int)stream.ReceiveNext();
            playerData.mnd = (int)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update () {
    
    }

    /// <summary>
    /// Save player data.
    /// </summary>
    void SavePlayerCharData()
    {
        // 座標を設定
        playerData.x = gameObject.transform.position.x;
        playerData.y = gameObject.transform.position.y;
        playerData.z = gameObject.transform.position.z;
        // ログアウトしたシーンを保存
        playerData.logoutScene = Application.loadedLevel;
        // セーブする
        SaveManager.Save<PlayerData>(playerData, playerData.name);
    }

    /// <summary>
    /// If destroy this game object, call this method once.
    /// </summary>
    void OnDestroy()
    {
        // セーブする
        SavePlayerCharData();
    }
}
