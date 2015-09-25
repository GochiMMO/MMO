using UnityEngine;
using System.Collections;

public class CharacterSelect : MonoBehaviour {
    [SerializeField]
    GameObject ynWindow;
    [SerializeField, Tooltip("キャラクタークリエイト画面のシーンの名前")]
    string characterCreateSceneName;
    [SerializeField, Tooltip("削除するかどうか聞くウインドウ")]
    GameObject askDeleteWindow;
    // Use this for initialization
    void Start () {
        PlayerStates.Init();
    }

    /// <summary>
    /// Popup "ask Delete Window" and entry a character destroy method on the window's button.
    /// </summary>
    public void PushDeleteButton()
    {
        var obj = GameObject.Instantiate(askDeleteWindow);
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            GameObject childObj = obj.transform.GetChild(i).gameObject;
            if (childObj.tag == "YesButton")
            {
                var btn = childObj.GetComponent<UnityEngine.UI.Button>();
                int playerNumber = int.Parse(gameObject.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text);
                btn.onClick.AddListener(() => { StaticMethods.DeletePlayerSaveData(playerNumber); });
                btn.onClick.AddListener(() => { Application.LoadLevel(Application.loadedLevel); });
            }
        }
    }

    public void LoadCharacterCreate()
    {
        Application.LoadLevel(characterCreateSceneName);
    }

    public void EnabledYNWindow()
    {
        Instantiate(ynWindow, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {
    
    }
}