using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillText : MonoBehaviour
{

    [SerializeField, Tooltip("スキルのID")]
    int skillId;

    public int SkillID
    {
        private set { skillId = value; }
        get { return skillId; }
    }

    int lvMax = 10; // スキルの上限Lv クポ

    /// <summary>
    /// 指定先のパス簡略化のために配列にした クポ
    /// </summary>
    string[] text_Array = new string[12] {
        "Contents/Before/Name",         // 0
        "Contents/Before/NowLv",        // 1
        "Contents/Before/NowCt",        // 2
        "Contents/Before/NowDgm",       // 3
        "Contents/Before/spValue",      // 4
        "Contents/After/Name",          // 5
        "Contents/After/NextLv",        // 6
        "Contents/After/NextCt",        // 7
        "Contents/After/NextDgm",       // 8
        "Contents/After/NextPoint",     // 9
        "Contents/After/NextSpValue",   // 10
        "Contents/After/NextPoint",     // 11
    };

    public void Text()
    {
        SkillBase skill = SkillControl.skills[skillId];

        int next_lv = skill.GetLv() + 1;       // 次のレベル表示
        float now_atk = 1 + skill.GetAttack() - skill.GetBonus();  // 現在のダメージ倍率表示
        float next_atk = 1 + skill.GetAttack();    // ｌｖUp時のダメージ倍率表示
        float now_effect = skill.GetEffectTime() - skill.GetBonus();   // 現在の効果時間表示

        if (skill.GetLv() != 0)
        {
            // 1 = バフスキル, 2 = アタックスキル, 3 = 魔法?
            if (skill.GetSkillType() == 2 || skill.GetSkillType() == 3)
            {
                // 現在のスキル情報を表示させる クポ
                transform.Find(text_Array[0]).GetComponent<Text>().text = skill.GetName();
                transform.Find(text_Array[1]).GetComponent<Text>().text = skill.GetLv().ToString();
                transform.Find(text_Array[2]).GetComponent<Text>().text = skill.GetCoolTime().ToString();
                transform.Find(text_Array[3]).GetComponent<Text>().text = now_atk.ToString("P0");
                transform.Find(text_Array[4]).GetComponent<Text>().text = skill.GetSp().ToString();

                // スキルがlv10を超えていたら クポ
                if (skill.GetLv() >= lvMax)
                {
                    //スキルの未来予知の表記をさせないようにする クポ
                    transform.Find(text_Array[5]).GetComponent<Text>().text = skill.GetName();
                    transform.Find(text_Array[6]).GetComponent<Text>().text = ("- -");
                    transform.Find(text_Array[7]).GetComponent<Text>().text = ("- -");
                    transform.Find(text_Array[8]).GetComponent<Text>().text = ("- -");
                    transform.Find(text_Array[9]).GetComponent<Text>().text = ("- - - -");
                    transform.Find(text_Array[10]).GetComponent<Text>().text = ("- -");
                }

                else
                {
                    // スキルの未来予知の表記をさせる クポ
                    transform.Find(text_Array[5]).GetComponent<Text>().text = skill.GetName();
                    transform.Find(text_Array[6]).GetComponent<Text>().text = next_lv.ToString();
                    transform.Find(text_Array[7]).GetComponent<Text>().text = skill.GetCoolTime().ToString();
                    transform.Find(text_Array[8]).GetComponent<Text>().text = next_atk.ToString("P0");
                    transform.Find(text_Array[9]).GetComponent<Text>().text = skill.GetPoint().ToString();
                    transform.Find(text_Array[10]).GetComponent<Text>().text = skill.GetSp().ToString();
                }

            }

            else if (skill.GetSkillType() == 1)  //バフならば
            {
                // 現在のスキル情報を表示させる クポ
                transform.Find(text_Array[0]).GetComponent<Text>().text = skill.GetName();
                transform.Find(text_Array[1]).GetComponent<Text>().text = skill.GetLv().ToString();
                transform.Find(text_Array[2]).GetComponent<Text>().text = skill.GetCoolTime().ToString("0.0") + (" s");
                transform.Find(text_Array[3]).GetComponent<Text>().text = now_effect.ToString("0.0") + (" s");
                transform.Find(text_Array[4]).GetComponent<Text>().text = skill.GetSp().ToString();

                // スキルがlv10を超えていたら クポ
                if (skill.GetLv() >= lvMax)
                {
                    //スキルの未来予知の表記をさせないようにする クポ
                    transform.Find(text_Array[5]).GetComponent<Text>().text = skill.GetName();
                    transform.Find(text_Array[6]).GetComponent<Text>().text = ("- -");
                    transform.Find(text_Array[7]).GetComponent<Text>().text = ("- -");
                    transform.Find(text_Array[8]).GetComponent<Text>().text = ("- -");
                    transform.Find(text_Array[9]).GetComponent<Text>().text = ("- - - -");
                    transform.Find(text_Array[10]).GetComponent<Text>().text = ("- -");
                }

                else
                {
                    // スキルの未来予知の表記をさせる クポ
                    transform.Find(text_Array[5]).GetComponent<Text>().text = skill.GetName();
                    transform.Find(text_Array[6]).GetComponent<Text>().text = (skill.GetLv() + 1).ToString();
                    transform.Find(text_Array[7]).GetComponent<Text>().text = skill.GetCoolTime().ToString("0.0") + (" s");
                    transform.Find(text_Array[8]).GetComponent<Text>().text = skill.GetEffectTime().ToString("0.0") + (" s");
                    transform.Find(text_Array[9]).GetComponent<Text>().text = skill.GetPoint().ToString();
                    transform.Find(text_Array[10]).GetComponent<Text>().text = skill.GetSp().ToString();
                }
            }
        }
        //ここにlv0の時、ｌｖ１の時のステータスを出させる
        if (skill.GetLv() == 0 && skill.GetSkillType() == 2 || skill.GetSkillType() == 3)
        {
            // 攻撃スキルの未来予知の表記をさせる クポ
            transform.Find(text_Array[5]).GetComponent<Text>().text = skill.GetName();
            transform.Find(text_Array[6]).GetComponent<Text>().text = next_lv.ToString();
            transform.Find(text_Array[7]).GetComponent<Text>().text = skill.GetCoolTime().ToString();
            transform.Find(text_Array[8]).GetComponent<Text>().text = next_atk.ToString("P0");
            transform.Find(text_Array[9]).GetComponent<Text>().text = skill.GetPoint().ToString();
            transform.Find(text_Array[10]).GetComponent<Text>().text = skill.GetSp().ToString();
        }
        else
        {
            // バフスキルの未来予知の表記をさせる クポ
            transform.Find(text_Array[5]).GetComponent<Text>().text = skill.GetName();
            transform.Find(text_Array[6]).GetComponent<Text>().text = next_lv.ToString();
            transform.Find(text_Array[7]).GetComponent<Text>().text = skill.GetCoolTime().ToString();
            transform.Find(text_Array[8]).GetComponent<Text>().text = next_atk.ToString("P0");
            transform.Find(text_Array[9]).GetComponent<Text>().text = skill.GetPoint().ToString();
            transform.Find(text_Array[10]).GetComponent<Text>().text = skill.GetSp().ToString();
        }

    }
    void Start()
    {
        Text(); // スキルのテキストを表示させる クポ
    }
        
}

