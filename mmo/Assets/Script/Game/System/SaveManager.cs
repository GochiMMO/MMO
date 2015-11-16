using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
    public static void Save<T>(T obj, string fileName)
        where T : class
    {
        File.Delete(savePass + "\\" + fileName + ".sav");
        FileStream fs = new FileStream(savePass + "\\" + fileName + ".sav", FileMode.OpenOrCreate, FileAccess.Write);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, obj);
        fs.Close();
    }

    /// <summary>
    /// Search config file in "savePass" directory.
    /// </summary>
    /// <returns>exist is true, not exist is false</returns>
    public static bool isExistConfigFile()
    {
        string[] file = System.IO.Directory.GetFiles(savePass + "\\", "Config.sav");
        return file.Length == 0 ? false : true;
    }

    /// <summary>
    /// Delete playerName file in "savePass" directory.
    /// </summary>
    /// <param name="playerName">The name of delete player.</param>
    public static void DeleteSaveData(string playerName)
    {
        FileInfo fi = new FileInfo(savePass + "\\" + playerName + ".sav");
        fi.Delete();
        
        /*
        string[] str = Directory.GetFiles(savePass + "/", playerName + ".sav");
        foreach (string name in str)
        {
            Debug.Log(name);
        }
        System.IO.File.Delete(savePass + "/" + playerName + ".sav");
        */
    }

    /// <summary>
    /// Check exist "fileName.sav" file in "./savedata/"
    /// </summary>
    /// <param name="fileName">Name of Check File</param>
    /// <returns>True is exist, False is not exist</returns>
    public static bool isExistSaveData(string fileName)
    {
        // コンフィグファイルに登録されているプレイヤーの名前の数だけ繰り返す
        foreach (string playerName in PlayerStatus.environmentalSaveData.playerName)
        {
            // 指定名とプレイヤーの名前が一致したら
            if (fileName == playerName)
            {
                // trueを返す
                return true;
            }
        }
        // 一致しなければfalseを返す
        return false;
    }

    /// <summary>
    /// Load Method
    /// </summary>
    /// <typeparam name="T">Non-Nullable and has "[Serialize]"</typeparam>
    /// <param name="obj">A struct type of "T"</param>
    /// <param name="fileName">Load File Name</param>
    /// <returns>obj</returns>
    public static T Load<T>(T obj, string fileName)
        where T : class
    {
        FileStream fs = new FileStream(savePass + "\\" + fileName + ".sav", FileMode.Open, FileAccess.Read);
        BinaryFormatter bf = new BinaryFormatter();
        obj = (T)bf.Deserialize(fs);
        fs.Close();
        return obj;
    }
}
