using UnityEngine;
using System.Collections;

public class CharacterSelect : MonoBehaviour {
    [SerializeField]
    GameObject ynWindow;
    [SerializeField, Tooltip("キャラクタークリエイト画面のシーンの名前")]
    string characterCreateSceneName;
    // Use this for initialization
    void Start () {
    
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