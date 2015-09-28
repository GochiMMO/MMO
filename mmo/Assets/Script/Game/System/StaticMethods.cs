﻿using UnityEngine;
using System.Collections;


public static class StaticMethods{
    /// <summary>
    /// Constractor.
    /// </summary>
    static StaticMethods() { }

    /// <summary>
    /// Destroy a game object.
    /// </summary>
    /// <param name="targetObject">Destroy object.</param>
    /// <param name="usePhotonNetworkFlag">Photon network flag.</param>
    public static void DestroyObject(GameObject targetObject, bool usePhotonNetworkFlag = false)
    {
        if (usePhotonNetworkFlag)
        {
            PhotonNetwork.Destroy(targetObject);
        }
        else
        {
            GameObject.Destroy(targetObject);
        }
    }

    /// <summary>
    /// Delete player save data in environmental save data of player number.
    /// </summary>
    /// <param name="playerNumber">Delete player number.</param>
    public static void DeletePlayerSaveData(int playerNumber)
    {
        //番号分け
        switch (playerNumber)
        {
            case 0:
                SaveManager.DeleteSaveData(PlayerStates.environmentalSaveData.oneSaveDataName);
                PlayerStates.environmentalSaveData.oneSaveDataName = null;
                PlayerStates.environmentalSaveData.saveDataNum--;
                if (PlayerStates.environmentalSaveData.saveDataNum > 1)
                {
                    PlayerStates.environmentalSaveData.oneSaveDataName = PlayerStates.environmentalSaveData.twoSaveDataName;
                    PlayerStates.environmentalSaveData.twoSaveDataName = PlayerStates.environmentalSaveData.threeSaveDataName;
                }
                else if (PlayerStates.environmentalSaveData.saveDataNum > 0)
                {
                    PlayerStates.environmentalSaveData.oneSaveDataName = PlayerStates.environmentalSaveData.twoSaveDataName;
                }
                break;

            case 1:
                SaveManager.DeleteSaveData(PlayerStates.environmentalSaveData.twoSaveDataName);
                PlayerStates.environmentalSaveData.twoSaveDataName = null;
                PlayerStates.environmentalSaveData.saveDataNum--;
                if (PlayerStates.environmentalSaveData.saveDataNum > 1)
                {
                    PlayerStates.environmentalSaveData.twoSaveDataName = PlayerStates.environmentalSaveData.threeSaveDataName;
                }
                break;

            case 2:
                SaveManager.DeleteSaveData(PlayerStates.environmentalSaveData.threeSaveDataName);
                PlayerStates.environmentalSaveData.threeSaveDataName = null;
                PlayerStates.environmentalSaveData.saveDataNum--;
                break;
        }
        PlayerStates.SaveEnvironmentalData();
    }

    /// <summary>
    /// For "PhotonNetwork.JoinOrCreateRoom()" secound parameter.
    /// </summary>
    /// <param name="isVisibled">Other players are able to show flag.</param>
    /// <param name="isOpen">Ohter players are able to into this room flag.</param>
    /// <param name="maxPlayer">This room can into players number.</param>
    /// <returns></returns>
    public static RoomOptions createRoomOptions(bool isVisibled = true, bool isOpen = true, byte maxPlayer = 10)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.isVisible = isVisibled;
        roomOptions.isOpen = isOpen;
        roomOptions.maxPlayers = maxPlayer;
        roomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "CustomProperties", "カスタムプロパティ" } };
        roomOptions.customRoomPropertiesForLobby = new string[] { "CustomProperties" };
        return roomOptions;
    }
}
