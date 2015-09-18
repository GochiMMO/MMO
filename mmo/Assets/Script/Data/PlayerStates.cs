using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

public static class PlayerStates{
    public static PlayerData playerData;
    public static EnvironmentalSaveData environmentalSaveData;
    public static Jobs jobs;

    /// <summary>
    /// Constractor.
    /// </summary>
    static PlayerStates()
    {
        Debug.Log("PlayerStates class Constractor");
        playerData = new PlayerData();
        jobs = new Jobs();
        environmentalSaveData = new EnvironmentalSaveData();
        if (SaveManager.isExistConfigFile())
        {
            environmentalSaveData = SaveManager.Load<EnvironmentalSaveData>(environmentalSaveData, "Config");
        }
        else
        {
            SaveManager.Save<EnvironmentalSaveData>(environmentalSaveData, "Config");
        }

        LoadFirstStatus();
    }

    /// <summary>
    /// Load first status from a file name of "./savedata/FirstStatus.xml".
    /// </summary>
    static void LoadFirstStatus()
    {
        FileStream fs = new FileStream("./savedata/FirstStatus.xml", FileMode.Open);
        XmlSerializer serializer = new XmlSerializer(typeof(Jobs));
        jobs = (Jobs)serializer.Deserialize(fs);
    }

    /// <summary>
    /// Save player data for file in "./savedata/".
    /// </summary>
    /// <returns></returns>
    public static bool SavePlayerData()
    {
        SaveManager.Save<PlayerData>(playerData, playerData.name);
        return true;
    }

    /// <summary>
    /// Save environmental data file.
    /// </summary>
    public static void SaveEnvironmentalData()
    {
        environmentalSaveData.saveDataNum++;
        SaveManager.Save<EnvironmentalSaveData>(environmentalSaveData, "Config");
    }

    /// <summary>
    /// Load first status from a member of this class.
    /// </summary>
    /// <param name="jobName">Load job name</param>
    /// <returns>True is completed but false is not completed</returns>
    public static bool LoadFirstStatus(string jobName)
    {
        foreach (var job in jobs.job)
        {
            if (job.name == jobName)
            {
                playerData.HP = int.Parse(job.hp);
                playerData.SP = int.Parse(job.sp);
                playerData.str = int.Parse(job.str);
                playerData.vit = int.Parse(job.vit);
                playerData.intelligence = int.Parse(job.intelligence);
                playerData.mnd = int.Parse(job.mnd);
                playerData.Lv = 1;
                playerData.name = job.name;
                return true;
            }
        }
        return false;
    }
}
