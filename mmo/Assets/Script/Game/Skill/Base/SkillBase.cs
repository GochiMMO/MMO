﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Skill Type
/// </summary>
enum SkillType
{
    BUFF = 1,   //強化系 
    ATTACK = 2  //攻撃系 
};

/// <summary>
/// スキルデータ基本構造体
/// </summary>
public class SkillData
{
    public int id;          //識別番号 
    public string name;     //名前 
    public int type;        //スキルタイプ 
    public int lv;          //スキル Lv 
    public int sp;          //消費 SP 
    public int point;       //獲得に必要なポイント 
    public int difficult;   //難易度 
    public float attack;    //力 or 上昇 (攻撃) 
    public float defence;   //力 or 上昇 (防御) 
    public float cooltime;  //クールタイム 画像の方ではリキャストタイムと表記してある。
    public float bonus;     //ボーナス (LvUp時) 
    public float casttime;  //詠唱時間 
    public float effecttime; //持続時間 
}

public class SkillBase {

    protected SkillData skillData = new SkillData();  //  基本構造体 

    protected float coolTime;   // クールタイム計算用 
    protected float castTime;   // 詠唱時間 

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="id">アイディー .</param>
    /// <param name="lv">レベル .</param>
    /// <param name="name">名前 .</param>
    /// <param name="type">タイプ .</param>
    /// <param name="pt">ポイント .</param>
    /// <param name="df">難易度 .</param>
    /// <param name="atk">攻撃力 or 攻撃上昇値 .</param>
    /// <param name="def">防御力 or 防御上昇値 .</param>
    /// <param name="ct">クールタイム .</param>
    /// <param name="bonus">ボーナス .</param>
    /// <param name="casttime">詠唱時間 .</param>
    /// <param name="ef">持続時間 .</param>
    public SkillBase(int id, int lv, string name, int type, int pt, int df, float atk, float def, float ct, float casttime, float ef, int sp, float bonus )
    {
        skillData.id = id;
        skillData.lv = lv;
        skillData.name = name;
        skillData.type = type;
        skillData.point = pt;
        skillData.difficult = df;
        skillData.attack = atk;
        skillData.defence = def;
        skillData.cooltime = ct;
        skillData.casttime = casttime;
        skillData.bonus = bonus;
        skillData.effecttime = ef;
        skillData.sp = sp;
    }

    public string GetName() { return skillData.name; }
    public int GetId() { return skillData.id; }
    public int GetLv() { return skillData.lv; }
    public int GetSkillType() { return skillData.type; }
    public int GetSp() { return skillData.sp; }
    public int GetPoint() { return (skillData.point + skillData.difficult * skillData.lv); }
    public int GetDifficult() { return skillData.difficult; }
    public float GetAttack() { return ( skillData.attack + skillData.bonus * skillData.lv ); }
    public float GetDefence() { return skillData.defence; }
    public float GetCoolTime() { return skillData.cooltime; }
    public float GetBonus() { return skillData.bonus; }
    public float GetEffectTime() { return ( skillData.effecttime + skillData.bonus * skillData.lv ); }
    public float GetCastTime() { return skillData.casttime; }
}

/// <summary>
/// プレイヤーのスキルを管理するクラス
/// </summary>
public static class SkillControl
{
    public static Dictionary<string, int> skillNameAndId = new Dictionary<string, int>();
    public static Dictionary<int, SkillBase> skills = new System.Collections.Generic.Dictionary<int, SkillBase>(); //スキルの配列
    
    /// <summary>
    /// スキルの名前でスキルを返す関数
    /// </summary>
    /// <param name="skillName">スキルの名前</param>
    /// <returns>スキル</returns>
    public static SkillBase GetSkill(string skillName)
    {
        return skills[skillNameAndId[skillName]];
    }

    public static PlayerData playerData;  //プレイヤーデータ 
    public static int job;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    static SkillControl()
    {
        // スキルのデータを読み込む
        LoadSkillData();
    }

    /// <summary>
    /// スキルデータの読み込み
    /// </summary>
    public static void LoadSkillData()
    {
        // プレイヤーのジョブをセットする
        job = PlayerStates.playerData.job;
        var skill_data = Resources.Load<Entity_Job>("Skill/SkillData").sheets[job].list;      // 本番用

        for (int i = 0; i < skill_data.Count; i++)
        {
            // スキルをを入れ込む 
            skills.Add(skill_data[i].id, new SkillBase(
                id: skill_data[i].id,
                lv: skill_data[i].lv,
                name: skill_data[i].name,
                type: skill_data[i].type,
                pt: skill_data[i].point,
                df: skill_data[i].difficult,
                atk: skill_data[i].attack,
                def: skill_data[i].defens,
                ct: skill_data[i].cooltime,
                casttime: skill_data[i].casttime,
                ef: skill_data[i].effect,
                sp: skill_data[i].sp,
                bonus: skill_data[i].bonus
                )
            );
            // 名前とidを紐づける
            skillNameAndId.Add(skill_data[i].name, skill_data[i].id);
            // Debug.Log(skill_data[i].name); //デバッグ用
        }
    }
}
