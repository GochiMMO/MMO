using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Skill Type
/// </summary>
enum SkillType
{
    BUFF = 1,   //強化系 クポ
    ATTACK = 2  //攻撃系 クポ
};

/// <summary>
/// スキルデータ基本構造体クポ
/// </summary>
public struct SkillData
{
    public int id;          //識別番号 クポ
    public string name;     //名前 クポ
    public int type;        //スキルタイプ クポ
    public int lv;          //スキル Lv クポ
    public int sp;          //消費 SP クポ
    public int point;       //獲得に必要なポイント クポ
    public int difficult;   //難易度 クポ
    public float attack;    //力 or 上昇 (攻撃) クポ
    public float defence;   //力 or 上昇 (防御) クポ
    public float cooltime;  //クールタイム 画像の方ではリキャストタイムと表記してある。
    public float bonus;     //ボーナス (LvUp時) クポ
    public float casttime;  //詠唱時間 クポ
    public float effecttime; //持続時間 クポ
}

public class SkillBase {

    protected SkillData skillData;  //  基本構造体 クポ

    protected float coolTime;   // クールタイム計算用 クポ
    protected float castTime;   // 詠唱時間 クポ

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">アイディー クポ.</param>
    /// <param name="lv">レベル クポ.</param>
    /// <param name="name">名前 クポ.</param>
    /// <param name="type">タイプ クポ.</param>
    /// <param name="pt">ポイント クポ.</param>
    /// <param name="df">難易度 クポ.</param>
    /// <param name="atk">攻撃力 or 攻撃上昇値 クポ.</param>
    /// <param name="def">防御力 or 防御上昇値 クポ.</param>
    /// <param name="ct">クールタイム クポ.</param>
    /// <param name="bonus">ボーナス クポ.</param>
    /// <param name="casttime">詠唱時間 クポ.</param>
    /// <param name="ef">持続時間 クポ.</param>
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
    
    /// <summary>
    /// スキルの発動までにかかる時間用
    /// </summary>
    protected void castingTime()
    {

    }

    /// <summary>
    /// 攻撃力上昇用
    /// </summary>
    protected void BuffAttack()
    {

    }

    /// <summary>
    /// 防御力上昇用
    /// </summary>
    protected void BuffDefence()
    {

    }

    /// <summary>
    /// 攻撃のみ用
    /// </summary>
    protected void SkillAttack()
    {

    }
}

public static class SkillControl
{
    public static Dictionary<int, SkillBase> skills = new System.Collections.Generic.Dictionary<int, SkillBase>(); //スキルの配列クポ

    public static PlayerData playerData;  //プレイヤーデータ クポ
    public static int job;

    static SkillControl()
    {
        
        // プレイヤーのデータを取得できるようにする クポ
        foreach (var players in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (players.GetPhotonView().isMine)
            {
                playerData = players.GetComponent<PlayerChar>().GetPlayerData();
                break;
            }
        }

        // プレイヤーのジョブを取得 クポ
        job = playerData.job;
        

        //プレイヤーのジョブに応じたスキルシートを読み込む クポ
        Debug.Log("読み込み完了"); //デバッグ用
        var skill_data = Resources.Load<Entity_Job>("Skill/SkillData").sheets[job].list;      // 本番用

        // テスト用 0 = アーチャー, 1 = ウォーリアー, 2 = ソーサラー, 3 = モンク
        //var skill_data = Resources.Load<Entity_Job>("Skill/SkillData").sheets[0].list;

        
        for (int i = 0; i < skill_data.Count; i++)
        {
            // スキルをを入れ込む クポ
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
           // Debug.Log(skill_data[i].name); //デバッグ用
        }
        
    }

}
