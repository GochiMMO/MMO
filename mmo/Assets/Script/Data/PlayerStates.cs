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
    /// strなどのステータスによってどのくらいatk等が上がるか記載された変数
    /// </summary>
    public static readonly Entity_StatusPoint level_status;
    /// <summary>
    /// 初期ステータスが格納されている変数
    /// </summary>
    private static readonly Entity_FirstStatus firstStatus;
    /// <summary>
    /// Constractor.
    /// </summary>
    static PlayerStatus()
    {
        Debug.Log("PlayerStates class Constractor");
        // プレイヤーのステータスを格納する変数の領域を確保
        playerData = new PlayerData();
        // コンフィグファイルを格納する変数の領域を確保
        environmentalSaveData = new EnvironmentalSaveData();
        // ステータスアップによって(ry
        level_status = Resources.Load<Entity_StatusPoint>("Player/Status/Status_Level");
        // 初期ステータスを読み込む
        firstStatus = Resources.Load<Entity_FirstStatus>("Player/Status/FirstStatus");
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
    /// ステータスの再計算
    /// </summary>
    public static void UpdateStatus()
    {
        // 初期ステータスを読み込む
        Entity_FirstStatus.Param jobFirstStatus = firstStatus.sheets[playerData.job].list[0];
        // ステータスを配列にまとめる
        int[] statusValue = {playerData.str,playerData.vit, playerData.intelligence, playerData.mnd};
        // 各ステータスを反映させる
        playerData.MaxHP = jobFirstStatus.HP;
        playerData.MaxSP = jobFirstStatus.SP;
        playerData.attack = jobFirstStatus.Attack;
        playerData.defense = jobFirstStatus.Defense;
        playerData.magicAttack = jobFirstStatus.MagicAttack;
        playerData.magicDefence = jobFirstStatus.MagicDefense;
        // 現在の振れるステータスの値によってステータスを加算する
        for(int i = 0 ; i < statusValue.Length ; i++)
        {
            // 上がり幅を計算する
            playerData.MaxHP += (int)(statusValue[i] * level_status.sheets[i].list[0].HP);
            playerData.MaxSP += (int)(statusValue[i] * level_status.sheets[i].list[0].SP);
            playerData.attack += (int)(statusValue[i] * level_status.sheets[i].list[0].Attack);
            playerData.defense += (int)(statusValue[i] * level_status.sheets[i].list[0].Defense);
            playerData.magicAttack += (int)(statusValue[i] * level_status.sheets[i].list[0].MagicAttack);
            playerData.magicDefence += (int)(statusValue[i] * level_status.sheets[i].list[0].MagicDefense);
        }
    }

    /// <summary>
    /// 初期ステータスを読み込む
    /// </summary>
    /// <param name="jobName">Load job name</param>
    /// <returns>True is completed but false is not completed</returns>
    public static bool LoadFirstStatus()
    {
        // 職業、キャラクター、名前を引き継ぐ
        int job = playerData.job;
        int character = playerData.characterNumber;
        string name = (string)playerData.name.Clone();
        // 新しくインスタンスを作成する
        playerData = new PlayerData();
        // 名前を入れる
        playerData.name = name;
        // キャラクターの番号を入れる
        playerData.characterNumber = character;
        // 職業番号を入れる
        playerData.job = job;
        // 初期ステータスを読み込む
        Entity_FirstStatus.Param jobFirstStatus = firstStatus.sheets[playerData.job].list[0];
        // 各ステータスを反映させる
        playerData.HP = playerData.MaxHP = jobFirstStatus.HP;
        playerData.SP = playerData.MaxSP = jobFirstStatus.SP;
        playerData.attack = jobFirstStatus.Attack;
        playerData.defense = jobFirstStatus.Defense;
        playerData.magicAttack = jobFirstStatus.MagicAttack;
        playerData.magicDefence = jobFirstStatus.MagicDefense;
        
        // 最初にボーナスステータスポイントとボーナススキルポイントを与える
        playerData.statusPoint = 10;
        playerData.skillPoint = 10;
        playerData.Lv = 1;
        return true;
    }

    /// <summary>
    /// 次のレベルに必要な経験値を返すプロパティ
    /// </summary>
    public static int nextLevelExp
    {
        private set { }
        get
        {
            // レベルの３乗×50
            return playerData.Lv * playerData.Lv * playerData.Lv * 50;
        }
    }

    /// <summary>
    /// 追加するスキルポイントの値
    /// </summary>
    public static int addSkillPoint
    {
        private set { }
        get
        {
            // (プレイヤーのレベル ÷ 10) × 10;
            return (int)(((float)playerData.Lv / 10f) * 10f);
        }
    }
}
