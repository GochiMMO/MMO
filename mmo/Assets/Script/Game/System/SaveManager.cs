using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager{
    /// <summary>
    /// Save directory pass
    /// </summary>
    static string savePass;

    /// <summary>
    /// Constractor
    /// </summary>
    static SaveManager(){
        savePass = Directory.GetCurrentDirectory();
        savePass += "\\savedata";
        Directory.CreateDirectory(savePass);
    }

    /// <summary>
    /// Save Method
    /// </summary>
    /// <typeparam name="T">Non-Nullable and has "[Serialize]"</typeparam>
    /// <param name="obj">struct type of "T"</param>
    /// <param name="fileName">Save File Name</param>
    /// <returns>obj</returns>
    public static Nullable<T> Save<T>(T obj, string fileName)
        where T : struct
    {
        FileStream fs = new FileStream(savePass + "\\" + fileName + ".sav", FileMode.OpenOrCreate, FileAccess.Write);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, obj);
        fs.Close();
        return obj;
    }

    /// <summary>
    /// Search config file in "savePass" directory.
    /// </summary>
    /// <returns>exist is true, not exist is false</returns>
    public static bool isExistConfigFile()
    {
        string[] file = System.IO.Directory.GetFiles(savePass + "/", "Config.sav");
        return file.Length == 0 ? false : true;   
    }

    /// <summary>
    /// Check exist "fileName.sav" file in "./savedata/"
    /// </summary>
    /// <param name="fileName">Name of Check File</param>
    /// <returns>True is exist, False is not exist</returns>
    public static bool isExistSaveData(string fileName)
    {
        if(PlayerStates.environmentalSaveData.oneSaveDataName == fileName ||
            PlayerStates.environmentalSaveData.twoSaveDataName == fileName ||
            PlayerStates.environmentalSaveData.threeSaveDataName == fileName)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Load Method
    /// </summary>
    /// <typeparam name="T">Non-Nullable and has "[Serialize]"</typeparam>
    /// <param name="obj">struct type of "T"</param>
    /// <param name="fileName">Load File Name</param>
    /// <returns>obj</returns>
    public static T Load<T>(T obj, string fileName)
        where T : struct
    {
        FileStream fs = new FileStream(savePass + "\\" + fileName + ".sav", FileMode.Open, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        obj = (T)bf.Deserialize(fs);
        return obj;
    }
}
