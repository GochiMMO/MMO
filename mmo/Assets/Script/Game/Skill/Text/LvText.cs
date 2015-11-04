using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LvText : MonoBehaviour
{

    [SerializeField, Tooltip("スキルのID")]
    int skillId;

    SkillBase skill;

    int oldSkillLv;

    void TextPreview()
    {
        // スキルｌｖが０の場合
        if (skill.GetLv() == 0)
        {
            GetComponent<Text>().text = ("-");
        }
        // スキルｌｖが０ではない場合
        else
        {
            oldSkillLv = skill.GetLv();

            GetComponent<Text>().text = skill.GetLv().ToString();
            
            // 現在のスキルレベルとoldSkillLvの中身が違った場合
            if (skill.GetLv() != oldSkillLv)
            {
                // 再度スキルｌｖを表示しなおす？
                GetComponent<Text>().text = skill.GetLv().ToString();
            }
    
        }
    
    }

    void Start()
    {
        skill = SkillControl.skills[skillId];
    }

    void Update()
    {
        this.TextPreview();

    }
}