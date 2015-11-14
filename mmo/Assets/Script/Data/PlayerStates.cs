using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

/// <summary>
/// プレイヤーのステータスやコンフィグファイルが格納されているクラス
/// </summary>
public static class PlayerStatus{
    /// <summary>
    /// 職業表
    /// </summary>
    public static readonly string[] JobName = { "アーチャー", "ウォーリア", "ソーサラー", "モンク" };
    /// <summary>
    /// 読込、セーブの対象になるプレイヤーのステータス
    /// </summary>
    public static PlayerData playerData;
    /// <summary>
    /// コンフィグファイル、主にセーブデータの数とセーブデータの名前が登録される
    /// </summary>
    public static EnvironmentalSaveData environmentalSaveData;
    /// <summary>
    /// 職業毎の初期ステータスが格納されている
    /// </summary>
    private static Jobs jobs;

    /// <summary>
    /// Constractor.
    /// </summary>
    static PlayerStatus()
    {
        Debug.Log("PlayerStates class Constractor");
        // プレイヤーのステータスを格納する変数の領域を確保
        playerData = new PlayerData();
        // 初期ステータスを格納する変数の領域を確保
        jobs = new Jobs();
        // コンフィグファイルを格納する変数の領域を確保
        environmentalSaveData = new EnvironmentalSaveData();
        // 各職業の初期ステータスを読み込む
        LoadFirstStatus();
        // 初期化する
        PlayerStatus.Init();
    }

    /// <summary>
    /// Initialization method.
    /// </summary>
    public static void Init()
    {
        // コンフィグファイルが存在する時
        if (SaveManager.isExistConfigFile())
        {
            // コンフィグファイルを読み込む
            environmentalSaveData = SaveManager.Load<EnvironmentalSaveData>(environmentalSaveData, "Config");
        }
        // コンフィグファイルが存在しないと時
        else
        {
            // セーブデータを作成する
            SaveManager.Save<EnvironmentalSaveData>(environmentalSaveData, "Config");
        }
    }

    /// <summary>
    /// 各職業の初期ステータスを読み込む
    /// </summary>
    private static void LoadFirstStatus()
    {
        // セーブデータを読み込む
        FileStream fs = new FileStream("./savedata/FirstStatus.xml", FileMode.Open);
        // XMLを解読して読み込む
        XmlSerializer serializer = new XmlSerializer(typeof(Jobs));
        // 職業として入れ込む
        jobs = (Jobs)serializer.Deserialize(fs);
    }

    /// <summary>
    /// Save player data for file in "./savedata/".
    /// </summary>
    /// <returns></returns>
    public static bool SavePlayerData()
    {
        // プレイヤーのデータをプレイヤーの名前でセーブする
        SaveManager.Save<PlayerData>(playerData, playerData.name);
        return true;
    }

    /// <summary>
    /// Save environmental data file.
    /// </summary>
    public static void SaveEnvironmentalData()
    {
        SaveManager.Save<EnvironmentalSaveData>(environmentalSaveData, "Config");
    }

    /// <summary>
    /// 初期ステータスを読み込む
    /// </summary>
    /// <param name="jobName">Load job name</param>
    /// <returns>True is completed but false is not completed</returns>
    public static bool LoadFirstStatus(string jobName)
    {
        // ジョブの数だけ繰り返す
        foreach (var job in jobs.job)
        {
            // ジョブの名前と引数で設定されたジョブの名前が等しければ
            if (job.name == jobName)
            {
                // 初期ステータスの読み込み
                playerData.HP = int.Parse(job.hp);
                playerData.SP = int.Parse(job.sp);
                playerData.str = int.Parse(job.str);
                playerData.vit = int.Parse(job.vit);
                playerData.intelligence = int.Parse(job.intelligence);
                playerData.mnd = int.Parse(job.mnd);
                playerData.Lv = 1;
                playerData.HP = playerData.MaxHP = 1000;
                playerData.SP = playerData.MaxSP = 100;

                //For debug.
                playerData.skillPoint = 5;
                playerData.statusPoint = 5;
                return true;
            }
        }
        return false;
    }
}
