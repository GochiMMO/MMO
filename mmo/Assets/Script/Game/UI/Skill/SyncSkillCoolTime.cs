using UnityEngine;
using System.Collections;

public static class SyncSkillCoolTime{
    /// <summary>
    /// 最大スキル数
    /// </summary>
    static int MAX_SKILL_NUM = 20;

    /// <summary>
    /// そのスキルのクールタイム
    /// </summary>
    static float[] skillCoolTime = new float[MAX_SKILL_NUM];
    /// <summary>
    /// そのスキルのクールタイムが始まった時間
    /// </summary>
    static float[] skillNowTime = new float[MAX_SKILL_NUM];
    /// <summary>
    /// スキルのクールタイムが行われているかのフラグ
    /// </summary>
    static bool[] skillCoolTimeFlag = new bool[MAX_SKILL_NUM];

    /// <summary>
    /// クールタイムが始まった時間を返す
    /// </summary>
    /// <param name="skillNumber">スキルの番号</param>
    /// <returns>クールタイムが始まった時間</returns>
    public static float GetStartTime(int skillNumber)
    {
        // 始まった時間を返す
        return skillNowTime[skillNumber];
    }

    /// <summary>
    /// クールタイムを返す
    /// </summary>
    /// <param name="skillNumber">スキルの番号</param>
    /// <returns>クールタイム</returns>
    public static float GetCoolTime(int skillNumber)
    {
        // クールタイムを返す
        return skillCoolTime[skillNumber];
    }

    /// <summary>
    /// クールタイムが同じかどうか
    /// </summary>
    /// <param name="skillID">スキルの番号</param>
    /// <param name="coolTime">クールタイム</param>
    /// <returns>同じならばtrue, 違うならfalse</returns>
    public static bool IsSameCoolTime(int skillID, float coolTime)
    {
        // 登録されているクールタイムと引数のクールタイムが同じならば
        if (skillCoolTime[skillID] == coolTime)
        {
            // trueを返す
            return true;
        }
        // falseを返す
        return false;
    }

    /// <summary>
    /// クールタイムであるかどうか
    /// </summary>
    /// <param name="skillNumber">スキルの番号</param>
    /// <returns>true:クールタイム中, false:クールタイム中ではない</returns>
    public static bool IsCoolTime(int skillNumber)
    {
        // クールタイムが行われているフラグが立っていたら
        if (skillCoolTimeFlag[skillNumber])
        {
            // スキルのクールタイムが終わっているかどうか
            if (skillCoolTime[skillNumber] + skillNowTime[skillNumber] < Time.time)
            {
                // スキルのクールタイムフラグをオフにする
                skillCoolTimeFlag[skillNumber] = false;
            }
        }
        // クールタイムが行われているかのフラグを返す
        return skillCoolTimeFlag[skillNumber];
    }

    /// <summary>
    /// スキルのクールタイムのフラグをセットする
    /// </summary>
    /// <param name="skillNumber">スキルの番号</param>
    /// <param name="flag">フラグ</param>
    public static void SetCoolTimeFlag(int skillNumber, bool flag)
    {
        skillCoolTimeFlag[skillNumber] = flag;
    }

    /// <summary>
    /// スキルのクールタイムを設定する
    /// </summary>
    /// <param name="skillNumber">スキルの番号</param>
    /// <param name="coolTime">クールタイム</param>
    /// <returns>設定できたかどうか</returns>
    public static bool SetSkillCoolTime(int skillNumber, float coolTime)
    {
        // スキルがクールタイムでなければ
        if (!skillCoolTimeFlag[skillNumber])
        {
            // クールタイムを設定する
            skillCoolTime[skillNumber] = coolTime;
            // 現在時刻を設定する
            skillNowTime[skillNumber] = Time.time;
            // クールタイムを行うフラグを立てる
            skillCoolTimeFlag[skillNumber] = true;
            // trueを返す
            return true;
        }
        // falseを返す
        return false;
    }
}
