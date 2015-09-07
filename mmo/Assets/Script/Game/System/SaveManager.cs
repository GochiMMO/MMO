using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager{
    static string savePass;

    // Use this for initialization
    static SaveManager(){
        savePass = Directory.GetCurrentDirectory();
        savePass += "\\savedata";
        Directory.CreateDirectory(savePass);
    }

    //セーブを行う関数
    //Serializeするため、Non-Nullable型であり、[Serializable]である必要がある。
    public static Nullable<T> Save<T>(T obj, string fileName)
        where T : struct
    {
        FileStream fs = new FileStream(savePass + "\\" + fileName + ".sav", FileMode.OpenOrCreate, FileAccess.Write);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, obj);
        fs.Close();
        return obj;
    }

    //ロードを行う関数
    //Serializeするため、Non-Nullable型であり、[Serializable]である必要がある。
    public static T Load<T>(T obj, string fileName)
        where T : struct
    {
        FileStream fs = new FileStream(savePass + "\\" + fileName + ".sav", FileMode.Open, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        obj = (T)bf.Deserialize(fs);
        return obj;
    }
}
