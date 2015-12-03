using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LvText : MonoBehaviour
{

    [SerializeField, Tooltip("スキルのID")]
    int skillId;

    int oldSkillLv;

    void TextPreview()
    {
        SkillBase skill = SkillControl.skills[skillId];

        // スキルｌｖが０の場合
        if (skill.level == 0)
        {
            this.transform.GetComponent<Text>().text = ("-");
        }
        // スキルｌｖが０ではない場合
        else
        {
            oldSkillLv = skill.GetLv();

            this.transform.GetComponent<Text>().text = skill.GetLv().ToString();
            
            // 現在のスキルレベルとoldSkillLvの中身が違った場合
            if (skill.GetLv() != oldSkillLv)
            {
                // 再度スキルｌｖを表示しなおす？
                this.transform.GetComponent<Text>().text = skill.GetLv().ToString();
            }
    
        }
    
    }

    void Start()
    {
     
    }

    void Update()
    {
        this.TextPreview();
    }
}