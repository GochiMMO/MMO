using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LvText : MonoBehaviour
{

    [SerializeField, Tooltip("スキルのID")]
    int skillId;

    SkillBase skill;

    int oldSkillLv;
    Text text;

    void TextPreview()
    {
        // スキルｌｖが０の場合
        if (skill.level == 0)
        {
            text.text = ("-");
        }
        // スキルｌｖが０ではない場合
        else
        {
            oldSkillLv = skill.GetLv();

            text.text = skill.GetLv().ToString();
            
            // 現在のスキルレベルとoldSkillLvの中身が違った場合
            if (skill.GetLv() != oldSkillLv)
            {
                // 再度スキルｌｖを表示しなおす？
                text.text = skill.GetLv().ToString();
            }
    
        }
    
    }

    void Start()
    {
        // 表示するスキルのデータクラスを取得する
        skill = SkillControl.skills[skillId];
        // テキストを表示するコンポーネントを取得する
        text = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        this.TextPreview();
    }
}