using UnityEngine;
using System.Collections;

public class StatusMenu : MonoBehaviour {
    [SerializeField, Tooltip("ステータス分配のウインドウ")]
    GameObject statusWindowPrefab;
    [SerializeField, Tooltip("アーチャーのスキル振りのウィンドウ")]
    GameObject archerSkillWindow;
    [SerializeField, Tooltip("ウォーリアのスキル振りのウインドウ")]
    GameObject warriorSkillWindow;
    [SerializeField, Tooltip("ソーサラーのスキル振りのウィンドウ")]
    GameObject sorcererSkillWindow;
    [SerializeField, Tooltip("モンクのスキル振りのウィンドウ")]
    GameObject monkSkillWindow;

    /// <summary>
    /// その職業のスキルウィンドウ
    /// </summary>
    GameObject skillWindowPrefab;

    /// <summary>
    /// ステータスを表示するウィンドウのインスタンスの参照
    /// </summary>
    GameObject statusWindowInstance = null;

    /// <summary>
    /// スキルを表示するウィンドウのインスタンスの参照
    /// </summary>
    GameObject skillWindowInstance = null;

    /// <summary>
    /// Create instance of status window.
    /// </summary>
    public void InstanceStatusWindow()
    {
        // ステータスを表示するウィンドウのインスタンスが作成されていなければ
        if (statusWindowInstance == null)
        {
            // インスタンスを作成する
            statusWindowInstance = GameObject.Instantiate(statusWindowPrefab);
        }
    }

    /// <summary>
    /// Create instance of skill window.
    /// </summary>
    public void InstanceSkillWindow()
    {
        // スキルを表示するウィンドウのインスタンスが作成されていなければ
        if (skillWindowInstance == null)
        {
            // インスタンスを作成する
            GameObject.Instantiate(skillWindowPrefab);
        }
    }

    // Use this for initialization
    void Start () {
        // プレイヤーのジョブを取得する
        switch (StaticMethods.player.GetComponent<PlayerChar>().GetPlayerData().job)
        {
            case 0:
                // スキルのウィンドウをアーチャーに設定する
                skillWindowPrefab = archerSkillWindow;
                break;
            case 1:
                // スキルのウィンドウをウォーリアに設定する
                skillWindowPrefab = warriorSkillWindow;
                break;
            case 2:
                // スキルのウィンドウをソーサラーに設定する
                skillWindowPrefab = sorcererSkillWindow;
                break;
            case 3:
                // スキルのウィンドウをモンクに設定する
                skillWindowPrefab = monkSkillWindow;
                break;

        }
    }
    
    // Update is called once per frame
    void Update () {
    
    }
}
