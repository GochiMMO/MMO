using UnityEngine;
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

    /// <summary>
    /// Instantiate game object at Vector3.zero and set name of gameObject.name.
    /// </summary>
    /// <param name="gameObject">Prefab</param>
    /// <param name="position">Instantiate position.</param>
    /// <returns>Instantiate object.</returns>
    public static GameObject GameObjectInstantiate(GameObject gameObject, Vector3 position = default(Vector3))
    {
        GameObject obj = GameObject.Instantiate(gameObject, position, Quaternion.identity) as GameObject;
        obj.name = gameObject.name;
        return obj;
    }

    /// <summary>
    /// Find game object with id and tag.
    /// </summary>
    /// <param name="ID">Object id.</param>
    /// <param name="tag">Tag name.</param>
    /// <returns>Exist gameobject, it or not exits, null.</returns>
    public static GameObject FindGameObjectWithPhotonNetworkIDAndObjectTag(int ID, string tag = default(string))
    {
        GameObject[] objects;
        // タグが設定されていたら
        if (!string.IsNullOrEmpty(tag))
        {
            // 設定されている全てのオブジェクトを取得
            objects = GameObject.FindGameObjectsWithTag(tag);
        }
        else
        {
            // ヒエラルキー上の全てのオブジェクトを取得
            objects = UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>();
        }
        // オブジェクト分だけ繰り返す
        for (int i = 0; i < objects.Length; i++ )
        {
            // PhotonViewを取得する
            PhotonView photonView = objects[i].GetPhotonView();
            // PhotonViewが存在する場合
            if (photonView)
            {
                // PhotonViewが持つIDと検索するIDが一緒ならば
                if (photonView.owner.ID == ID)
                {
                    return objects[i];
                }
            }
        }
        // IDが一致するオブジェクトが無いorヒエラルキー上にオブジェクトが無いor設定したタグのオブジェクトが無いならばnullを返す
        return null;
    }

    /// <summary>
    /// Find game ovjects with PhotonNetwork.player.name and game object's tags.
    /// </summary>
    /// <param name="name">Photon network name.</param>
    /// <param name="tag">Game object tag.</param>
    /// <returns>Game objects array.</returns>
    public static GameObject[] FindGameObjectsWithNameAndTag(string name, string tag = default(string))
    {
        // 名前と一致するオブジェクトを格納するリスト
        System.Collections.Generic.List<GameObject> objects = new System.Collections.Generic.List<GameObject>();
        // 検索して出てきたオブジェクトを入れておく一時変数
        GameObject[] targetObjects;
        // 検索するタグが設定されているかどうか
        if (!string.IsNullOrEmpty(tag))
        {
            // 設定されている全てのオブジェクトを取得
            targetObjects = GameObject.FindGameObjectsWithTag(tag);
        }
        else
        {
            // ヒエラルキー上の全てのオブジェクトを取得
            targetObjects = UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>();
        }
        // 見つかったオブジェクト分だけ繰り返す
        for (int i = 0; i < targetObjects.Length; i++)
        {
            // PhotonViewを取得する
            PhotonView photonView = targetObjects[i].GetPhotonView();
            // もしPhotonViewが存在すれば
            if (photonView)
            {
                // PhotonViewのある親の名前と検索する名前が一致し、かつ自分以外
                if (photonView.owner.name == name && photonView.owner.ID != PhotonNetwork.player.ID)
                {
                    // 配列に登録する
                    objects.Add(targetObjects[i]);
                }
            }
        }
        // オブジェクトが発見されたら
        if (objects.Count > 0)
        {
            // 配列にして返す
            return objects.ToArray();
        }
        // オブジェクトが存在しなければ
        return null;
    }

    /// <summary>
    /// Create a string of rich text from text and color class.
    /// </summary>
    /// <param name="caption">text caption.</param>
    /// <param name="color">text color.</param>
    /// <returns></returns>
    public static string CreateRichTextFromCaptionAndColor(string caption, Color color)
    {
        return "<color=#" + color.ToHexStringRGBA() + ">" + caption + "</color>";
    }

    public static string CreateRichTextFromCaptionAndColorCode(string caption, string colorCode)
    {
        return "<color=#" + colorCode + ">" + caption + "</color>";
    }

    public static string CreateRichTextFromCaptionAndColorName(string caption, string colorName)
    {
        return "<color=" + colorName + ">" + caption + "</color>";
    }
}
